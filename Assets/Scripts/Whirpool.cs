using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    public float pullStrength = 20f;
    public float maxPullDistance = 5f;

    public float escapeSpeedThreshold = 6f;
    public float centerLockRadius = 0.3f;

    public string playerTag = "Boat";

    public float trappedRotationSpeed = 360f; // degrees per second


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb == null)
            return;

        Vector2 directionToCenter = (Vector2)transform.position - rb.position;
        float distance = directionToCenter.magnitude;

        float playerSpeed = rb.linearVelocity.magnitude;

        // If player is fast enough, they can fight the pull
        if (playerSpeed >= escapeSpeedThreshold)
        {
            ApplyPull(rb, directionToCenter, distance, 0.5f);
            return;
        }

        // If player is slow and near center → trapped
        if (distance <= centerLockRadius)
        {
            rb.linearVelocity = Vector2.zero;
            rb.position = transform.position;
            rb.MoveRotation(rb.rotation + trappedRotationSpeed * Time.fixedDeltaTime);

            return;
        }

        // Normal pull
        ApplyPull(rb, directionToCenter, distance, 1f);
    }

    private void ApplyPull(Rigidbody2D rb, Vector2 direction, float distance, float strengthMultiplier)
    {
        float distanceFactor = Mathf.Clamp01(1f - (distance / maxPullDistance));
        Vector2 force = direction.normalized * pullStrength * distanceFactor * strengthMultiplier;

        rb.AddForce(force, ForceMode2D.Force);
    }
}
