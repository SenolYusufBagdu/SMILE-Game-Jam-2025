using UnityEngine;
using UnityEngine.SceneManagement; // Sahne deðiþtirmek için bu kütüphane gerekli

public class MainMenuManager : MonoBehaviour
{
    // Ayarlar panelini Inspector'dan atamak için
    [SerializeField] private GameObject settingsPanel;

    // Oyunu baþlatacak fonksiyon
    public void StartGame()
    {
        // "GameScene" yazan yere kendi oyun sahnenin adýný yazmalýsýn!
        SceneManager.LoadScene("levelTEST");
    }

    // Ayarlar panelini açacak fonksiyon
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Ayarlar panelini kapatacak fonksiyon
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    // Oyundan çýkýþ yapacak fonksiyon
    public void QuitGame()
    {
        Debug.Log("Oyundan Çýkýldý!"); // Unity Editor'de test etmek için log basar
        Application.Quit(); // Sadece build alýnmýþ oyunda çalýþýr
    }
}