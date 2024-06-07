using UnityEngine;

public class HelpPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject panel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            panel.SetActive(panel.activeSelf == false);
        }
    }
}
