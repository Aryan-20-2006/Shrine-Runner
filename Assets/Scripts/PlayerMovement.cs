using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 6.8f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float deceleration = 55f;
    [SerializeField] private float airControlMultiplier = 0.7f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 11f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Landing Effect")]
    [SerializeField] private float landingSquash = 0.8f;
    [SerializeField] private float squashRecoverSpeed = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.35f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Player Fit")]
    [SerializeField] private float visualScaleMultiplier = 0.9f;
    [SerializeField] private bool autoAdjustFeetClearance = true;
    [SerializeField] private float feetClearance = 0.04f;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool wasGrounded;
    private Vector3 baseScale;
    private float moveInput;
    private bool jumpHeld;
    private float lastGroundedTime = -999f;
    private float jumpPressedTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null && rb.interpolation == RigidbodyInterpolation2D.None)
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Keep the character slightly smaller and preserve that as the baseline scale.
        baseScale = transform.localScale * visualScaleMultiplier;
        transform.localScale = baseScale;

        if (autoAdjustFeetClearance)
            AdjustFeetClearance();
    }

    void Update()
    {
        ReadInput();
        RecoverScale();
        UpdateVisuals();
    }

    void FixedUpdate()
    {
        UpdateGroundedState();
        HandleMovement();
        HandleJumpAndGravity();
        LandingEffect();
    }

    void ReadInput()
    {
        moveInput = 0f;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput = -1f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput = 1f;

        jumpHeld = Keyboard.current.spaceKey.isPressed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressedTime = Time.time;
    }

    void UpdateGroundedState()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
            lastGroundedTime = Time.time;
    }

    void HandleMovement()
    {
        float targetSpeed = moveInput * speed;
        float control = isGrounded ? 1f : airControlMultiplier;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float nextX = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            accelRate * control * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(nextX, rb.linearVelocity.y);
    }

    void HandleJumpAndGravity()
    {
        if ((Time.time - jumpPressedTime) <= jumpBufferTime && (Time.time - lastGroundedTime) <= coyoteTime)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressedTime = -999f;
            lastGroundedTime = -999f;
            isGrounded = false;
        }

        if (rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    void LandingEffect()
    {
        if (isGrounded && !wasGrounded)
        {
            transform.localScale = new Vector3(
                baseScale.x * 1.2f,
                baseScale.y * landingSquash,
                baseScale.z
            );
        }

        wasGrounded = isGrounded;
    }

    void RecoverScale()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            baseScale,
            squashRecoverSpeed * Time.deltaTime
        );
    }

    void UpdateVisuals()
    {
        if (animator != null)
            animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.05f);

        if (spriteRenderer == null)
            return;

        if (moveInput > 0f)
            spriteRenderer.flipX = false;
        else if (moveInput < 0f)
            spriteRenderer.flipX = true;
    }

    void AdjustFeetClearance()
    {
        if (feetClearance <= 0f)
            return;

        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        if (capsule != null)
        {
            capsule.size = new Vector2(capsule.size.x, Mathf.Max(0.1f, capsule.size.y - feetClearance));
            capsule.offset += new Vector2(0f, feetClearance * 0.5f);
            return;
        }

        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            box.size = new Vector2(box.size.x, Mathf.Max(0.1f, box.size.y - feetClearance));
            box.offset += new Vector2(0f, feetClearance * 0.5f);
        }
    }
}