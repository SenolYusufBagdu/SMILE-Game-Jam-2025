using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Portal Ayarlarý")]
    [Tooltip("Bu portalýn ýþýnlayacaðý diðer portal objesini buraya sürükleyin.")]
    [SerializeField] private Transform destinationPortal;

    // Bu deðiþken, art arda ýþýnlanmayý önlemek için kullanýlýr.
    private bool isTeleporting = false;

    // Oyuncu portalýn tetikleme alanýna girdiðinde bu fonksiyon çalýþýr
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer alana giren nesne oyuncu ise ve þu an bir ýþýnlanma iþlemi yoksa
        if (other.CompareTag("Player") && !isTeleporting)
        {
            // Hedef portalýn da oyuncuyu hemen geri ýþýnlamasýný engelle
            Portal destinationPortalScript = destinationPortal.GetComponent<Portal>();
            if (destinationPortalScript != null)
            {
                destinationPortalScript.isTeleporting = true; // Hedef portalýn ýþýnlama yapmasýný geçici olarak engelle
            }

            // Oyuncunun pozisyonunu, hedef portalýn pozisyonu yap
            other.transform.position = destinationPortal.position;
        }
    }

    // Oyuncu portalýn tetikleme alanýndan tamamen çýktýðýnda bu fonksiyon çalýþýr
    private void OnTriggerExit2D(Collider2D other)
    {
        // Eðer alandan çýkan oyuncu ise, tekrar ýþýnlanmaya izin ver
        if (other.CompareTag("Player"))
        {
            isTeleporting = false;
        }
    }
}