using UnityEngine;

public class Slider2Handler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Slider2 slider;

    private float minPosition;
    private float maxPosition;

    private void Awake()
    {
        float shifterHalfWidth = slider.Shifter.rect.width / 2.0f;
        minPosition = rectTransform.rect.xMin + shifterHalfWidth;
        maxPosition = rectTransform.rect.xMax - shifterHalfWidth;
    }

    public void ShiftEvent()
    {
        slider.InvisibleShifter.transform.position = new Vector3(Input.mousePosition.x, slider.InvisibleShifter.transform.position.y, 0.0f);
        float value01 = Utilities.CalculateProgress01(slider.InvisibleShifter.transform.localPosition.x, minPosition, maxPosition);
        float position = Mathf.Clamp(slider.InvisibleShifter.transform.localPosition.x, minPosition, maxPosition);
        slider.ChangeValue(value01, position);
    }

    public float CalculatePositionForShifter(float zeroOne)
    {
        return Utilities.Evaluate(zeroOne, minPosition, maxPosition);
    }
}
