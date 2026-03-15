using UnityEngine;

public class DisappearOnContact : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarptýðýmýz nesnenin katmaný "Zemin" katmaný ise
        if (collision.gameObject.layer == LayerMask.NameToLayer("Zemin"))
        {
            // Bu objeyi deaktif et (kaybet)
            gameObject.SetActive(false);
        }
    }
}