using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChanger : MonoBehaviour
{
    public Button myButton; // Assign this in the inspector

    private void Start()
    {
        // Add an onClick listener to your button
        myButton.onClick.AddListener(ChangeButtonColor);
    }

    private void ChangeButtonColor()
    {
        ColorBlock cb = myButton.colors;
        cb.normalColor = Color.green; // Change the normal color to green
        cb.highlightedColor = Color.green; // Optional: also change the highlighted color to green
        cb.pressedColor = Color.green; // Optional: also change the pressed color to a slightly darker green
        cb.selectedColor = Color.green; // Optional: also change the selected color to green
        myButton.colors = cb;
    }
}
