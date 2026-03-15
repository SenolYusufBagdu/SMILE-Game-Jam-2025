using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Health = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
