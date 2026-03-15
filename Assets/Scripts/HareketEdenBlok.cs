using System.Collections;
using UnityEngine;

/// <summary>
/// Oyuncu temas ettiğinde belirli bir yönde hareket eden, hedefine ulaştıktan sonra kendini
/// deaktif eden ve dışarıdan bir komutla (ResetPlatform) başlangıç durumuna dönebilen bir platform.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class HareketEdenBlok : MonoBehaviour
{
    [Header("Component References")]
    [Tooltip("Platformun Rigidbody2D bileşeni. Otomatik bulunur ama elle de atanabilir.")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Settings")]
    [Tooltip("Platformun hareket edeceği yön. Örn: Sola (-1, 0), Sağa (1, 0), Yukarı (0, 1)")]
    [SerializeField] private Vector2 moveDirection = Vector2.left;
    [Tooltip("Platformun saniyedeki hareket hızı (birim/saniye).")]
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("Platformun başlangıç noktasından ne kadar uzağa gideceği.")]
    [SerializeField] private float moveDistance = 10f;

    [Header("Timing Settings")]
    [Tooltip("Oyuncu platforma dokunduktan sonra hareketin başlaması için geçecek süre.")]
    [SerializeField] private float moveDelay = 0.5f;
    [Tooltip("Platform hedefe ulaştıktan sonra deaktif olmadan önce beklenecek süre.")]
    [SerializeField] private float destroyDelay = 2f;

    // --- Private Durum Değişkenleri ---
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool hasTriggered = false;
    private Transform playerOnPlatform = null;
    private Coroutine moveCoroutine = null;

    private void Awake()
    {
        // Rigidbody referansını al
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Rigidbody'i Kinematic olarak ayarla ki sadece script ile hareket etsin
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Başlangıç ve hedef pozisyonlarını kaydet
        initialPosition = transform.position;
        targetPosition = initialPosition + (Vector3)(moveDirection.normalized * moveDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sadece "Player" etiketli bir nesne çarptıysa ve daha önce tetiklenmediyse devam et
        if (!hasTriggered && collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = collision.transform;
            hasTriggered = true;
            moveCoroutine = StartCoroutine(MoveAndDeactivate());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Oyuncu platformdan ayrıldıysa, parent ilişkisini kopar
        if (collision.gameObject.CompareTag("Player") && playerOnPlatform != null)
        {
            playerOnPlatform.SetParent(null);
            playerOnPlatform = null;
        }
    }

    private IEnumerator MoveAndDeactivate()
    {
        // Harekete başlamadan önce bekle
        yield return new WaitForSeconds(moveDelay);

        // Oyuncu hala platformdaysa, onu platformun çocuğu yap ki birlikte hareket etsinler
        if (playerOnPlatform != null)
        {
            playerOnPlatform.SetParent(transform);
        }

        // Platformu hedefe doğru hareket ettir
        float journeyStartTime = Time.time;
        float expectedDuration = (moveSpeed > 0) ? moveDistance / moveSpeed : float.MaxValue;

        rb.linearVelocity = moveDirection.normalized * moveSpeed; // GÜNCELLENDİ

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Eğer platform bir yere takılırsa sonsuz döngüden çık (zaman aşımı)
            if (Time.time - journeyStartTime > expectedDuration * 1.1f)
            {
                Debug.LogWarning("Platform hareketi zaman aşımına uğradı! Muhtemelen bir engele takıldı.", this.gameObject);
                break;
            }
            yield return new WaitForFixedUpdate();
        }

        // Hedefe ulaşıldığında hareketi durdur ve pozisyonu tam hedefe sabitle
        rb.linearVelocity = Vector2.zero; // GÜNCELLENDİ
        transform.position = targetPosition;

        // Deaktif olmadan önce bekle
        yield return new WaitForSeconds(destroyDelay);

        // Oyuncu hala platformdaysa, parent ilişkisini kopar
        if (playerOnPlatform != null)
        {
            playerOnPlatform.SetParent(null);
            playerOnPlatform = null;
        }

        // Platformu deaktif et
        gameObject.SetActive(false);
        moveCoroutine = null;
    }

    /// <summary>
    /// Platformu anında durdurur ve başlangıç durumuna geri döndürür.
    /// Bu fonksiyon PlayerMovement script'i tarafından çağrılır.
    /// </summary>
    public void ResetPlatform()
    {
        // Eğer bir hareket Coroutine'i çalışıyorsa, onu anında durdur
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        // Oyuncu hala platformdaysa, parent ilişkisini kopar
        if (playerOnPlatform != null)
        {
            playerOnPlatform.SetParent(null);
            playerOnPlatform = null;
        }

        // Platformu başlangıç ayarlarına döndür
        gameObject.SetActive(true);
        transform.position = initialPosition;
        hasTriggered = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero; // GÜNCELLENDİ
    }
}