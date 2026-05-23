using UnityEngine;

namespace DoubleSide.Flip
{
    /// <summary>
    /// Helper puro per il check di restrizione del flip (GDD §2 — "Regole tecniche").
    /// Fa un Physics2D.OverlapBox alla posizione del player sul layer del lato opposto:
    /// se trova qualcosa, il flip è bloccato.
    /// </summary>
    public static class FlipBlockChecker
    {
        /// <summary>
        /// Ritorna true se la posizione sul lato opposto sarebbe occupata da un ostacolo solido.
        /// </summary>
        /// <param name="worldCenter">Centro del box di test (tipicamente la posizione del player).</param>
        /// <param name="boxSize">Dimensioni del box (tipicamente il bounding del player).</param>
        /// <param name="oppositeSideMask">LayerMask del lato opposto.</param>
        /// <param name="boxAngle">Rotazione del box (di solito 0).</param>
        public static bool IsBlocked(
            Vector2 worldCenter,
            Vector2 boxSize,
            LayerMask oppositeSideMask,
            float boxAngle = 0f)
        {
            // Riduco leggermente il box per evitare falsi positivi su collider a filo.
            const float shrink = 0.02f;
            var size = new Vector2(
                Mathf.Max(0.01f, boxSize.x - shrink),
                Mathf.Max(0.01f, boxSize.y - shrink));

            return Physics2D.OverlapBox(worldCenter, size, boxAngle, oppositeSideMask) != null;
        }
    }
}
