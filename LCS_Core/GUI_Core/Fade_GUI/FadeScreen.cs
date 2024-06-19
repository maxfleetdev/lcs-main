using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private Image fadeImage;

    private bool isFaded = false;

    private float fadeValue = 0f;
    private float fadeTime = 1f;

    private void Start()
    {
        fadeImage = GetComponent<Image>();
        if (fadeImage == null)
        {
            Debug.LogError("Image component not found.");
            return;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    private IEnumerator BeginFade()
    {
        if (fadeImage == null) yield break;

        float normalised_time = 0f;
        float current_alpha = isFaded ? 1f : 0f;
        float wish_alpha = isFaded ? 0f : 1f;
        isFaded = !isFaded;
        while (normalised_time <= 1f)
        {
            fadeValue = Mathf.Lerp(current_alpha, wish_alpha, normalised_time);
            fadeImage.color = new Color(0, 0, 0, fadeValue);
            normalised_time += Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, wish_alpha);
    }

    public void ToggleFade()
    {
        if (fadeImage != null)
        {
            StopAllCoroutines();
            StartCoroutine(BeginFade());
        }
    }
}