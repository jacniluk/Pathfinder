using UnityEngine;

public class InteractionPanelSection : MonoBehaviour
{
    [Header("Data")]
    [TextArea(2, 4)]
    [SerializeField] private string sectionMessage;

    [Header("References")]
    [SerializeField] private GameObject panel;

    public string SectionMessage => sectionMessage;

    public virtual void Show()
    {
        panel.SetActive(true);
    }

    public virtual void Hide()
    {
        panel.SetActive(false);
    }

    public virtual void Submit()
    {
        
    }

    public virtual void Back()
    {

    }
}
