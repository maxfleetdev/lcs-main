using TMPro;
using UnityEngine;

public class ShowStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cpuText;
    [SerializeField] private TextMeshProUGUI ramText;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI versionText;

    private int frameCount;
    private float nextUpdate;
    private float fps;
    private float updateRate = 4f;

    private void Start()
    {
        versionText.text = $"v{Application.version}";
    }

    private void Update()
    {
        CalculateFPS();
        CalculateMS();
    }

    private void CalculateFPS()
    {
        frameCount++;
        nextUpdate += Time.deltaTime;
        if (nextUpdate > 1f / updateRate)
        {
            fps = frameCount / nextUpdate;
            frameCount = 0;
            nextUpdate -= 1f / updateRate;
            string fps_text = string.Format("FPS: {0}", ((int)fps).ToString());

            if (fps < 30)
            {
                fpsText.text = $"<color=red>{fps_text}</color>";
            }
            else if (fps < 60)
            {
                fpsText.text = $"<color=yellow>{fps_text}</color>";
            }
            else
            {
                fpsText.text = $"<color=green>{fps_text}</color>";
            }
        }
    }

    private void CalculateMS()
    {
        float ms = 1000 / fps;
        string s_ms = string.Format("{0:0.00}", ms);
        cpuText.text = $"CPU: {s_ms}ms";
    }
}