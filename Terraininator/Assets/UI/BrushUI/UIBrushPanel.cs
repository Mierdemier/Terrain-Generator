using UnityEngine;
using TMPro;
using UnityEngine.UI;

//This class controls the UI for the brushes.
//It has two public methods, intended to be called by UnityEngine.UI elements:
//      UpdateBrush() sets the currently selected brush based on the selector and updates UI to show its settings.
//      SetBrushSettings() takes information from the UI to set the settings of the currently selected brush.
public class UIBrushPanel : MonoBehaviour
{
    [SerializeField]
    mouseInput Mouse;
    [SerializeField]
    UIBrushSelector BrushSelector;
    [SerializeField]
    Slider RadiusSlider;
    [SerializeField]
    Slider IntensitySlider;
    [SerializeField]
    GameObject Marks;               //Some brushes use special marks to indicate extra information.
    [SerializeField]
    Image ColourSquare;

    void Start()
    {
        UpdateBrush();
    }

    public void UpdateBrush()
    {
        Brush brush = BrushSelector.GetSelected();

        IntensitySlider.transform.parent.gameObject.SetActive(brush.useIntensity);
        Marks.SetActive(brush.useMarks);
        ColourSquare.transform.parent.gameObject.SetActive(brush.useColour);

        RadiusSlider.value = brush.radius;
        IntensitySlider.value = brush.intensity;
        ColourSquare.color = brush.colour;

        Mouse.SetBrush(brush);
    }

    public void SetBrushSettings()
    {
        Brush brush = BrushSelector.GetSelected();

        brush.radius = (int)RadiusSlider.value;
        brush.intensity = IntensitySlider.value;
        brush.colour = ColourSquare.color;

        brush.RecalculateAlteration();
    }

}
