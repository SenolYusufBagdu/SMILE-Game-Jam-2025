using System.Collections;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    [Tooltip("Testerenin ileri doðru hareket hýzý.")]
    [SerializeField] private float moveSpeed = 10f;

    [Tooltip("Testerenin kaybolmadan önce ne kadar mesafe gideceði.")]
    [SerializeField] private float moveDistance = 8f;

    // YENÝ EKLENDÝ -> Devriye Ayarlarý
    [Header("Devriye Ayarlarý")]
    [Tooltip("Hedefe ulaþtýktan sonra kaç saniye devriye atacaðý.")]
    [SerializeField] private float patrolDuration = 4f;

    [Tooltip("Devriye atarken ne kadar mesafe geri gideceði.")]
    [SerializeField] private float patrolDistance = 1.5f;

    [Tooltip("Devriye atarkenki ileri-geri hýzý.")]
    [SerializeField] private float patrolSpeed = 5f;

    [Header("Sýfýrlanma Ayarlarý")]
    [Tooltip("Testere kaybolduktan ne kadar sonra eski yerine dönsün? (0 ise geri dönmez)")]
    [SerializeField] private float resetDelay = 5f;

    private Vector2 initialPosition;
    private bool isSawActive = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSawActive)
        {
            StartCoroutine(ActivateSaw());
        }
    }

    private IEnumerator ActivateSaw()
    {
        isSawActive = true;

        // --- 1. HEDEFE GÝTME AÞAMASI ---
        Vector2 mainTargetPosition = initialPosition + (Vector2.right * moveDistance);
        while (Vector2.Distance(transform.position, mainTargetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, mainTargetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // --- 2. YENÝ EKLENEN DEVRIYE AÞAMASI ---
        if (patrolDuration > 0)
        {
            float patrolTimer = 0f;
            // Devriye atacaðý iki noktayý belirle
            Vector2 patrolPointA = mainTargetPosition; // Saðdaki nokta
            Vector2 patrolPointB = mainTargetPosition - new Vector2(patrolDistance, 0); // Soldaki nokta

            while (patrolTimer < patrolDuration)
            {
                // Mathf.PingPong ile iki nokta arasýnda yumuþak bir þekilde git gel yap
                float pingPongTime = Mathf.PingPong(Time.time * patrolSpeed, 1);
                transform.position = Vector2.Lerp(patrolPointB, patrolPointA, pingPongTime);

                patrolTimer += Time.deltaTime;
                yield return null; // Her frame bekle
            }
        }

        // --- 3. KAYBOLMA VE SIFIRLANMA AÞAMASI ---
        gameObject.SetActive(false);

        if (resetDelay > 0)
        {
            yield return new WaitForSeconds(resetDelay);
            transform.position = initialPosition;
            gameObject.SetActive(true);
            isSawActive = false;
        }
    }
}