using UnityEngine;

/*
    This class handles the procedural generation of the terrain heights.
    HeightMap() is the only function intended to be called from the outside:
        LayeredNoise() only helps HeightMap().
    The HeightMap() function returns a float[,] of heights.
*/
public class ProceduralGenerator : MonoBehaviour
{
    [Header("Base Settings")]
    //The seed of a perlin noise map consists of two values: x and y.
    //  These are simply added to any position on the noise.
    //  Coincidentally, this means that similar seeds look like offset versions of each other.
    //  (but I don't see this as a problem)
    public Vector2 Seed;

    public float Scale = 2f; //Higher values = bigger height differences.

    [Range(50, 250)]
    public float Zoom = 75f; //Higher values = more 'zoomed in'.
    
    [Header("Advanced Noise Settings")]
    
    public bool Ridged = false; //Ridging noise makes it sharper and linier, like a mountain range.
    [Min(1)]
    public int Octaves = 4; //More octaves = more details, but generating takes longer.
    [Min(1f)]
    public float Lacunarity = 2; //Higher lacunarity = more small details, at cost of large details.
    [Range(0, 1)]
    public float Persistance = 0.5f; //Higher persistance = smaller details contribute more to overall height.

    [Header("Height Controls")]
    //The height curve is a more versatile way of clamping the final noise value.
    //  Instead of simply lerping linearly, or setting values outside [0, 1] to 0 or 1,
    //  the curve can be manipulated to be basically any function.
    //  For example: An exponential curve will result in lots of flat terrain, with a couple
    //      very high mountains.
    public AnimationCurve HeightCurve;
    //The biome map is a way to control where mountains/hills/flat land will appear.
    //  Currently it works very simply: the brighter the colour on the map, the higher the terrain there (on average).
    //  But we could easily expand it by using multiple colour channels:
    //      e.g.: Red means higher, green means more height differences, blue means more persistance, etc.
    public Texture2D BiomeMap;
    [Range(0,1)]
    public float BiomeInfluence; //How much the biome map influences height.

    [Header("Waaaaaaaaarp")]
    //Warping noise is a practice where you use noise to find the position to sample on the noise map.
    //  At low levels (~5) it's effect is almost too subtle to notice,
    //  At medium levels (~20) it makes the terrain look sorta displaced, like dunes in a windy area.
    //  At high levels (~50) it creates these really cool 'alien' landscapes.
    public float WarpStrength = 0;
    
    public Vector2 WarpSeed = Vector2.zero;


    public float[,] HeightMap(int xSize, int zSize)
    {

        float[,] map = new float[xSize, zSize];
        float maxHeight = Mathf.NegativeInfinity;
        float minHeight = Mathf.Infinity;

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                //Facilitate warping the noise
                int sampleX = x;
                int sampleZ = z;
                if (WarpStrength != 0)
                {
                    sampleX += (int)(WarpStrength * LayeredNoise(x + WarpSeed.x, z + WarpSeed.y));
                    sampleZ += (int)(WarpStrength * LayeredNoise(x + WarpSeed.x, z + WarpSeed.y));
                }

                map[x,z] = LayeredNoise(sampleX, sampleZ);
                if (Ridged)
                    map[x,z] = Mathf.Abs((map[x,z] - 0.5f) * 2f);

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
                
                map[x,z] -= (1 - BiomeMap.GetPixel((x * BiomeMap.width) / xSize, 
                            (z * BiomeMap.height) / zSize).grayscale) * BiomeInfluence;

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