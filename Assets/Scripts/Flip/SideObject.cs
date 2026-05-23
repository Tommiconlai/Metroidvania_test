using UnityEngine;

namespace DoubleSide.Flip
{
    /// <summary>
    /// Marker da mettere su ogni Tilemap/ostacolo che appartiene a uno specifico lato.
    /// Si auto-registra nel SideManager.
    ///
    /// I collider restano sempre attivi (servono al check di restrizione del flip);
    /// solo i Renderer vengono spenti/accesi quando il lato non è quello visibile.
    /// </summary>
    [DisallowMultipleComponent]
    public class SideObject : MonoBehaviour
    {
        [Tooltip("A quale lato appartiene questo oggetto.")]
        [SerializeField] private FlipSide side = FlipSide.SideA;

        [Tooltip("Forza il layer del GameObject in base al Side (GroundA/GroundB). Evita errori umani nel setup.")]
        [SerializeField] private bool autoAssignLayer = true;

        [SerializeField] private string sideALayerName = "GroundA";
        [SerializeField] private string sideBLayerName = "GroundB";

        private Renderer[] cachedRenderers;

        public FlipSide Side => side;

        /// <summary>
        /// Setter pubblico per builder/spawner. Riallinea layer se autoAssignLayer è on.
        /// Da usare prima di OnEnable (typically subito dopo AddComponent).
        /// </summary>
        public void SetSide(FlipSide newSide)
        {
            side = newSide;
            if (autoAssignLayer)
            {
                string layerName = side == FlipSide.SideA ? sideALayerName : sideBLayerName;
                int layer = LayerMask.NameToLayer(layerName);
                if (layer >= 0)
                {
                    gameObject.layer = layer;
                    SetLayerRecursive(transform, layer);
                }
            }
        }

        void Awake()
        {
            cachedRenderers = GetComponentsInChildren<Renderer>(includeInactive: true);

            if (autoAssignLayer)
            {
                string layerName = side == FlipSide.SideA ? sideALayerName : sideBLayerName;
                int layer = LayerMask.NameToLayer(layerName);
                if (layer < 0)
                {
                    Debug.LogError($"SideObject '{name}': layer '{layerName}' non trovato in Tags and Layers.", this);
                }
                else if (gameObject.layer != layer)
                {
                    Debug.Log($"SideObject '{name}': layer riallineato a '{layerName}' ({layer}).", this);
                    gameObject.layer = layer;
                    // Allinea anche i figli (collider su child sono comuni).
                    SetLayerRecursive(transform, layer);
                }
            }
        }

        private static void SetLayerRecursive(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursive(t.GetChild(i), layer);
        }

        void OnValidate()
        {
            // In Editor (non-play): se autoAssignLayer attivo, applica subito così è visibile in inspector.
            if (!autoAssignLayer) return;
            string layerName = side == FlipSide.SideA ? sideALayerName : sideBLayerName;
            int layer = LayerMask.NameToLayer(layerName);
            if (layer >= 0 && gameObject.layer != layer)
            {
                gameObject.layer = layer;
            }
        }

        void OnEnable()
        {
            SideManager.Register(this);
        }

        void OnDisable()
        {
            SideManager.Unregister(this);
        }

        /// <summary>Mostra o nasconde i Renderer. I collider restano attivi.</summary>
        public void SetVisible(bool visible)
        {
            if (cachedRenderers == null) return;
            foreach (var r in cachedRenderers)
            {
                if (r != null) r.enabled = visible;
            }
        }
    }
}
