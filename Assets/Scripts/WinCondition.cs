using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private bool hasWon = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasWon) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        hasWon = true;

        // Stop boat physics
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Show win UI
        if (winPanel != null)
            winPanel.SetActive(true);
    }
}
