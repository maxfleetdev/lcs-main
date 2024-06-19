using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    private void Awake()
    {
        TimeScaleHandler.OnTimeResume += ResumeTime;
        TimeScaleHandler.OnTimePause += PauseTime;
    }

    private void OnDestroy()
    {
        TimeScaleHandler.OnTimeResume -= ResumeTime;
        TimeScaleHandler.OnTimePause -= PauseTime;
    }

    private void PauseTime()
    {
        Time.timeScale = 0;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1;
    }
}