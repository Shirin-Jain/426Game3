using UnityEngine;
using UnityEngine.InputSystem;

public class Whirlpool : MonoBehaviour
{
    public float pullStrength = 20f;
    public float maxPullDistance = 5f;

    public float escapeForce = 25f;
    public float escapeSpeedThreshold = 6f;
    public float centerLockRadius = 0.3f;

    public string playerTag = "Boat";

    public float trappedRotationSpeed = 360f; // degrees per second


    public float mashGainPerPress = 0.2f;
    public float mashDecayPerSecond = 0.4f;
    public float mashRequired = 1f;
    // public KeyCode mashKey = KeyCode.Space;

    public float maxTrappedTime = 3f;
    private float trappedTimer = 0f;

    public BoatCollision playerDie;


    public float mashProgress = 0f;

    public bool escape = false;

    bool spacePressed = false;

    public Vector2 startDirection = Vector2.zero;

    private void Start()
    {
        ResetTrapState();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
            return;

        startDirection = collision.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag))
            return;

        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb == null)
            return;

        Vector2 directionToCenter = (Vector2)transform.position - rb.position;
        float distance = directionToCenter.magnitude;

        float playerSpeed = rb.linearVelocity.magnitude;

        // If player is fast enough, they can fight the pull
        if (playerSpeed >= escapeSpeedThreshold)
        {
            ApplyPull(rb, directionToCenter, distance, 0.5f);
            mashProgress = 0f;

            return;
        }

        // If player is slow and near center → trapped
        if (distance <= centerLockRadius)
        {
            escape = false;

           TrapPlayer(rb);
           HandleButtonMash(rb);
           HandleDeathTimer(collision);



            return;
        }

        // Normal pull
        ApplyPull(rb, directionToCenter, distance, 1f);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        ResetTrapState();
    }

    private void TrapPlayer(Rigidbody2D rb)
    {
        rb.linearVelocity = Vector2.zero;
        rb.position = transform.position;
        rb.MoveRotation(rb.rotation + trappedRotationSpeed * Time.fixedDeltaTime);
    }

    private void HandleButtonMash(Rigidbody2D rb)
    {
        // Gain progress on button press
        if (Keyboard.current.spaceKey.IsPressed() && !spacePressed)
        {
            mashProgress += mashGainPerPress;
        }
        spacePressed = Keyboard.current.spaceKey.IsPressed();

        // Decay over time
        mashProgress -= mashDecayPerSecond * Time.fixedDeltaTime;
        mashProgress = Mathf.Clamp01(mashProgress);

        // Escape!
        if (mashProgress >= mashRequired)
        {
            escape = true;
            //Vector2 escapeDirection = (rb.position - (Vector2)transform.position).normalized; // get old direction
            Vector2 escapeDirection = new Vector2(1, 0);
            rb.AddForce(escapeDirection * escapeForce, ForceMode2D.Impulse);

            mashProgress = 0f;
        }
    }

    private void HandleDeathTimer(Collider2D player)
    {
        trappedTimer += Time.fixedDeltaTime;

        if (trappedTimer >= maxTrappedTime)
        {
            KillPlayer(player);
        }
    }

    private void ApplyPull(Rigidbody2D rb, Vector2 direction, float distance, float strengthMultiplier)
    {
        float distanceFactor = Mathf.Clamp01(1f - (distance / maxPullDistance));
        Vector2 force = direction.normalized * pullStrength * distanceFactor * strengthMultiplier;

        rb.AddForce(force, ForceMode2D.Force);
    }

    private void KillPlayer(Collider2D player)
    {
        // cal player die
        Debug.Log("Die");
    }


    private void ResetTrapState()
    {
        mashProgress = 0f;
        trappedTimer = 0f;
        startDirection = Vector2.zero;
    }
}
