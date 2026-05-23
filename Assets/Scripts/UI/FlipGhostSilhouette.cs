using UnityEngine;
using DoubleSide.Flip;

namespace DoubleSide.UI
{
    /// <summary>
    /// Silhouette semitrasparente del player che rappresenta dove sarebbe sul lato opposto.
    /// Coordinate globali preservate dal flip (GDD §2): il ghost sta esattamente
    /// nella stessa posizione mondo del player.
    ///
    /// Colore:
    ///   - tintFree (es. ciano tenue) se il flip è libero
    ///   - tintBlocked (rosso) se ostacolo sul lato opposto
    ///   - invisibile durante l'animazione di flip (eviterebbe sovrapporsi al player visibile)
    ///
    /// Attaccare a un GameObject figlio del Player con SpriteRenderer (sprite uguale al player).
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class FlipGhostSilhouette : MonoBehaviour
    {
        [Header("Riferimenti")]
        [Tooltip("FlipController da osservare. Se vuoto cerca sul parent.")]
        [SerializeField] private FlipController flipController;

        [Header("Colori")]
        [SerializeField, Range(0f, 1f)] private float alpha = 0.35f;
        [SerializeField] private Color tintFree = new Color(0.4f, 0.9f, 1f);
        [SerializeField] private Color tintBlocked = new Color(1f, 0.3f, 0.3f);

        [Header("Comportamento")]
        [Tooltip("Nasconde il ghost durante l'animazione di flip.")]
        [SerializeField] private bool hideWhileFlipping = true;

        private SpriteRenderer sr;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (flipController == null)
                flipController = GetComponentInParent<FlipController>();
        }

        void LateUpdate()
        {
            if (flipController == null || sr == null) return;

            if (hideWhileFlipping && flipController.IsFlipping)
            {
                sr.enabled = false;
                return;
            }
            sr.enabled = true;

            Color baseTint = flipController.IsBlocked() ? tintBlocked : tintFree;
            sr.color = new Color(baseTint.r, baseTint.g, baseTint.b, alpha);
        }
    }
}
