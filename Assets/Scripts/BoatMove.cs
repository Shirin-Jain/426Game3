using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMove : MonoBehaviour
{
    public float maxHorizontalSpeed;
    public float horizontalAccel;
    public float horizontalDeccel;

    public float verticalDeccel;

    public float currentAccel = 15;

    public string currentTag = "Current";
    public bool onCurrent = false;

    private Rigidbody2D rb;

    [Header("Movement Audio")]
    [SerializeField] private AudioSource moveAudio;     // looping movement sound
    [SerializeField] private float minSpeedForSound = 0.1f;
    [SerializeField] private float movePitchMin = 0.95f;
    [SerializeField] private float movePitchMax = 1.05f;

    [Header("Explosion Audio")]
    [SerializeField] private AudioSource explosionAudio; // one-shot explosion sound
    [SerializeField] private string tntTag = "TNT";
    [SerializeField] private float explosionPitchMin = 0.95f;
    [SerializeField] private float explosionPitchMax = 1.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // If you forgot to assign, try grabbing an AudioSource from the same GameObject for moveAudio.
        if (moveAudio == null)
            moveAudio = GetComponent<AudioSource>();

        if (moveAudio != null)
        {
            moveAudio.loop = true;
            moveAudio.playOnAwake = false;
        }

        if (explosionAudio != null)
        {
            explosionAudio.loop = false;
            explosionAudio.playOnAwake = false;
        }
    }

    void Update()
    {
        float horizotnalVelocity = rb.linearVelocity.x;
        float verticalVelocity = rb.linearVelocity.y;

        if (Keyboard.current.spaceKey.isPressed)
        {
            horizotnalVelocity += horizontalAccel * Time.deltaTime;
        }
        else
        {
            horizotnalVelocity -= horizontalDeccel * Time.deltaTime;
        }

        horizotnalVelocity = Mathf.Clamp(horizotnalVelocity, 0, maxHorizontalSpeed);

        if (!onCurrent)
        {
            if (verticalVelocity != 0)
            {
                float verticalDirection = (verticalVelocity > 0) ? 1f : -1f;

                verticalVelocity += -verticalDirection * verticalDeccel * Time.deltaTime;

                if (verticalDirection == -1f)
                {
                    if (verticalVelocity > 0f) verticalVelocity = 0f;
                }
                else
                {
                    if (verticalVelocity < 0f) verticalVelocity = 0f;
                }
            }
        }

        rb.linearVelocity = new Vector2(horizotnalVelocity, verticalVelocity);

        UpdateMoveSound();
    }

    private void UpdateMoveSound()
    {
        if (moveAudio == null) return;

        float speed = rb.linearVelocity.magnitude;

        if (speed > minSpeedForSound)
        {
            if (!moveAudio.isPlaying)
            {
                moveAudio.pitch = Random.Range(movePitchMin, movePitchMax);
                moveAudio.Play();
            }
        }
        else
        {
            if (moveAudio.isPlaying)
            {
                moveAudio.Pause();
            }
        }
    }

    private void PlayExplosionSound()
    {
        if (explosionAudio == null) return;

        explosionAudio.pitch = Random.Range(explosionPitchMin, explosionPitchMax);
        explosionAudio.Play();
    }

    // TNT collision (requires TNT Collider2D with IsTrigger OFF)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tntTag))
        {
            // Optional: stop movement loop on explosion
            if (moveAudio != null && moveAudio.isPlaying)
                moveAudio.Stop();

            PlayExplosionSound();

            // Optional: destroy TNT after hit
            // Destroy(collision.gameObject);

            // Optional: call game over here if you want
            // LevelManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
        {
            onCurrent = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(currentTag))
            return;

        float radians = other.transform.rotation.z;

        Vector2 direction = new Vector2(
            Mathf.Cos(radians),
            Mathf.Sin(radians)
        );

        rb.linearVelocity = rb.linearVelocity + (direction * currentAccel * Time.fixedDeltaTime);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
        {
            onCurrent = false;
        }
    }
}
