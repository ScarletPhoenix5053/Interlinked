using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class TimerBar : MonoBehaviour
{
    // Slaved to a phased clock object
    // Adjusts a slider based on value passed through

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    
    public void UpdateValue(float newValue)
    {
        _slider.value = newValue;
    }
}
