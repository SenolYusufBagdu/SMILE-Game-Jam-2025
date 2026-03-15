using System.Collections;
using UnityEngine;

public class ControlledFallPlatform : MonoBehaviour
{
    [Header("Zamanlama Ayarlar�")]
    [Tooltip("Platformun tepede ne kadar s�re bekleyece�i.")]
    [SerializeField] private float stayTime = 2f;

    [Tooltip("Platformun a�a�� do�ru ne kadar s�re boyunca hareket edece�i.")]
    [SerializeField] private float fallDuration = 6f;

    [Tooltip("Platformun ba�lang�� konumuna geri d�nmesinin ne kadar s�rece�i.")]
    [SerializeField] private float returnDuration = 1.5f;

    [Header("Hareket Ayarlar�")]
    [Tooltip("Platformun a�a�� d��erken h�z�. Yer�ekimini ge�ersiz k�lar.")]
    [SerializeField] private float fallSpeed = 5f;

    private Rigidbody2D rb2D;
    private Vector2 initialPosition;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        StartCoroutine(PlatformCycle());
    }

    private IEnumerator PlatformCycle()
    {
        while (true)
        {
            // --- 1. BEKLEME A�AMASI ---
            transform.position = initialPosition;
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(stayTime);

            // --- 2. KONTROLL� D��ME A�AMASI ---
            rb2D.bodyType = RigidbodyType2D.Dynamic; // �arp��malar i�in Dynamic yap
            float fallTimer = 0f;
            while (fallTimer < fallDuration)
            {
                // Yer�ekimini ezip h�z� kendimiz belirliyoruz
                rb2D.linearVelocity = new Vector2(0, -fallSpeed);
                fallTimer += Time.deltaTime;
                yield return null; // Bir sonraki frame'i bekle
            }

            // --- 3. YUMU�AK GER� D�N�� A�AMASI ---
            rb2D.bodyType = RigidbodyType2D.Kinematic; // Fizi�i tekrar kapat
            rb2D.linearVelocity = Vector2.zero;

            Vector2 returnStartPosition = transform.position;
            float returnTimer = 0f;
            while (returnTimer < returnDuration)
            {
                // Lerp (Do�rusal Enterpolasyon) ile pozisyonu yumu�ak�a de�i�tir
                float percentageComplete = returnTimer / returnDuration;
                transform.position = Vector2.Lerp(returnStartPosition, initialPosition, percentageComplete);

                returnTimer += Time.deltaTime;
                yield return null;
            }
        }
    }
}