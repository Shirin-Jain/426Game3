using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    public float requiredImpactSpeed = 5f;

    public GameObject breakEffect; // particle, animation, etc.

    public string playerTag = "Boat";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player hit it
        if (!collision.gameObject.CompareTag(playerTag))
            return;

        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null)
            return;

        // Get impact speed
        float impactSpeed = collision.relativeVelocity.x;

        // Break only if fast enough
        if (impactSpeed >= requiredImpactSpeed)
        {
            Break();
        }
    }

    private void Break()
    {
        if (breakEffect != null)
        {
            Instantiate(breakEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
