using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            // Çarpýlan objede EnemyHealth var mý kontrol asdfa
            EnemyHealth target = other.GetComponent<EnemyHealth>();
            if (target != null)
            {
                target.Health -= 1f; // Can azalt
                Debug.Log("Çarptý: " + other.gameObject.name + " | Yeni Health: " + target.Health);
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
