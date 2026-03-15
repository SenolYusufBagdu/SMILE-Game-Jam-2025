using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))] // SpriteRenderer bile�enini zorunlu k�lar
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 15f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Advanced Jump Settings")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("Dependencies & Features")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private  GameObject fallDetector;

    // --- Component Referanslar� ---
    private Rigidbody2D rb;
    private OyuncuCan oyuncuCanSistemi;
    private Animator anim;
    private SpriteRenderer spriteRenderer; // YEN� EKLEND�: Karakterin g�rselini �evirmek i�in

    // --- Private Durum De�i�kenleri ---
    private Vector3 respawnPoint;
    private float horizontalInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isFacingRight = true; // YEN� EKLEND�: Karakterin sa�a bakt���n� varsayarak ba�lar

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        oyuncuCanSistemi = GetComponent<OyuncuCan>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // YEN� EKLEND�: SpriteRenderer referans�n� al
    }

    void Start()
    {
        respawnPoint = transform.position;

        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObj.transform;
        }
    }

    void Update()
    {
        HandleInput();
        CheckGroundStatus();
        HandleCoyoteAndBufferTime();
        HandleJump();
        UpdateAnimationParameters();

        // YEN� EKLEND�: Karakterin y�n�n� her frame kontrol et
        HandleFlip();

        if (fallDetector != null)
        {
            fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleBetterJumpPhysics();
    }

    private void UpdateAnimationParameters()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetBool("isJumping", !isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FallDetector"))
        {
            if (oyuncuCanSistemi != null)
            {
                // DE����KL�K: Art�k d���nce t�m can gitmiyor, sadece 1 can gidiyor.
                // �sterseniz buradaki '1' de�erini art�rabilirsiniz.
                oyuncuCanSistemi.CanAzalt(1);
            }

            transform.position = respawnPoint;
            rb.linearVelocity = Vector2.zero;

            HareketEdenBlok[] hareketEdenBloks = FindObjectsByType<HareketEdenBlok>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (HareketEdenBlok blok in hareketEdenBloks)
            {
                blok.ResetPlatform();
            }

            DusenZemn[] dusenZeminler = FindObjectsByType<DusenZemn>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (DusenZemn zemin in dusenZeminler)
            {
                zemin.ResetPlatform();
            }
        }
        else if (collision.CompareTag("TeleportEntrance"))
        {
            if (teleportDestination != null)
            {
                transform.position = teleportDestination.position;
            }
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    // YEN� EKLENEN FONKS�YON: Karakterin sprite'�n� �evirir
    private void HandleFlip()
    {
        // Sola gidiyorsa ve y�z� sa�a d�n�kse, �evir.
        if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
        // Sa�a gidiyorsa ve y�z� sola d�n�kse, �evir.
        else if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
    }

    // YEN� EKLENEN FONKS�YON: �evirme i�lemini yapar
    private void Flip()
    {
        isFacingRight = !isFacingRight; // Y�n de�i�kenini tersine �evir
        spriteRenderer.flipX = !isFacingRight; // Sprite'� X ekseninde �evir
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
    }

    private void HandleCoyoteAndBufferTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleBetterJumpPhysics()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}