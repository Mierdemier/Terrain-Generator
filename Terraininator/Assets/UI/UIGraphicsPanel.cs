using UnityEngine;
using UnityEngine.UI;

public class UIGraphicsPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Transform oceanTransform;
    [SerializeField]
    Material oceanMat;

    [Header("UI Elements")]
    [SerializeField]
    Slider WaterLevelSlider;
    [SerializeField]
    Image WaterShallowSquare;
    [SerializeField]
    Image WaterDeepSquare;

    void Start()
    {
        WaterLevelSlider.value = oceanTransform.position.y;
        WaterShallowSquare.color = oceanMat.GetColor("_ShallowColour");
        WaterDeepSquare.color = oceanMat.GetColor("_DeepColour");
    }

    public void SetWaterLevel(float level)
    {
        oceanTransform.position = new Vector3(0, level, 0);
    }

    public void SetWaterDepth(float depth)
    {
        oceanMat.SetFloat("_Depth", depth);
    }

    public void SetWaterColour(Color colour, string colourVariable)
    {
        oceanMat.SetColor(colourVariable, colour);
    }
}
