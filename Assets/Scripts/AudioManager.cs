using UnityEngine;
using UnityEngine.Audio; // Audio Mixer için bu kütüphane gerekli

public class AudioManager : MonoBehaviour
{
    // Bu script'e her yerden kolayca eriþmek için "singleton" deseni
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource; // Arka plan müziði için
    [SerializeField] private AudioSource sfxSource;   // Ses efektleri için

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    private void Awake()
    {
        // Singleton deseni: Sahnede sadece bir tane AudioManager olmasýný saðlar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþse bile bu nesneyi yok etme
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Oyunu arka plan müziði ile baþlat
        musicSource.clip = backgroundMusic;
        musicSource.loop = true; // Müziði döngüye al
        musicSource.Play();
    }

    // Dýþarýdan çaðrýlacak ses efekti oynatma fonksiyonu
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Slider'dan gelen deðere göre sesi ayarlayan fonksiyon
    public void SetMasterVolume(float volume)
    {
        // volume slider'dan 0-1 arasýnda bir deðer alýr.
        // Mixer logaritmik çalýþtýðý için lineer deðeri logaritmik deðere çeviriyoruz.
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}