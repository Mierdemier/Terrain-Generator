using UnityEngine;
using UnityEngine.UI;

//This UI class works with a FlexibleColorPicker asset (Selector) to set the colours of ingame things through UI.
//To accomplish this it keeps track of:
//  A previousColour; to revert back to if the colour picking is canceled.
//  A display image; an image in the UI (separate from the selector) where the colour can be previewed.
//  A string indicating which shader property is being edited.

//When the user wants to pick a new colour use:
//  SetDisplay() to configure where this colour will be displayed, and enable the Selector too if it wasn't enabled.
//  SetEditing() to configure which shader property will be edited.
//When the user is done picking a colour using the Selector he can either:
//  Cancel(), reverting back to the previous colour.
//  SetWaterColour(), setting a colour in the watershader.
public class UIColourPicker : MonoBehaviour
{
    [SerializeField]
    FlexibleColorPicker Selector; //The UI element that will be used to pick the colour.
    [SerializeField]
    UIGraphicsPanel panel;

    Image display;
    Color previousColour;
    string currentlyEditing;
    int IBasedCurrentlyEditing;

    void Update()
    {
        if (display != null)
            display.color = Selector.color;
    }

    public void SetDisplay(Image newDisplay)
    {
        previousColour = newDisplay.color;
        display = newDisplay;

        Selector.gameObject.SetActive(true);
        Selector.color = previousColour;
    }
    public void SetEditing(string toEdit)
    {
        currentlyEditing = toEdit;
    }
    public void SetEditing(int toEdit)
    {
        IBasedCurrentlyEditing = toEdit;
    }



    public void Cancel()
    {
        Selector.gameObject.SetActive(false);

        display.color = previousColour;

        display = null;
    }

    public void SetWaterColour()
    {
        panel.SetWaterColour(Selector.color, currentlyEditing);
        Selector.gameObject.SetActive(false);
    }
    public void SetTerrainColour()
    {
        panel.SetTerrainColour(Selector.color, IBasedCurrentlyEditing);
        Selector.gameObject.SetActive(false);
    }
}
