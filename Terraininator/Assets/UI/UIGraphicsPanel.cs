using UnityEngine;
using UnityEngine.UI;

//The UIGraphicsPanel is a collection of methods to be stored in the graphics tab and triggered by UI elements there.
//This ended up being a bit messy, because of quirks in the UnityEngine.UI system.

//Basically, if a method can be done using only one argument, we pass in that argument using whatever slider sets it.
//But for more complex methods we read the UI elements directly through this script, because Unity doesn't like when you
//make a UI element trigger multiple parameters in a function.
public class UIGraphicsPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Transform OceanTransform;
    [SerializeField]
    Material OceanMat;
    [Space]
    [SerializeField]
    ProceduralGenerator generator;
    [SerializeField]
    ChunkSystem chunksystem;
    
    [SerializeField]
    Light Sun;

    [Header("UI Elements")]
    [SerializeField]
    Slider WaterLevelSlider;
    [SerializeField]
    Image WaterShallowSquare;
    [SerializeField]
    Image WaterDeepSquare;
    [Space]

    [SerializeField]
    UISkySelector SkySelector;

    [Space]

    [SerializeField]
    Slider SlopeSlider;
    [SerializeField]
    Slider[] HeightSliders;

    void Start()
    {
        WaterLevelSlider.value = OceanTransform.position.y;
        WaterShallowSquare.color = OceanMat.GetColor("_ShallowColour");
        WaterDeepSquare.color = OceanMat.GetColor("_DeepColour");

        //Just set the terrain and sky to the default values manually.
    }

    public void SetWaterLevel(float level)
    {
        OceanTransform.position = new Vector3(0, level, 0);
    }

    public void SetWaterDepth(float depth)
    {
        OceanMat.SetFloat("_Depth", depth);
    }

    public void SetWaterColour(Color colour, string colourVariable)
    {
        OceanMat.SetColor(colourVariable, colour);
    }

    public void SetTerrainColour(Color colour, int colourIndex)
    {
        int numColours = generator.TerrainTypes.Length;
        //High indices are for slope colours.
        if (colourIndex >= numColours)
            generator.TerrainTypes[colourIndex - numColours].slopeyColour = colour;
        else
            generator.TerrainTypes[colourIndex].flatColour = colour;

        chunksystem.GenerateTextures(Vector2Int.zero, chunksystem.numChunks - new Vector2Int(1, 1), true);
    }
    public void SetTerrainColourHeight(int index)
    {
        generator.TerrainTypes[index].height = HeightSliders[index].value;

        chunksystem.GenerateTextures(Vector2Int.zero, chunksystem.numChunks - new Vector2Int(1, 1), true);
    }
    public void SetTerrainColourSlope()
    {
        generator.SlopeThreshold = SlopeSlider.value;

        chunksystem.GenerateTextures(Vector2Int.zero, chunksystem.numChunks - new Vector2Int(1, 1), true);
    }

    public void SetSky()
    {
        SkyData sky = SkySelector.GetSelected();

        RenderSettings.skybox = sky.Skybox;

        RenderSettings.ambientIntensity = sky.AmbientIntensity;

        Sun.color = sky.SunColour;
        Sun.intensity = sky.SunIntensity;
        Sun.transform.rotation = Quaternion.Euler(sky.SunAngle.x, sky.SunAngle.y, sky.SunAngle.z);
    }
}
