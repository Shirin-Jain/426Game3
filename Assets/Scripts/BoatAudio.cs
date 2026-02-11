using UnityEngine;

public class BoatAudio : MonoBehaviour
{

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


    [Header("Audio SFX")]
    [SerializeField] private AudioSource crateBreakAudio;
    [SerializeField] private AudioSource crashAudio;
    [SerializeField] private AudioSource powerUpAudio;






    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;

    void Start()
    {

        rb = GetComponent<Rigidbody2D>();


        if (moveAudio != null)
        {
            moveAudio.loop = true;
            //TODO only when moving
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

        if(crateBreakAudio != null)
        {
            crateBreakAudio.loop = false;
            crateBreakAudio.playOnAwake = false;
        }

        if (crashAudio != null)
        {
            crashAudio.loop = false;
            crashAudio.playOnAwake = false;
        }

        if (powerUpAudio != null)
        {
            powerUpAudio.loop = false;
            powerUpAudio.playOnAwake = false;
        }

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


    public void PlayCrateBreakSound()
    {
        if (crateBreakAudio == null) return;
        crateBreakAudio.Play();

        Debug.Log("Crate break");


    }

    public void PlayCrashSound(float volume)
    {
        Debug.Log("Crash");

        if (crashAudio == null) return;
        crashAudio.volume = volume;
        crashAudio.Play();


    }

    public void PlayPowerUpSound()
    {
        if (powerUpAudio == null) return;
        powerUpAudio.Play();

        Debug.Log("Power up");

    }

    public void PlayExplosionSound()
    {
        if (explosionAudio == null) return;
        explosionAudio.pitch = Random.Range(explosionPitchMin, explosionPitchMax);
        explosionAudio.Play();

        Debug.Log("TnT SOund");
    }

    public void PlayDeathSound()
    {
        if (deathAudio == null) return;
        deathAudio.pitch = Random.Range(deathPitchMin, deathPitchMax);
        deathAudio.Play();
        Debug.Log("Death SOund");

    }
    // Update is called once per frame
    void Update()
    {
        UpdateMoveSound();


    }
}
