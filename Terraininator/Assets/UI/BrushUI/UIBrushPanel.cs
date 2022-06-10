using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    GameObject Marks;
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
    }

}