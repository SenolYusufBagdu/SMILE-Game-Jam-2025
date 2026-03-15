using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLetterTrap : MonoBehaviour
{
    [Header("Tuzak Ayarlar�")]
    [Tooltip("D��ecek olan harf objelerini buraya s�r�kleyin.")]
    [SerializeField] private GameObject[] letters;

    [Tooltip("Harf d��t�kten ka� saniye sonra tuzak tekrar kurulur?")]
    [SerializeField] private float resetDelay = 4f;

    private List<Vector3> initialPositions;
    private List<Quaternion> initialRotations;
    private bool isTrapActive = false;

    void Start()
    {
        initialPositions = new List<Vector3>();
        initialRotations = new List<Quaternion>();
        foreach (GameObject letter in letters)
        {
            initialPositions.Add(letter.transform.position);
            initialRotations.Add(letter.transform.rotation);
            letter.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTrapActive)
        {
            StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap()
    {
        isTrapActive = true;

        // --- �K� FARKLI RASTGELE HARF SE�ME ---
        int index1 = Random.Range(0, letters.Length);
        int index2;
        do
        {
            index2 = Random.Range(0, letters.Length);
        } while (index1 == index2);

        GameObject[] lettersToDrop = new GameObject[] { letters[index1], letters[index2] };

        // --- SE��LEN �K� HARF� D���RME ---
        foreach (GameObject letter in lettersToDrop)
        {
            Rigidbody2D rb = letter.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        // S�f�rlama i�in bekle
        yield return new WaitForSeconds(resetDelay);

        // --- D��EN �K� HARF� SIFIRLAMA ---
        foreach (GameObject letter in lettersToDrop)
        {
            Rigidbody2D rb = letter.GetComponent<Rigidbody2D>();
            int originalIndex = System.Array.IndexOf(letters, letter);

            if (rb != null)
            {
                letter.SetActive(true);
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                letter.transform.position = initialPositions[originalIndex];
                letter.transform.rotation = initialRotations[originalIndex];
            }
        }

        isTrapActive = false;
    }
}