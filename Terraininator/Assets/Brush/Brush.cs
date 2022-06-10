using UnityEngine;

//Brush is an abstract class that keeps track of brush settings and has a method that can be called
//to apply the brush to the chunk system.
public abstract class Brush : MonoBehaviour
{
    //All brushes have a radius.
    public int radius = 5;

    //Some brushes might not have a variable intensity.
    // Some brushes need extra UI elements to make it clear what intensity does.
    //Bools like these are used to update the UI.
    public bool useIntensity;
    public bool useMarks;
    public float intensity = 6f;

    //Not all brushes have a colour.
    public bool useColour;
    public Color colour = Color.magenta;

    //Called every frame brush is applied.
    public abstract void Apply(ChunkSystem chunkSystem, RaycastHit hitData);

    //Called every time the brush settings change.
    public abstract void RecalculateAlteration();

    //Called frame after brush is applied.
    public abstract void Finish(ChunkSystem chunkSystem, RaycastHit hitData);
}