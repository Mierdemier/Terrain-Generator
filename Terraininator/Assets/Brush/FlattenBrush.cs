using UnityEngine;

public class FlattenBrush : Brush
{
    public override void Apply(ChunkSystem chunkSystem, RaycastHit hitData, float timeApplied)
    {
        Vector3 hitPosition = hitData.point;
        int xCoor = (int)hitPosition.x;
        int zCoor = (int)hitPosition.z;

        //Edit terrain in heightmap.
        float[,] flatness = new float[2 * radius + 1, 2 * radius + 1];
        int threshold = radius * radius;
        for (int x = 0; x <= radius; x++)
        {
            for (int z = 0; z <= radius; z++)
            {
                flatness[x,z] = (x - radius) * (x - radius) + (z - radius) * (z - radius)
                 < threshold ? intensity / 10f : 0;

                //lifehack
                flatness[2 * radius - x , z] = flatness[x,z];
                flatness[x, 2 * radius - z] = flatness[x,z];
                flatness[2 * radius - x, 2 * radius - z] = flatness[x,z];
            }
        }

        chunkSystem.FlattenHM(flatness, new Vector2Int(xCoor - radius, zCoor - radius));
    }

    public override void Finish(ChunkSystem chunkSystem, RaycastHit hitData)
    {
        chunkSystem.RegenerateCollisions();
    }
}