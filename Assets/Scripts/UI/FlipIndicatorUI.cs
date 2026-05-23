using UnityEngine;
using UnityEngine.UI;
using DoubleSide.Flip;

namespace DoubleSide.UI
{
    /// <summary>
    /// Icona UI che mostra lo stato del flip:
    ///   - Verde: flip disponibile
    ///   - Rosso: flip bloccato (ostacolo sul lato opposto)
    ///   - Giallo: flip in corso (animazione di transizione)
    ///
    /// Attaccare a un GameObject UI con un Image (es. dentro un Canvas Screen Space - Overlay).
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class FlipIndicatorUI : MonoBehaviour
    {
        [Header("Riferimenti")]
        [Tooltip("FlipController da osservare. Se vuoto lo cerca via tag 'Player'.")]
        [SerializeField] private FlipController flipController;

        [Header("Colori")]
        [SerializeField] private Color freeColor = new Color(0.3f, 1f, 0.4f, 1f);
        [SerializeField] private Color blockedColor = new Color(1f, 0.25f, 0.25f, 1f);
        [SerializeField] private Color flippingColor = new Color(1f, 0.85f, 0.2f, 1f);

        [Header("Feedback bloccato")]
        [Tooltip("Quando si preme F bloccati, lampeggia di rosso intenso.")]
        [SerializeField] private float deniedFlashDuration = 0.25f;

        private Image image;
        private float deniedFlashTimer;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        void OnEnable()
        {
            if (flipController == null)
            {
                var go = GameObject.FindGameObjectWithTag("Player");
                if (go != null) flipController = go.GetComponent<FlipController>();
            }
            if (flipController != null)
            {
                flipController.OnFlipBlocked += HandleBlocked;
            }
        }

        void OnDisable()
        {
            if (flipController != null)
            {
                flipController.OnFlipBlocked -= HandleBlocked;
            }
        }

        void Update()
        {
            if (flipController == null || image == null) return;

            Color target;
            if (deniedFlashTimer > 0f)
            {
                deniedFlashTimer -= Time.deltaTime;
                // Lampeggio: alterna pieno → blocked
                target = Color.Lerp(blockedColor, Color.white, Mathf.PingPong(deniedFlashTimer * 12f, 1f));
            }
            else if (flipController.IsFlipping)
            {
                target = flippingColor;
            }
            else if (flipController.IsBlocked())
            {
                target = blockedColor;
            }
            else
            {
                target = freeColor;
            }

            image.color = target;
        }

        private void HandleBlocked(FlipSide attemptedSide)
        {
            deniedFlashTimer = deniedFlashDuration;
        }
    }
}
