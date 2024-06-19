using System;

public static class TimeScaleHandler
{
    public static event Action OnTimePause;
    public static event Action OnTimeResume;

    public static void PauseTime() => OnTimePause?.Invoke();
    public static void ResumeTime() => OnTimeResume?.Invoke();
}