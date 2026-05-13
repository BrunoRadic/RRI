// PlayerHUD.cs
// Place in Assets/Scripts/
// Attach to: any GameObject in the scene

// Setup:
//   1. Create a Canvas
//   2. Add a Slider
//      - Min=0, Max=1, Interactable=OFF
//      - Rename to "StaminaSlider"
//   3. Add a Text for the state label
//   4. Drag references into script's Inspector fields 

using UnityEngine;
using UnityEngine.UI;
using TMPro;                   

public class PlayerHUD : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;

    [Header("UI Elements")]
    public Slider staminaSlider;
    public TextMeshProUGUI stateText;

    void Update()
    {
        if (player == null) return;

        // Stamina bar (0-1)
        if (staminaSlider != null)
            staminaSlider.value = player.CurrentStamina / player.MaxStamina;

        // State label
        if (stateText != null)
            stateText.text = player.CurrentState.ToString();
    }
}