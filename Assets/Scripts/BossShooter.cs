using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bossTransform;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float shotForce = 10f;
    [SerializeField] private float fireRate = 1f; // 1 saniyede bir mermi

    private float nextFireTime = 0f;

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            ShootAtPlayer();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void ShootAtPlayer()
    {
        if (characterTransform == null || bulletPrefab == null) return;

        Vector3 spawnPos = bossTransform.position; // Boss’un pozisyonu
        Vector3 targetPos = characterTransform.position; // Player’ýn pozisyonu

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Destroy(bullet, 0.5f);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = (targetPos - spawnPos).normalized;
            rb.AddForce(direction * shotForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody2D!");
        }
    }
}
