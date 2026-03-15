using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [Tooltip("Dikenin yukarý çýkacaðý hedef pozisyon.")]
    [SerializeField] private Transform targetPoint;

    [Tooltip("Dikenin yukarý-aþaðý hareket hýzý.")]
    [SerializeField] private float moveSpeed = 8f;

    [Tooltip("Diken tepeye ulaþtýktan sonra ne kadar bekleyeceði.")]
    [SerializeField] private float delayAtTop = 0.5f;

    private Vector2 initialPosition;
    private bool isTrapActive = false;

    void Start()
    {
        // Tuzaðýn baþlangýç pozisyonunu kaydet
        initialPosition = transform.position;
    }

    // Oyuncu algýlama alanýna girdiðinde bu fonksiyon tetiklenir
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer alana giren oyuncu ise ve tuzak zaten aktif deðilse
        if (other.CompareTag("Player") && !isTrapActive)
        {
            // Tuzaðý etkinleþtirme döngüsünü baþlat
            StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap()
    {
        // Tuzaðý aktif olarak iþaretle ki tekrar tekrar tetiklenmesin
        isTrapActive = true;

        // --- YUKARI HAREKET ---
        // Hedef pozisyona ulaþana kadar dikeni yukarý hareket ettir
        while (Vector2.Distance(transform.position, targetPoint.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            yield return null; // Bir sonraki frame'e kadar bekle
        }

        // --- TEPEDE BEKLEME ---
        // Belirtilen süre kadar tepede bekle
        yield return new WaitForSeconds(delayAtTop);

        // --- AÞAÐI HAREKET ---
        // Baþlangýç pozisyonuna ulaþana kadar dikeni aþaðý hareket ettir
        while (Vector2.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            yield return null; // Bir sonraki frame'e kadar bekle
        }

        // Tam baþlangýç pozisyonuna geldiðinden emin ol
        transform.position = initialPosition;

        // Tuzaðý tekrar tetiklenebilir hale getir
        isTrapActive = false;
    }
}