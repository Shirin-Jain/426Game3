using UnityEngine;
using System.Collections;

public class WinCondition : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;          // can be Pier audio source
    [SerializeField] private AudioClip victoryClip;          // assign clip here
    [SerializeField] private AudioSource backgroundMusic;    // drag BGM source here
    [SerializeField] private float musicFadeDuration = 2f;

    private bool hasWon = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Pier hit by: " + collision.gameObject.name);

        if (hasWon) return;
        if (!collision.gameObject.CompareTag("Boat")) return;

        hasWon = true;

        if (sfxSource != null && victoryClip != null)
            sfxSource.PlayOneShot(victoryClip);

        if (backgroundMusic != null)
            StartCoroutine(FadeOutMusic(backgroundMusic, musicFadeDuration));

        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    private IEnumerator FadeOutMusic(AudioSource music, float duration)
    {
        float startVolume = music.volume;

        float t = 0f;
        while (t < duration)
        {
            music.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        music.volume = 0f;
        music.Stop();
    }
}
