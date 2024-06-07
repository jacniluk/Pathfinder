using UnityEngine;

public class MapSizeSection : InteractionPanelSection
{
    public void ChangeMapSizeWidth(float value)
    {
        MapManager.Instance.SetMapSizeWidth(Mathf.Floor(value));
    }

    public void ChangeMapSizeLength(float value)
    {
        MapManager.Instance.SetMapSizeLength(Mathf.Floor(value));
    }
}
