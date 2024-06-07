using UnityEngine;
using UnityEngine.UI;

public class SummarySection : InteractionPanelSection
{
    [Header("References")]
    [SerializeField] private Text summaryText;
    [SerializeField] private Button runnerButton;

    public override void Show()
    {
        bool result = PathManager.Instance.BuildPath();
        if (result)
        {
            summaryText.text = "Path found! The algorithm time was: " + NavigationManager.Instance.FindingPathTime * 1000.0f + " ms.";
            runnerButton.interactable = true;
        }
        else
        {
            summaryText.text = "There is no path between these two points.";
            runnerButton.interactable = false;
        }

        base.Show();
    }

    public override void Back()
    {
        PathManager.Instance.ClearPath();
        PathManager.Instance.HideRunner();
    }

    public void Runner()
    {
        runnerButton.interactable = false;

        PathManager.Instance.ShowRunner(() => runnerButton.interactable = true);
    }
}
