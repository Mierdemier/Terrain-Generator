using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Class to be attached to the 'Procedural Generation' tab as a component.
//It handles some small utilities:
//  Automatically initialises the UI display elements to the default values so I don't have to keep changing them.
//  Generate() should be called whenever the 'Generate!' button is pressed.
//      It communicates the values from the UI to the ChunkSystem/ProceduralGenerator.
//      And tells them to make a new map.
public class UIProcGenPanel : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField]
    ProceduralGenerator generator;
    [SerializeField]
    ChunkSystem chunksystem;

    [Header("UI Elements")]
    [SerializeField]
    TMP_InputField SeedX;
    [SerializeField]
    TMP_InputField SeedY;
    [SerializeField]
    TMP_InputField Scale;
    [SerializeField]
    Slider Zoom;
    [SerializeField]
    Toggle Ridged;
    [SerializeField]
    TMP_InputField Octaves;
    [SerializeField]
    TMP_InputField Lacunarity;
    [SerializeField]
    Slider Persistance;
    [SerializeField]
    UICurveSelector HeightCurve;
    [SerializeField]
    UITextureSelector BiomeMap;
    [SerializeField]
    TMP_InputField WarpSeedX;
    [SerializeField]
    TMP_InputField WarpSeedY;
    [SerializeField]
    TMP_InputField WarpStrength;

    //Load in values from inspector.
    void Start()
    {
        SeedX.text = generator.Seed.x.ToString();
        SeedY.text = generator.Seed.y.ToString();
        Scale.text = generator.Scale.ToString();
        Zoom.value = generator.Zoom;

        Ridged.isOn = generator.Ridged;
        Octaves.text = generator.Octaves.ToString();
        Lacunarity.text = generator.Lacunarity.ToString();
        Persistance.value = generator.Persistance;

        WarpSeedX.text = generator.WarpSeed.x.ToString();
        WarpSeedY.text = generator.WarpSeed.y.ToString();
        WarpStrength.text = generator.WarpStrength.ToString();
    }

    public void Generate()
    {
        SetValues();

        chunksystem.GenerateFromScratch();
    }

    void SetValues()
    {
        generator.Seed = new Vector2(float.Parse(SeedX.text), float.Parse(SeedY.text));
        generator.Scale = float.Parse(Scale.text);
        generator.Zoom = Zoom.value;

        generator.Ridged = Ridged.isOn;
        generator.Octaves = int.Parse(Octaves.text);
        generator.Lacunarity = float.Parse(Lacunarity.text);
        generator.Persistance = Persistance.value;

        generator.HeightCurve = HeightCurve.GetSelected();
        generator.BiomeMap = BiomeMap.GetSelected();

        generator.WarpSeed = new Vector2(float.Parse(WarpSeedX.text), float.Parse(WarpSeedY.text));
        generator.WarpStrength = float.Parse(WarpStrength.text);
    }
}
