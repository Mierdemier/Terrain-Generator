using UnityEngine;

//This brush changes the colour of the terrain in its radius.
public class ColourBrush : Brush
{
    //On apply: alter colours in diameter.
    public override void Apply(ChunkSystem chunksystem, RaycastHit hitData)
    {
        Vector3 hitPosition = hitData.point;
        int xCoor = (int)hitPosition.x;
        int zCoor = (int)hitPosition.z;

        chunksystem.AlterColours(colour, new Vector2Int(xCoor - radius, zCoor - radius), 2 * radius + 1);
    }

    public override void Finish(ChunkSystem chunkSystem, RaycastHit hitData)
    {
        //Everything is done in apply.
        return;
    }

    public override void RecalculateAlteration()
    {
        //We don't have an alteration.
        return;
    }
}
