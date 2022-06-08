using UnityEngine;

//Brush is an abstract class that keeps track of brush settings and has a method that can be called
//to apply the brush to the chunk system.
public abstract class Brush : MonoBehaviour
{
    protected int radius = 5;
    protected float intensity = 6f;
    protected Color colour = Color.magenta;

    //Called every frame brush is applied.
    public abstract void Apply(ChunkSystem chunkSystem, RaycastHit hitData, float timeApplied);

    //Called frame after brush is applied.
    public abstract void Finish(ChunkSystem chunkSystem, RaycastHit hitData);

    public int GetRadius() {return radius;}
}