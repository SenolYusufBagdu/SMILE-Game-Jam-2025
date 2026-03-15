using UnityEngine;
using UnityEngine.SceneManagement; // Bu satýr sahne yönetimi için olmazsa olmaz!

public class SceneLoader : MonoBehaviour
{
    [Header("Sahne Ayarlarý")]
    [Tooltip("Yüklenmesini istediðiniz sahnenin adýný tam olarak yazýn.")]
    [SerializeField] private string sceneNameToLoad;

    // Bir nesne trigger alanýna girdiðinde bu fonksiyon çalýþýr
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Alana giren nesnenin etiketi "Player" ise
        if (other.CompareTag("Player"))
        {
            // Belirtilen sahneyi yükle
            Debug.Log(sceneNameToLoad + " sahnesi yükleniyor...");
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}