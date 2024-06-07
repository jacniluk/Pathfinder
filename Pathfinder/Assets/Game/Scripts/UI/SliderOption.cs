using UnityEngine;
using UnityEngine.UI;

public class SliderOption : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float defaultValue01;

    [Header("Data")]
    [SerializeField] private UnityEventFloat changeValueEvent;

    [Header("References")]
    [SerializeField] private Text valueText;
    [SerializeField] private Slider2 slider;

    private void Start()
    {
        PrepareSettingsOption();
    }

    private void PrepareSettingsOption()
    {
        slider.PrepareSlider(defaultValue01);
        SetValueText(GetValue(defaultValue01));
    }

    public void ChangeValueEvent(float value01)
    {
        float value = GetValue(value01);

        changeValueEvent.Invoke(value);

        SetValueText(value);
    }

    private void SetValueText(float value01)
    {
        valueText.text = Mathf.Floor(value01).ToString();
    }

    private float GetValue(float value01)
    {
        return Utilities.Evaluate(value01, minValue, maxValue);
    }
}
