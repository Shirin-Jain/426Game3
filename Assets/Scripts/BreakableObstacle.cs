using UnityEngine;
using UnityEngine.Rendering;

public class BreakableObstacle : MonoBehaviour
{
    public float requiredImpactSpeed = 5f;

    public GameObject breakEffect; // particle, animation, etc.

    public float minImpactSpeed = 1f;
    public float maxImpactSpeed = 20f;

    public float minVolume = 0.1f;
    public float maxVolume = 1f;

    public string playerTag = "Boat";

    public float bounceForce = 8f;


    public BoatAudio audioManager;
    public BoatMove boat;

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

     ;

        // Break only if fast enough
        if (impactSpeed >= requiredImpactSpeed)
        {
            audioManager.PlayCrateBreakSound();
            Break();
        }
        else
        {
            float normalized = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, impactSpeed);

            // Convert to volume range
            float volume = Mathf.Lerp(minVolume, maxVolume, normalized);
            audioManager.PlayCrashSound(volume);

            Debug.Log("normal:" + normalized);
            //Vector2 bounceDirection = collision.contacts[0].normal;
            //bounceDirection.x = - bounceDirection.x;
            //bounceDirection.y = - bounceDirection.y;

            //boat.Bounce(normalized, collision.relativeVelocity);
            //playerRb.linearVelocity = Vector2.zero;
            //playerRb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
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
