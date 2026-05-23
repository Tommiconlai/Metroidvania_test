using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DoubleSide.Flip
{
    /// <summary>
    /// Orchestratore del flip lato player.
    /// Step 4: state machine Idle/Flipping con animazione di transizione (~0.5s).
    /// Durante Flipping: input movimento congelato, fisica del player sospesa,
    /// switch visivo a metà animazione per coprire il pop dei renderer.
    ///
    /// Eventi pubblicati per camera, UI, ghost silhouette.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class FlipController : MonoBehaviour
    {
        [Header("Input")]
        [Tooltip("Tasto per innescare il flip. Default: F.")]
        [SerializeField] private Key flipKey = Key.F;

        [Header("Animazione")]
        [Tooltip("Durata totale dell'animazione di flip. GDD: 0.4-0.6s.")]
        [SerializeField, Range(0.1f, 1.5f)] private float flipDuration = 0.5f;

        [Header("Restrizione")]
        [Tooltip("Override delle dimensioni del box di check. Se zero, usa il bounding del Collider2D.")]
        [SerializeField] private Vector2 boxSizeOverride = Vector2.zero;

        [Header("Riferimenti")]
        [Tooltip("PlayerMovement da bloccare durante il flip. Se vuoto, lo cerca sullo stesso GameObject.")]
        [SerializeField] private PlayerMovement movement;

        [Header("Debug")]
        [SerializeField] private bool logFlips = true;
        [SerializeField] private bool drawGizmo = true;

        private Collider2D playerCollider;
        private Rigidbody2D rb;
        private Coroutine flipRoutine;
        private float cachedGravityScale;

        /// <summary>Durata totale dell'animazione, in secondi. Camera e UI la leggono per sincronizzarsi.</summary>
        public float FlipDuration => flipDuration;

        /// <summary>Lato corrente del player.</summary>
        public FlipSide CurrentSide => SideManager.Instance != null
            ? SideManager.Instance.ActiveSide
            : FlipSide.SideA;

        /// <summary>True durante l'animazione di transizione.</summary>
        public bool IsFlipping { get; private set; }

        /// <summary>True se al momento il flip è eseguibile (nessun ostacolo sul lato opposto).</summary>
        public bool CanFlipNow => !IsFlipping && !IsBlocked();

        /// <summary>Emesso all'avvio del flip. Camera ascolta per ruotare.</summary>
        public event Action<FlipSide, FlipSide, float> OnFlipStarted;
        /// <summary>Emesso a flip completato.</summary>
        public event Action<FlipSide> OnFlipCompleted;
        /// <summary>Emesso quando si tenta un flip bloccato.</summary>
        public event Action<FlipSide> OnFlipBlocked;

        void Awake()
        {
            playerCollider = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            if (movement == null) movement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            if (IsFlipping) return;

            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard[flipKey].wasPressedThisFrame)
            {
                TryFlip();
            }
        }

        /// <summary>
        /// Tenta il flip. Se bloccato emette OnFlipBlocked. Se libero parte la coroutine di animazione.
        /// </summary>
        public bool TryFlip()
        {
            if (IsFlipping) return false;
            if (SideManager.Instance == null)
            {
                Debug.LogWarning("FlipController: nessun SideManager in scena.");
                return false;
            }

            var from = SideManager.Instance.ActiveSide;
            var to = from.Opposite();

            if (IsBlocked())
            {
                if (logFlips) Debug.Log($"[Flip] BLOCKED — ostacolo su {to}");
                OnFlipBlocked?.Invoke(to);
                return false;
            }

            flipRoutine = StartCoroutine(FlipRoutine(from, to));
            return true;
        }

        private IEnumerator FlipRoutine(FlipSide from, FlipSide to)
        {
            IsFlipping = true;
            if (logFlips) Debug.Log($"[Flip] start {from} → {to} ({flipDuration:0.00}s)");

            // Sospendi la fisica del player: il flip è una "fase" — il personaggio non cade né scivola.
            if (movement != null) movement.MovementLocked = true;
            if (rb != null)
            {
                cachedGravityScale = rb.gravityScale;
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
            }

            OnFlipStarted?.Invoke(from, to, flipDuration);

            // Attendi metà animazione, poi switcha il lato: nasconde il pop dei renderer
            // dietro la rotazione della camera (game feel).
            float half = flipDuration * 0.5f;
            yield return new WaitForSeconds(half);

            SideManager.Instance.ApplySide(to);

            yield return new WaitForSeconds(flipDuration - half);

            // Ripristina fisica e input.
            if (rb != null) rb.gravityScale = cachedGravityScale;
            if (movement != null) movement.MovementLocked = false;

            IsFlipping = false;
            flipRoutine = null;

            if (logFlips) Debug.Log($"[Flip] complete → {to}");
            OnFlipCompleted?.Invoke(to);
        }

        /// <summary>Check di restrizione. Pubblico per UI / ghost silhouette.</summary>
        public bool IsBlocked()
        {
            if (SideManager.Instance == null) return false;

            var oppositeMask = SideManager.Instance.LayerMaskFor(
                SideManager.Instance.ActiveSide.Opposite());

            var size = ResolveBoxSize();
            return FlipBlockChecker.IsBlocked(transform.position, size, oppositeMask);
        }

        private Vector2 ResolveBoxSize()
        {
            if (boxSizeOverride.sqrMagnitude > 0.0001f)
                return boxSizeOverride;

            if (playerCollider != null)
            {
                var b = playerCollider.bounds.size;
                return new Vector2(b.x, b.y);
            }

            return Vector2.one;
        }

        void OnDrawGizmosSelected()
        {
            if (!drawGizmo) return;
            if (playerCollider == null) playerCollider = GetComponent<Collider2D>();

            var size = ResolveBoxSize();
            Gizmos.color = (Application.isPlaying && IsBlocked())
                ? new Color(1f, 0.2f, 0.2f, 0.6f)
                : new Color(0.2f, 1f, 0.4f, 0.4f);
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0.1f));
        }
    }
}
