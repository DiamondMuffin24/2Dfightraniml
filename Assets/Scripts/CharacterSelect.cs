using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public void OnCharacterSelected()
    {
        Debug.Log($"Selected character: {gameObject.name}");
        // Add logic to highlight the selected character or load the game
    }
}
