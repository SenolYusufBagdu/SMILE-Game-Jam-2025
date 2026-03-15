using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DusenZemn : MonoBehaviour
{
    [Header("Düþme Ayarlarý")]
    [Tooltip("Platformun saniyede ne kadar hýzla aþaðýya ineceðini belirtir.")]
    [SerializeField] private float fallSpeed = 4f;

    [Tooltip("Oyuncu dokunduktan kaç saniye sonra platformun hareket etmeye baþlayacaðýný belirtir.")]
    [SerializeField] private float activationDelay = 0.5f;

    [Tooltip("Platform düþmeye baþladýktan kaç saniye sonra kaybolacaðýný belirtir.")]
    [SerializeField] private float destroyDelay = 3f;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isFalling = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // DÜZELTME: Rigidbody2D.Kinematic yerine RigidbodyType2D.Kinematic kullanýlmalý.
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            Vector2 moveDirection = Vector2.down * fallSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFalling && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ActivateFall());
        }
    }

    private IEnumerator ActivateFall()
    {
        yield return new WaitForSeconds(activationDelay);
        isFalling = true;

        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }

    public void ResetPlatform()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        isFalling = false;

        // DÜZELTME: Rigidbody2D.Kinematic yerine RigidbodyType2D.Kinematic kullanýlmalý.
        rb.bodyType = RigidbodyType2D.Kinematic;

        // DÜZELTME: velocity yerine güncel olan linearVelocity kullanýlmalý.
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
}