using UnityEngine;

public class ColourBrush : Brush
{
    public override void Apply(ChunkSystem chunksystem, RaycastHit hitData, float timeApplied)
    {
        Vector3 hitPosition = hitData.point;
        int xCoor = (int)hitPosition.x;
        int zCoor = (int)hitPosition.z;

        chunksystem.AlterColours(colour, new Vector2Int(xCoor - radius, zCoor - radius), 2 * radius + 1);
    }
}
