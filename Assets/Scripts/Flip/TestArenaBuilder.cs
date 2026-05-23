using UnityEngine;

namespace DoubleSide.Flip
{
    /// <summary>
    /// Genera proceduralmente una test arena con 4 scenari di flip per validare il game feel.
    ///
    /// Layout (vista laterale, X cresce verso destra):
    ///
    ///   Z1 = SPAWN: flip libero su entrambi i lati (pavimento A e B coincidenti).
    ///   Z2 = BLOCCATO: ostacolo SideB sovrapposto al pavimento A.
    ///                  Premi F qui → indicator rosso, niente animazione.
    ///   Z3 = PASSAGGIO ASIMMETRICO 1: muro SideA chiude il corridoio,
    ///                  su B il corridoio è libero. Devi essere su B per passare.
    ///   Z4 = PASSAGGIO ASIMMETRICO 2: muro SideB chiude il corridoio,
    ///                  su A il corridoio è libero. Devi essere su A per passare.
    ///
    /// Uso: aggiungere questo componente a un GameObject vuoto "TestArena" nella scena,
    /// trascinare uno Sprite quadrato in `blockSprite`, click destro sul componente →
    /// "Build Test Arena". Per pulire: "Clear Test Arena".
    /// </summary>
    public class TestArenaBuilder : MonoBehaviour
    {
        [Header("Asset")]
        [Tooltip("Sprite quadrato da usare per tutti i blocchi (es. quello già usato dai floor esistenti).")]
        [SerializeField] private Sprite blockSprite;

        [Header("Colori")]
        [SerializeField] private Color colorA = new Color(0.85f, 0.3f, 0.3f);
        [SerializeField] private Color colorB = new Color(0.3f, 0.5f, 0.95f);

        [Header("Sorting")]
        [SerializeField] private int sortingOrder = 0;

        [ContextMenu("Build Test Arena")]
        public void Build()
        {
            if (blockSprite == null)
            {
                Debug.LogError("TestArenaBuilder: assegna 'blockSprite' prima di buildare.", this);
                return;
            }

            ClearInternal();

            // ===== Pavimenti continui A e B (sovrapposti) =====
            // Larghezza totale 40, altezza 1, y=-3. Stessa coord per entrambi i lati:
            // il flip è libero camminando sopra ad essi.
            CreateBlock("Floor A", size: new Vector2(40f, 1f), pos: new Vector2(20f, -3f), FlipSide.SideA);
            CreateBlock("Floor B", size: new Vector2(40f, 1f), pos: new Vector2(20f, -3f), FlipSide.SideB);

            // ===== Z2 — Test BLOCCATO =====
            // Ostacolo B a x=8, alla stessa altezza del player. Camminando su A passi sopra
            // un pavimento normale; tentando flip → ostacolo B nello spazio → bloccato.
            CreateBlock("Z2 - Wall B (blocking flip)", size: new Vector2(2f, 3f), pos: new Vector2(8f, -1f), FlipSide.SideB);

            // ===== Z3 — Passaggio: muro A, vuoto su B =====
            // Muro alto su A a x=14. Su B niente: per passare devi flippare PRIMA del muro
            // (in zona libera, es. x=12), avanzare su B, poi tornare ad A se vuoi.
            CreateBlock("Z3 - Wall A (path blocked on A)", size: new Vector2(2f, 5f), pos: new Vector2(14f, 0f), FlipSide.SideA);

            // ===== Z4 — Passaggio inverso: muro B, vuoto su A =====
            // Muro alto su B a x=22. Se sei su B devi flippare ad A per passare.
            CreateBlock("Z4 - Wall B (path blocked on B)", size: new Vector2(2f, 5f), pos: new Vector2(22f, 0f), FlipSide.SideB);

            // ===== Cap di destra: pareti A+B per non cadere oltre =====
            CreateBlock("End Wall A", size: new Vector2(1f, 6f), pos: new Vector2(38f, 0f), FlipSide.SideA);
            CreateBlock("End Wall B", size: new Vector2(1f, 6f), pos: new Vector2(38f, 0f), FlipSide.SideB);

            // ===== Cap di sinistra =====
            CreateBlock("Start Wall A", size: new Vector2(1f, 6f), pos: new Vector2(1f, 0f), FlipSide.SideA);
            CreateBlock("Start Wall B", size: new Vector2(1f, 6f), pos: new Vector2(1f, 0f), FlipSide.SideB);

            Debug.Log("[TestArena] Build completato. Spawn player a ~ (3, -1).", this);
        }

        [ContextMenu("Clear Test Arena")]
        public void Clear()
        {
            ClearInternal();
        }

        private void ClearInternal()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i).gameObject;
                if (Application.isPlaying)
                    Destroy(child);
                else
                    DestroyImmediate(child);
            }
        }

        private void CreateBlock(string blockName, Vector2 size, Vector2 pos, FlipSide side)
        {
            var go = new GameObject(blockName);
            go.transform.SetParent(transform, worldPositionStays: false);
            go.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = blockSprite;
            sr.color = side == FlipSide.SideA ? colorA : colorB;
            sr.sortingOrder = sortingOrder + (side == FlipSide.SideB ? -1 : 0);

            // BoxCollider2D copre 1x1 in local space; lo scaling del transform fa il resto.
            go.AddComponent<BoxCollider2D>();

            var so = go.AddComponent<SideObject>();
            so.SetSide(side);
        }
    }
}
