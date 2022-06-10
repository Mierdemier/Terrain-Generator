using UnityEngine;
using System;


//This brush flattens the terrain around it.
public class FlattenBrush : Brush
{
    //Instead of altering the heightmap directly, we store an alteration and apply that.
    //This allows us to only recalculate the alteration when the brush settings are changed instead of every frame.
    float[,] alteration;

    public override void RecalculateAlteration()
    {
        //2 * radius + 1 = diameter (the 1 is the vertex in the middle)
        alteration = new float[2 * radius + 1, 2 * radius + 1];
        int threshold = radius * radius;
        for (int x = 0; x <= radius; x++)
        {
            for (int z = 0; z <= radius; z++)
            {
                //Equation for a circle: (x - xPosition)^2 + (y - yPosition)^2 = radius^2
                alteration[x,z] = (x - radius) * (x - radius) + (z - radius) * (z - radius)
                 < threshold ? intensity / 5f : 0;

                //lifehack:
                //Circles are quadrilaterally symmetrical. So no need to calculate for every point.
                alteration[2 * radius - x , z] = alteration[x,z];
                alteration[x, 2 * radius - z] = alteration[x,z];
                alteration[2 * radius - x, 2 * radius - z] = alteration[x,z];
            }
        }
    }

    //On Apply: FlattenHM.
    public override void Apply(ChunkSystem chunkSystem, RaycastHit hitData)
    {
        Vector3 hitPosition = hitData.point;
        int xCoor = (int)hitPosition.x;
        int zCoor = (int)hitPosition.z;

        chunkSystem.FlattenHM(alteration, new Vector2Int(xCoor - radius, zCoor - radius));
    }

    //Only do the (expensive) collider calculations when you're done.
    public override void Finish(ChunkSystem chunkSystem, RaycastHit hitData)
    {
        chunkSystem.RegenerateCollisions();
    }
}