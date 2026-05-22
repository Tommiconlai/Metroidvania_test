using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, -10f);

    [Tooltip("Quanto morbidamente la camera insegue il player")]
    [SerializeField] private float smoothTime = 0.15f;

    [Header("Rotazione 180°")]
    [Tooltip("Velocità della rotazione attorno al player, in gradi/secondo")]
    [SerializeField] private float rotationSpeed = 360f;

    private Vector3 velocity;
    private float targetAngle;
    private float currentAngle;

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            else
                Debug.LogWarning("CameraFollow: nessun oggetto con tag 'Player' trovato.");
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // CTRL: alterna tra 0° e 180° di rotazione attorno al player
        if (keyboard.leftCtrlKey.wasPressedThisFrame
            || keyboard.rightCtrlKey.wasPressedThisFrame)
        {
            targetAngle = Mathf.Approximately(targetAngle, 0f) ? 180f : 0f;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Avvicina gradualmente l'angolo a quello desiderato
        currentAngle = Mathf.MoveTowardsAngle(
            currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);

        // L'offset ruota attorno al player, che resta il centro della rotazione
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.SmoothDamp(
            transform.position, desiredPosition, ref velocity, smoothTime);

        // La camera stessa ruota, così la vista si capovolge
        transform.rotation = rotation;
    }
}
