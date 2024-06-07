using UnityEngine;

public class ObstaclesSection : InteractionPanelSection
{
    public override void Show()
    {
        base.Show();

        ObstaclePlacingManager.Instance.EnablePlacingMode();
    }

    public override void Hide()
    {
        base.Hide();

        ObstaclePlacingManager.Instance.DisablePlacingMode();
    }

    public override void Submit()
    {
        NavigationManager.Instance.BuildNavigationSystem();
    }

    public override void Back()
    {
        Clear();
    }

    public void ChangeRandomObstaclesCount(float value)
    {
        MapManager.Instance.SetRandomObstaclesCount(Mathf.FloorToInt(value));
    }

    public void Random()
    {
        MapManager.Instance.PlaceRandomObstacles();
    }

    public void Clear()
    {
        MapManager.Instance.ClearObstacles();
    }
}
