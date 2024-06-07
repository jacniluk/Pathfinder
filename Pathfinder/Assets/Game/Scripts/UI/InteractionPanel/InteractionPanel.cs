using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text sectionMessage;
    [SerializeField] private Button backButton;
    [SerializeField] private Button submitButton;
    [SerializeField] private List<InteractionPanelSection> sections;

    public static InteractionPanel Instance;

    private int currentSectionIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ChangeSection(0, true);
    }

    public void ChangeSection(int index, bool onStart = false)
    {
        if (onStart == false)
        {
            sections[currentSectionIndex].Hide();
        }
        currentSectionIndex = index;
        sections[currentSectionIndex].Show();

        sectionMessage.text = sections[currentSectionIndex].SectionMessage;
        backButton.gameObject.SetActive(currentSectionIndex > 0);
        submitButton.gameObject.SetActive(currentSectionIndex < sections.Count - 1);
    }

    public void Back()
    {
        sections[currentSectionIndex].Back();

        ChangeSection(currentSectionIndex - 1);
    }

    public void Submit()
    {
        sections[currentSectionIndex].Submit();

        ChangeSection(currentSectionIndex + 1);
    }

    public void SetSubmitButtonInteractable(bool state)
    {
        submitButton.interactable = state;
    }
}
