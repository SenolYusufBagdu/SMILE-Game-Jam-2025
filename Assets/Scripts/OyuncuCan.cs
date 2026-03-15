using UnityEngine;
using UnityEngine.UI; // UI elemanlarý için bu kütüphane gerekli
using UnityEngine.SceneManagement; // SAHNE YÖNETÝMÝ ÝÇÝN EKLENDÝ

public class OyuncuCan : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public int maxCan = 3;
    [SerializeField]
    public float mevcutCan;

    [Header("UI Elemanlarý")]
    [Tooltip("Hiyerarþideki 'Bar' objesini buraya sürükleyin")]
    public Image canBari; // Slider yerine Image kullanýyoruz

    void Start()
    {
        mevcutCan = maxCan;
        if (canBari != null)
        {
            canBari.fillAmount = 1;
        }
    }

    public void CanAzalt(int hasarMiktari)
    {
        mevcutCan -= hasarMiktari;
        if (mevcutCan < 0)
        {
            mevcutCan = 0;
        }
        Debug.Log("Oyuncu caný: " + mevcutCan);
        GuncelleCanBari();

        if (mevcutCan <= 0)
        {
            Olum();
        }
    }

    void GuncelleCanBari()
    {
        if (canBari != null)
        {
            canBari.fillAmount = (float)mevcutCan / maxCan;
        }
    }

    // GÜNCELLENEN KISIM: Oyuncu ölünce sahneyi yeniden baþlat
    private void Olum()
    {
        Debug.Log("Oyuncu öldü! Sahne yeniden baþlatýlýyor.");

        // Aktif olan sahnenin 'build index'ini al ve o sahneyi yeniden yükle.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eðer temas ettiðimiz nesnenin etiketi "Tehlike" ise
        if (collision.gameObject.CompareTag("Tehlike"))
        {
            // Kendi canýmýzý 1 kadar azalt.
            CanAzalt(1);
        }
    }
}