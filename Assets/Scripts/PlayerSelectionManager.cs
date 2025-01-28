using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Linq;

public class PlayerSelectionManager : MonoBehaviour
{
    public PlayerSelectIcon[] allIcons;  // Assign all icons in the Inspector
    public Transform centerPoint; // Center of the circle
    public Button backButton; // Assign a UI Button in the Inspector

    private PlayerSelectIcon selectedIcon = null;

    void Start()
    {
        backButton.gameObject.SetActive(false); // Hide the back button at start
        backButton.onClick.AddListener(DeselectCharacter);
    }

    public void SelectCharacter(PlayerSelectIcon icon)
    {
        if (selectedIcon != null && selectedIcon != icon)
        {
            selectedIcon.ResetIcon();
        }

        selectedIcon = icon;
        backButton.gameObject.SetActive(true);

        // Hide all other icons
        foreach (PlayerSelectIcon i in allIcons)
        {
            if (i != icon)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

    public void DeselectCharacter()
    {
        if (selectedIcon != null)
        {
            selectedIcon.ResetIcon();
            selectedIcon = null;
        }

        // Show all icons again
        foreach (PlayerSelectIcon i in allIcons)
        {
            i.gameObject.SetActive(true);
        }

        backButton.gameObject.SetActive(false); // Hide back button
    }
}
