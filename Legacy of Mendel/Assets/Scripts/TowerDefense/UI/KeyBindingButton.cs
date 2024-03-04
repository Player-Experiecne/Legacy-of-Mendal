using TMPro;
using UnityEngine;

public class KeyBindingButton : MonoBehaviour
{
    public string actionToRebind; // Set this in the inspector to match the action name in the InputManager
    public TextMeshProUGUI buttonText; // Optional, for updating button text to new key
    public GameObject keyBindingInstruction;

    private void Start()
    {
        buttonText.text = InputManager.Instance.GetCurrentKeyForAction(actionToRebind).ToString();
    }
    public void OnRebindButtonClick()
    {
        keyBindingInstruction.SetActive(true);
        InputManager.Instance.StartRebindProcess(actionToRebind, () =>
        {
            // Optional callback action after rebind completes
            keyBindingInstruction.SetActive(false);
            KeyCode newKey = InputManager.Instance.GetCurrentKeyForAction(actionToRebind);
            buttonText.text = newKey.ToString(); // Update button text to show new key
        });
    }
}
