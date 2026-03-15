using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    public Vector2 direction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Enemy"))
        {
            // Çarpýlan objede EnemyHealth var mý kontrol asdfa
            OyuncuCan target = other.GetComponent<OyuncuCan>();
            if (target != null)
            {
                target.CanAzalt(1); // Can azalt
                Debug.Log("Çarptý: " + other.gameObject.name + " | Yeni Health: " + target.mevcutCan);
            }

            // Mermi çarptýktan sonra yok olsun (istersen)
            Destroy(gameObject);
        }




    }

    //private void Update()
    //{
    //    transform.position += new Vector3(direction.x * 10f * Time.deltaTime, direction.y * 10f * Time.deltaTime);
    //}
}
