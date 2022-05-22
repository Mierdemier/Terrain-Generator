using UnityEngine;

/*
    This class handles the procedural generation of the terrain.
    Currently, HeightMap() is the only function intended to be called from the outside:
        LayeredNoise() only helps HeightMap().
    The HeightMap() function returns a float[,] of heights.
*/
public class ProceduralGenerator : MonoBehaviour
{
    [SerializeField]
    //The seed of a perlin noise map consists of two values: x and y.
    //  These are simply added to any position on the noise.
    //  Coincidentally, this means that similar seeds look like offset versions of each other.
    //  (but I don't see this as a problem)
    Vector2 Seed;

    [SerializeField]
    //The height curve is a more versatile way of clamping the final noise value.
    //  Instead of simply lerping linearly, or setting values outside [0, 1] to 0 or 1,
    //  the curve can be manipulated to be basically any function.
    //  For example: An exponential curve will result in lots of flat terrain, with a couple
    //      very high mountains.
    AnimationCurve HeightCurve;
    
    [SerializeField]
    float Scale = 2f; //Higher values = bigger height differences.
    [SerializeField] [Range(50, 250)]
    float Zoom = 75f; //Higher values = more 'zoomed in'.
    [SerializeField] [Min(1)]
    int Octaves = 4; //More octaves = more details, but generating takes longer.
    [SerializeField] [Min(1f)]
    float Lacunarity = 2; //Higher lacunarity = more small details, at cost of large details.
    [SerializeField] [Range(0, 1)]
    float Persistance = 0.5f; //Higher persistance = smaller details contribute more to overall height.

    //This function uses the layered noise to create an entire procedural map.
    public float[,] HeightMap(int xSize, int zSize)
    {
        float[,] map = new float[xSize, zSize];
        float maxHeight = Mathf.NegativeInfinity;
        float minHeight = Mathf.Infinity;

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                map[x,z] = LayeredNoise(x, z);

                if (map[x,z] > maxHeight)
                    maxHeight = map[x,z];
                else if (map[x,z] < minHeight)
                    minHeight = map[x,z];
            }    
        }
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                map[x,z] = Mathf.InverseLerp(minHeight, maxHeight, map[x,z]);
                map[x,z] = HeightCurve.Evaluate(map[x,z]) * Scale;
            }
        }

        return map;
    }

    //This function returns a layered perlin noise value for randomising terrain.
    float LayeredNoise(float x, float z)
    {
        float height = 0;

        //Local copies of the variables that can be changed without affecting other noise values.
        float frequency = 1;
        float amplitude = 1;

        //Must be a new vector so it doesn't become a pointer to Seed.
        Vector2 currentSeed = new Vector2(Seed.x, Seed.y);

        for (int i = 0; i < Octaves; i++)
        {
            float sampleX = x * frequency / Zoom + currentSeed.x;
            float sampleZ = z * frequency / Zoom + currentSeed.y;

            height += (Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1) * amplitude;

            amplitude *= Persistance;
            frequency *= Lacunarity;
            //This looks random enough, even though it's technically deterministic.
            currentSeed.x += 1423.157158923f;
            currentSeed.y -= 4362.781592748f;
        }

        return height;
    }
}
