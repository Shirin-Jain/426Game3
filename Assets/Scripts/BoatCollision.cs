using UnityEngine;
using System.Collections;


public class BoatCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private string deathTag = "TnT";

    [SerializeField] private string invulnerableTag = "Invulnerable";
    [SerializeField] private float invulnerableDuration = 3f;
 
    private bool isInvulnerable = false;
    private Collider2D playerCollider;

    [SerializeField] private float flashTimeRemaining = 0.5f;
    [SerializeField] private float flashInterval = 0.05f;

    [Header("Visuals")]
    [SerializeField] private Color invulnerableColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public bool actuallyDie = false;


    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(invulnerableTag))
        {
            StartCoroutine(InvulnerabilityCoroutine());
            Destroy(other.gameObject); 
        }
        else if (other.CompareTag(deathTag))
        {
            TryDie();
        }
    }

    // For hazards using collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //disable collisions
        if (collision.gameObject.CompareTag(deathTag))
        {
            TryDie();
        }
    }

    public void TryDie()
    {
        if (isInvulnerable)
            return;

        Die();
    }

    void Die()
    {

        if (actuallyDie)
        {
            Destroy(gameObject);
            // game over screen

        }
        else
        {
            Debug.Log("Die");
        }
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        playerCollider.enabled = false; // 

        spriteRenderer.color = invulnerableColor;


        float timeLeft = invulnerableDuration;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            // Start flashing near the end
            if (timeLeft <= flashTimeRemaining)
            {
                yield return StartCoroutine(FlashEffect());
                break;
            }

            yield return null;
        }



        spriteRenderer.color = originalColor;
        isInvulnerable = false;
        playerCollider.enabled = true; 
    }


    IEnumerator FlashEffect()
    {
        float elapsed = 0f;

        while (elapsed < flashTimeRemaining)
        {
            spriteRenderer.color = invulnerableColor;
            yield return new WaitForSeconds(flashInterval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashInterval);

            elapsed += flashInterval * 2;
        }
    }
}
