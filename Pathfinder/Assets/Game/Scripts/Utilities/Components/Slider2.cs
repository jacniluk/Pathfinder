using UnityEngine;
using UnityEngine.UI;

public class Slider2 : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private UnityEventFloat changeValueEvent;

    [Header("References")]
    [SerializeField] private Image fill;
    [SerializeField] private RectTransform shifter;
    [SerializeField] private Slider2Handler handler;
    [SerializeField] private Transform invisibleShifter;

    public RectTransform Shifter => shifter;
    public Transform InvisibleShifter => invisibleShifter;

    public void PrepareSlider(float value01)
    {
        ChangeValue(value01, handler.CalculatePositionForShifter(value01));
    }

    public void ChangeValue(float value01, float positionX)
    {
        changeValueEvent.Invoke(value01);

        SetAppearance(value01, positionX);
    }

    private void SetAppearance(float value01, float positionX)
    {
        fill.fillAmount = value01;
        shifter.transform.localPosition = new Vector2(positionX, shifter.transform.localPosition.y);
    }
}
