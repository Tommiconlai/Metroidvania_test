using System.Collections.Generic;
using UnityEngine;

namespace DoubleSide.Flip
{
    /// <summary>
    /// Conosce tutti i SideObject della scena e applica il lato attivo:
    /// - mostra i Renderer del lato attivo, nasconde quelli dell'opposto;
    /// - aggiorna la collision matrix in modo che il Player attraversi solo il lato attivo;
    /// - aggiorna PlayerMovement.GroundLayer per il ground check.
    ///
    /// I collider del lato opposto restano nel mondo: serviranno al check di restrizione
    /// del flip (Step 3). Per ora questo manager espone solo ToggleSide() per validazione manuale.
    /// </summary>
    public class SideManager : MonoBehaviour
    {
        [Header("Lato di partenza")]
        [SerializeField] private FlipSide startingSide = FlipSide.SideA;

        [Header("Riferimenti")]
        [Tooltip("Player con PlayerMovement. Se vuoto, lo cerca via tag 'Player'.")]
        [SerializeField] private PlayerMovement player;

        [Header("Nomi dei layer (da Project Settings)")]
        [SerializeField] private string playerLayerName = "Player";
        [SerializeField] private string sideALayerName = "GroundA";
        [SerializeField] private string sideBLayerName = "GroundB";

        // Registro globale dei SideObject in scena.
        private static readonly HashSet<SideObject> registered = new HashSet<SideObject>();
        private static SideManager instance;

        private int playerLayer;
        private int sideALayer;
        private int sideBLayer;

        public FlipSide ActiveSide { get; private set; }
        public static SideManager Instance => instance;

        public static void Register(SideObject so)
        {
            if (so == null) return;
            registered.Add(so);
            // Se il manager esiste già, applica subito la visibility corretta.
            if (instance != null)
            {
                so.SetVisible(so.Side == instance.ActiveSide);
            }
        }

        public static void Unregister(SideObject so)
        {
            registered.Remove(so);
        }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogWarning(
                    $"Esiste già un SideManager in scena ({instance.name}). " +
                    $"Distruggo il duplicato '{name}'.", this);
                Destroy(gameObject);
                return;
            }
            instance = this;

            playerLayer = LayerMask.NameToLayer(playerLayerName);
            sideALayer = LayerMask.NameToLayer(sideALayerName);
            sideBLayer = LayerMask.NameToLayer(sideBLayerName);

            if (playerLayer < 0 || sideALayer < 0 || sideBLayer < 0)
            {
                Debug.LogError(
                    "SideManager: layer mancanti. " +
                    "Verifica che Project Settings > Tags and Layers contenga " +
                    $"'{playerLayerName}', '{sideALayerName}', '{sideBLayerName}'.", this);
            }

            if (player == null)
            {
                var go = GameObject.FindGameObjectWithTag("Player");
                if (go != null) player = go.GetComponent<PlayerMovement>();
            }
        }

        void Start()
        {
            ApplySide(startingSide);
        }

        void OnDestroy()
        {
            if (instance == this) instance = null;
        }

        /// <summary>
        /// LayerMask del lato richiesto. Usato dal FlipController per il check di restrizione.
        /// Ritorna 0 se i layer non sono stati risolti correttamente.
        /// </summary>
        public LayerMask LayerMaskFor(FlipSide s)
        {
            int layer = s == FlipSide.SideA ? sideALayer : sideBLayer;
            return layer >= 0 ? (LayerMask)(1 << layer) : (LayerMask)0;
        }

        /// <summary>Applica un lato come attivo. Idempotente.</summary>
        public void ApplySide(FlipSide newSide)
        {
            ActiveSide = newSide;

            // 1) Visibility dei SideObject registrati.
            foreach (var so in registered)
            {
                if (so == null) continue;
                so.SetVisible(so.Side == newSide);
            }

            // 2) Collision matrix: il Player ignora il layer del lato opposto.
            if (playerLayer >= 0 && sideALayer >= 0 && sideBLayer >= 0)
            {
                bool aActive = newSide == FlipSide.SideA;
                Physics2D.IgnoreLayerCollision(playerLayer, sideALayer, !aActive);
                Physics2D.IgnoreLayerCollision(playerLayer, sideBLayer, aActive);
            }

            // 3) Ground check del Player: usa il layer del lato attivo.
            if (player != null)
            {
                int activeGroundLayer = newSide == FlipSide.SideA ? sideALayer : sideBLayer;
                if (activeGroundLayer >= 0)
                {
                    player.GroundLayer = 1 << activeGroundLayer;
                }
            }
        }

        /// <summary>
        /// Toggle di debug per validazione manuale dello Step 2.
        /// In Play, click destro sul componente SideManager > Toggle Side.
        /// Verrà rimpiazzato dagli eventi del FlipController nello Step 3.
        /// </summary>
        [ContextMenu("Toggle Side")]
        public void ToggleSide()
        {
            ApplySide(ActiveSide.Opposite());
            Debug.Log($"SideManager: lato attivo → {ActiveSide}");
        }
    }
}
