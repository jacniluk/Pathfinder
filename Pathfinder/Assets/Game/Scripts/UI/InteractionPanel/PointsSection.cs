public class PointsSection : InteractionPanelSection
{
    public override void Show()
    {
        InteractionPanel.Instance.SetSubmitButtonInteractable(false);

        base.Show();

        PathManager.Instance.EnableSelectingPointsMode(() => InteractionPanel.Instance.SetSubmitButtonInteractable(true));
    }

    public override void Back()
    {
        InteractionPanel.Instance.SetSubmitButtonInteractable(true);

        PathManager.Instance.DisableSelectingPointsMode();
        PathManager.Instance.ClearPath(false);
    }

    public void Clear()
    {
        InteractionPanel.Instance.SetSubmitButtonInteractable(false);

        PathManager.Instance.ClearPath(true);
    }
}
