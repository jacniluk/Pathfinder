using UnityEngine;
using UnityEngine.UI;

public class MoveSpeedPanel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float showDuration;

    [Header("References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Text moveSpeedText;

    public static MoveSpeedPanel Instance;

	private void Awake()
    {
        Instance = this;
    }

    public void Show(float moveSpeed)
    {
        if (panel.activeSelf == false)
        {
            panel.SetActive(true);
        }
        else
        {
            CancelInvoke(nameof(Hide));
        }
        moveSpeedText.text = "x" + moveSpeed;
        Invoke(nameof(Hide), showDuration);
    }

    private void Hide()
    {
        panel.SetActive(false);
    }
}
