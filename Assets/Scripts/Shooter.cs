using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private Transform characterTransform;

    [Header("Settings")]
    [SerializeField] private float shotForce = 2f;

    [Header("Audio")] // YENÝ BÖLÜM
    [SerializeField] private AudioClip shootSound; // Ateþ etme sesi için bir alan ekledik

    private void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            LeftShootPortal();
        }
    }

    private void LeftShootPortal()
    {
        ShootBullet(BulletPrefab);
    }

    private void ShootBullet(GameObject prefab)
    {
        // --- SESÝ ÇALDIÐIMIZ YER ---
        // AudioManager'a eriþip ses efektini çalmasýný söylüyoruz
        if (shootSound != null && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(shootSound);
        }
        // -------------------------

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Vector3 spawnPos = characterTransform.position;

        GameObject portalProjectile = Instantiate(prefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = portalProjectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (mousePos - spawnPos).normalized;
            rb.AddForce(direction * shotForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Portal prefab does not have a Rigidbody2D!");
        }
    }
}