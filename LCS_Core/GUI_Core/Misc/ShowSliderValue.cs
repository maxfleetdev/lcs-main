using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValue : MonoBehaviour
{
    [SerializeField] private Slider targetSlider;

    private float rawValue;
    private int shownValue;

    private void OnEnable()
    {
        
    }
}