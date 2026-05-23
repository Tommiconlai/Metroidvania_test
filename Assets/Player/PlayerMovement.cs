using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 8f;
    [Tooltip("Quanto velocemente si raggiunge la velocità massima")]
    [SerializeField] private float acceleration = 70f;
    [Tooltip("Quanto velocemente ci si ferma dopo aver mollato il tasto")]
    [SerializeField] private float deceleration = 50f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float maxJumpHoldTime = 0.18f;

    [Header("Salto - Sensazione")]
    [Tooltip("Gravità extra quando si sale dopo aver rilasciato il salto")]
    [SerializeField] private float lowJumpMultiplier = 3f;
    [Tooltip("Gravità extra durante la caduta")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [Tooltip("Velocità di caduta massima")]
    [SerializeField] private float maxFallSpeed = 22f;

    [Header("Salto - Tolleranze")]
    [Tooltip("Tempo dopo aver lasciato il bordo in cui si può ancora saltare")]
    [SerializeField] private float coyoteTime = 0.1f;
    [Tooltip("Tempo prima di toccare terra in cui il salto resta memorizzato")]
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    /// <summary>
    /// Layer del terreno usato dal ground check. Aggiornato runtime dal SideManager
    /// quando cambia il lato attivo: il player "sta" solo sui collider del lato visibile.
    /// </summary>
    public LayerMask GroundLayer
    {
        get => groundLayer;
        set => groundLayer = value;
    }

    /// <summary>
    /// Quando true, l'input di movimento e salto viene ignorato.
    /// Usato dal FlipController durante l'animazione di transizione (Step 4).
    /// </summary>
    public bool MovementLocked { get; set; }

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private float jumpHoldTimer;
    private bool isJumping;
    private bool jumpHeld;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Durante il flip ignoriamo input. La fisica (gravità, velocità) la gestisce FlipController.
        if (MovementLocked)
        {
            horizontalInput = 0f;
            jumpHeld = false;
            jumpBufferTimer = 0f;
            isJumping = false;
            return;
        }

        horizontalInput = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
            horizontalInput -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
            horizontalInput += 1f;

        jumpHeld = keyboard.spaceKey.isPressed;

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position, groundCheckRadius, groundLayer);

        // Coyote time: finestra di salto dopo aver lasciato una piattaforma
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Jump buffer: memorizza il salto premuto poco prima di toccare terra
        if (keyboard.spaceKey.wasPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // Avvio del salto
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            jumpHoldTimer = maxJumpHoldTime;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Salto variabile: smette di "spingere" se si rilascia o scade il tempo
        if (isJumping)
        {
            if (jumpHeld && jumpHoldTimer > 0f)
                jumpHoldTimer -= Time.deltaTime;
            else
                isJumping = false;
        }

        if (horizontalInput != 0f)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(horizontalInput), 1f, 1f);
        }
    }

    void FixedUpdate()
    {
        ApplyHorizontalMovement();
        ApplyJumpGravity();
    }

    void ApplyHorizontalMovement()
    {
        float targetSpeed = horizontalInput * moveSpeed;

        // Accelera se c'è input, decelera se il tasto è stato mollato
        float rate = (horizontalInput != 0f) ? acceleration : deceleration;

        float newX = Mathf.MoveTowards(
            rb.linearVelocity.x, targetSpeed, rate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
    }

    void ApplyJumpGravity()
    {
        float extraGravity = 0f;

        if (rb.linearVelocity.y < 0f)
        {
            // In caduta: cade più veloce per un controllo più reattivo
            extraGravity = fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0f && !isJumping)
        {
            // In salita ma il tasto è rilasciato: taglia il salto
            extraGravity = lowJumpMultiplier;
        }

        if (extraGravity > 0f)
        {
            rb.linearVelocity += Vector2.up
                * (Physics2D.gravity.y * (extraGravity - 1f) * Time.fixedDeltaTime);
        }

        // Limita la velocità di caduta
        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
