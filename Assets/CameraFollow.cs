using System.Collections;
using UnityEngine;
using DoubleSide.Flip;

/// <summary>
/// Segue il player e ruota di 180° in risposta agli eventi del FlipController.
/// Step 4: l'input CTRL standalone è stato rimosso; la rotazione parte solo quando
/// FlipController emette OnFlipStarted, e dura quanto FlipController.FlipDuration.
///
/// Nome classe lasciato "CameraFollow" per compatibilità con i riferimenti
/// serializzati nelle scene esistenti.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    [Tooltip("Quanto morbidamente la camera insegue il player")]
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Flip")]
    [Tooltip("FlipController di cui ascoltare gli eventi. Se vuoto, lo cerca sul target.")]
    [SerializeField] private FlipController flipController;

    [Tooltip("Curva di easing per la rotazione 180°. Identity = lineare.")]
    [SerializeField]
    private AnimationCurve flipEase = new AnimationCurve(
        new Keyframe(0f, 0f, 0f, 2f),
        new Keyframe(1f, 1f, 0f, 0f));

    private Vector3 velocity;
    private float currentAngle;     // angolo correntemente applicato alla camera
    private float baseAngle;        // angolo "a riposo" (0 o 180), aggiornato a fine flip
    private Coroutine rotationRoutine;

    void Start()
    {
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogWarning("CameraFollow: nessun GameObject con tag 'Player' trovato.");
        }

        if (flipController == null && target != null)
            flipController = target.GetComponent<FlipController>();

        if (flipController != null)
            flipController.OnFlipStarted += HandleFlipStarted;
        else
            Debug.LogWarning("CameraFollow: nessun FlipController collegato; rotazione disattivata.");
    }

    void OnDestroy()
    {
        if (flipController != null)
            flipController.OnFlipStarted -= HandleFlipStarted;
    }

    private void HandleFlipStarted(FlipSide from, FlipSide to, float duration)
    {
        // Target angle: alterna tra 0 e 180.
        float newTarget = Mathf.Approximately(baseAngle, 0f) ? 180f : 0f;

        if (rotationRoutine != null) StopCoroutine(rotationRoutine);
        rotationRoutine = StartCoroutine(RotateRoutine(baseAngle, newTarget, duration));
        baseAngle = newTarget;
    }

    private IEnumerator RotateRoutine(float fromAngle, float toAngle, float duration)
    {
        float t = 0f;
        // Mathf.LerpAngle gestisce il wrap; partiamo dall'angolo correntemente applicato
        // per evitare scatti se un nuovo flip parte prima della fine del precedente.
        float start = currentAngle;

        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);
            float eased = flipEase.Evaluate(k);
            currentAngle = Mathf.LerpAngle(start, toAngle, eased);
            yield return null;
        }

        currentAngle = toAngle;
        rotationRoutine = null;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var rotation = Quaternion.Euler(0f, currentAngle, 0f);

        // L'offset ruota attorno al player, che resta il centro della rotazione.
        var desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPosition, ref velocity, smoothTime);

        transform.rotation = rotation;
    }
}
