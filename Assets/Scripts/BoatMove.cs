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
    private bool isDead = false;

    [Header("Movement Audio")]
    [SerializeField] private AudioSource moveAudio;
    [SerializeField] private float minSpeedForSound = 0.1f;
    [SerializeField] private float movePitchMin = 0.95f;
    [SerializeField] private float movePitchMax = 1.05f;

    [Header("Explosion Audio")]
    [SerializeField] private AudioSource explosionAudio;
    [SerializeField] private float explosionPitchMin = 0.95f;
    [SerializeField] private float explosionPitchMax = 1.05f;

    [Header("Death Audio")]
    [SerializeField] private AudioSource deathAudio;
    [SerializeField] private float deathPitchMin = 0.95f;
    [SerializeField] private float deathPitchMax = 1.05f;

    [Header("Tags")]
    [SerializeField] private string tntTag = "TNT";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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

        if (deathAudio != null)
        {
            deathAudio.loop = false;
            deathAudio.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isDead) return;

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

        if (!onCurrent && verticalVelocity != 0)
        {
            float verticalDirection = verticalVelocity > 0 ? 1f : -1f;
            verticalVelocity += -verticalDirection * verticalDeccel * Time.deltaTime;

            if (verticalDirection == -1f && verticalVelocity > 0f)
                verticalVelocity = 0f;
            else if (verticalDirection == 1f && verticalVelocity < 0f)
                verticalVelocity = 0f;
        }

        rb.linearVelocity = new Vector2(horizotnalVelocity, verticalVelocity);

        UpdateMoveSound();
    }

    private void UpdateMoveSound()
    {
        if (moveAudio == null || isDead) return;

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
            moveAudio.Pause();
        }
    }

    private void PlayExplosionSound()
    {
        if (explosionAudio == null) return;
        explosionAudio.pitch = Random.Range(explosionPitchMin, explosionPitchMax);
        explosionAudio.Play();
    }

    private void PlayDeathSound()
    {
        if (deathAudio == null) return;
        deathAudio.pitch = Random.Range(deathPitchMin, deathPitchMax);
        deathAudio.Play();
    }

    // 💥 TNT collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag(tntTag))
        {
            isDead = true;

            // Stop movement immediately
            rb.linearVelocity = Vector2.zero;

            if (moveAudio != null)
                moveAudio.Stop();

            PlayExplosionSound();
            PlayDeathSound();

            // Optional extras:
            // Destroy(collision.gameObject); // remove TNT
            // LevelManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
            onCurrent = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(currentTag) || isDead)
            return;

        float radians = other.transform.rotation.z;

        Vector2 direction = new Vector2(
            Mathf.Cos(radians),
            Mathf.Sin(radians)
        );

        rb.linearVelocity += direction * currentAccel * Time.fixedDeltaTime;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(currentTag))
            onCurrent = false;
    }
}
