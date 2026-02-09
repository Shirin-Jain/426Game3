using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Timer : MonoBehaviour
{
    public Slider timerSlider;
    public TMP_Text timerText;
    public float gameTime = 120f;

    private bool stopTimer;

    void Start()
    {
        stopTimer = false;
        timerSlider.maxValue = gameTime;
        timerSlider.minValue = 0;
    }

    void Update()
    {
        if (stopTimer) return;

        float time = gameTime - Time.time;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerSlider.value = time;

        if (time <= 0)
        {
            stopTimer = true;
        }
    }
}
