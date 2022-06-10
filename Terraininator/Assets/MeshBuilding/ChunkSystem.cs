using UnityEngine;

[RequireComponent(typeof(ProceduralGenerator))]
//The ChunkSystem is the central class that stores the state of the entire terrain as a whole.
//  In Start(), it checks whether a save file is being loaded. If not, it prepares some procedural terrain.
//  This class has many methods that are intended to be called from the outside:
//      GenerateMeshes() tells the system to regenerate the meshes of the terrain chunks. This is automatically called by
//          the chunksystem itself whenever necessary, but can be manually called as well.
//      GenerateTextures() does the same for the terrain chunk textures.
//      RegenerateCollisions() checks which meshes have changed since the last time it was called, and updates the
//          colliders of those meshes. 
//          This is done separately from GenerateMeshes() so it can be called less often for performance reasons.
//      
//      GenerateFromScratch() generates an entire new procedural map.
//      GenerateFromMap() generates terrain using a Texture as a heightmap (brighter colours = higher)
//      GenerateFromSave() generates terrain from a deserialised save file.
//
//      AlterHM() adds to the heightmap and updates the meshes to display the change.
//      FlattenHM() flattens the heightmap and updates the meshes to display the change.
//      AlterColours() replaces the colours and updates the textures to display the change.
public class ChunkSystem : MonoBehaviour
{

    [Range(8, 248)]
    //The size of each (square) chunk. Must be a multiple of 8!
    public int ChunkSize = 248;
    public Vector2Int numChunks;
    [SerializeField]
    GameObject ChunkPrefab;
    [SerializeField]
    CameraScript Camera;    //Only used to update the max/min camera zoom.


    ProceduralGenerator generator;

    //You can access the HM/CM if you really need to (prefer to just pass it as a parameter)
    //But only the chunksystem is allowed to edit it.
    float[,] globalHM;  //Global heightmap, don't lose this!
    public float[,] GlobalHM {get {return globalHM;} } 
    Color[,] globalColours; //Global colour map.
    public Color[,] GlobalColours {get {return globalColours;} }

    TerrainBuilder[,] chunks; //Contains references to every chunk.
    bool[,] chunkCollisionDirty; //Stores whether the chunk needs to get new collider.

    void Start()
    {
        //Find generator.
        generator = GetComponent<ProceduralGenerator>();
        generator.Seed = new Vector2(Random.Range(0f, 10000f), Random.Range(0f, 10000f));

        //If we're not loading any save, create a new map and procedurally generate some starting terrain.
        if (!PlayerPrefs.HasKey("save"))
        {
            //Find player's preferred canvas size, or make one up if he didn't specify.
            numChunks = new Vector2Int(PlayerPrefs.GetInt("xChunks", 2), PlayerPrefs.GetInt("zChunks", 2));

            //Spawn terrain chunks
            chunks = new TerrainBuilder[numChunks.x , numChunks.y];
            chunkCollisionDirty = new bool[numChunks.x, numChunks.y];

            for(int x = 0; x < numChunks.x; x++)
            {
                for (int z = 0; z < numChunks.y; z++)
                {
                    //Create a new GameObject in the correct position to hold the chunk.
                        GameObject newChunk = Instantiate(ChunkPrefab, 
                    transform.position + new Vector3(x * (ChunkSize - 1), 0, z * (ChunkSize - 1)), 
                    Quaternion.identity);
                
                 //Store a reference to the chunk.
                    chunks[x,z] = newChunk.GetComponent<TerrainBuilder>();
                }
            }

            GenerateFromScratch();
        }
    }

    /*------------------------- Generate individual elements. ------------------------------------------------------*/

    //END IS INCLUSIVE!!!
    public void GenerateMeshes(Vector2Int start, Vector2Int end)
    {
        //Precondition
        if (start.x < 0 || start.y < 0 || end.x < start.x || end.y < start.y || end.x >= numChunks.x || end.y >= numChunks.y)
            return;

        for(int x = start.x; x <= end.x; x++)
        {
            for (int z = start.y; z <= end.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int chunkStart = new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1));
                chunks[x,z].GenerateMesh(globalHM, chunkStart, ChunkSize);
                chunkCollisionDirty[x,z] = true;
            }
        }
    }

    //Making an entire separate method just to generate procedural colours felt kind of awkward, so you can create
    //a new procedural colour map and immediately apply it by calling this method with overWriteColours = true.
    //End is inclusive.
    public void GenerateTextures(Vector2Int start, Vector2Int end, bool overwriteColours = false)
    {
        //Precondition
        if (start.x < 0 || start.y < 0 || end.x < start.x || end.y < start.y || end.x >= numChunks.x || end.y >= numChunks.y)
            return;

        if (overwriteColours)
            globalColours = generator.AddColour(globalHM);

        //Give terrain chunks each a section of the colours to render.
        for(int x = 0; x <= end.x; x++)
        {
            for (int z = 0; z <= end.y; z++)
            {
                //Make a texture for the chunk
                Vector2Int chunkStart = new Vector2Int(x * ChunkSize , z * ChunkSize);
                chunks[x,z].GetComponent<ColourMap>().TextureFromColourMap(globalColours, chunkStart, ChunkSize);
            }
        }
    }

    public void RegenerateCollisions()
    {
        for (int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                if (chunkCollisionDirty[x,z])
                {
                    chunks[x,z].UpdateCollisions();
                    chunkCollisionDirty[x,z] = false;
                }
            }
        }
    }


    /*------------------------------- Generate whole maps. -------------------------------------------------------------*/

    public void GenerateFromScratch()
    {
        //Set globalHM to procedural Heightmap.
        globalHM = generator.HeightMap(ChunkSize * numChunks.x, ChunkSize * numChunks.y);

        GenerateMeshes(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
        RegenerateCollisions();
        GenerateTextures(Vector2Int.zero, numChunks - new Vector2Int(1, 1), true);

        //Set camera zoom
        Camera.setZoom(numChunks.x * (-100f), generator.Scale * (-2f));
    }

    public void GenerateFromMap(Texture2D map)
    {
        int xSize = globalHM.GetLength(0);
        int zSize = globalHM.GetLength(1);

        //The brightness of the map at this point ~ it's height.
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
                globalHM[x,z] = map.GetPixel((x * map.width) / xSize, (z * map.height) / zSize).grayscale * 50;
        }

        //Update
        GenerateMeshes(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
        RegenerateCollisions();
        GenerateTextures(Vector2Int.zero, numChunks - new Vector2Int(1, 1), true);
    }

    public void GenerateFromSave(Save save)
    {
        //Spawn terrain chunks
        numChunks = new Vector2Int(save.xChunks, save.zChunks);
        chunks = new TerrainBuilder[numChunks.x , numChunks.y];
        chunkCollisionDirty = new bool[numChunks.x, numChunks.y];
        for(int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                //Create a new GameObject in the correct position to hold the chunk.
                GameObject newChunk = Instantiate(ChunkPrefab, 
                transform.position + new Vector3(x * (ChunkSize - 1), 0, z * (ChunkSize - 1)), 
                Quaternion.identity);
                
                //Store a reference to the chunk.
                chunks[x,z] = newChunk.GetComponent<TerrainBuilder>();
            }
        }

        //Load heights and colours from save.
        int xSize = save.savedHM.GetLength(0);
        int zSize = save.savedHM.GetLength(1);
        globalHM = new float[xSize, zSize];
        globalColours = new Color[xSize, zSize];

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                globalHM[x,z] = save.savedHM[x,z];
                globalColours[x,z] = save.savedColours[x,z].ToColour();
            }
        }

        //Update
        GenerateMeshes(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
        RegenerateCollisions();
        GenerateTextures(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
    }


    /*------------------Alter part of the terrain.---------------------------------------------------------------------- */

    public void AlterHM(float[,] alteration, Vector2Int start)
    {
        int size = alteration.GetLength(0);
        //Precondition: alteration is a square!
        if (size != alteration.GetLength(1))
            Debug.LogError("Please only make square alterations to heightmap!");

        //Make sure alteration does not go off the heightmap!
        if (start.x < 0 || start.x + size >= globalHM.GetLength(0) || start.y < 0 || start.y + size >= globalHM.GetLength(1))
            return;

        //Edit terrain.
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                globalHM[x + start.x, z + start.y] += alteration[x, z] * Time.deltaTime;
                globalHM[x + start.x, z + start.y] = Mathf.Max(globalHM[x + start.x, z + start.y], 0);
            }

        }

        Vector2Int[] affectedChunks = FindChunkIndices(start, size);
        GenerateMeshes(affectedChunks[0], affectedChunks[1]);
        //Remember to regenerate collisions once you're done altering!
        //This doesn't happen everytime the heightmap is altered for performance reasons.
    }

    public void FlattenHM(float[,] intensity, Vector2Int start)
    {
        int size = intensity.GetLength(0);
        //Precondition: intensity is a square!
        if (size != intensity.GetLength(1))
            return;

        //Can't flatten parts of the HM that don't exist.
        if (start.x < 0 || start.x + size >= globalHM.GetLength(0) || start.y < 0 || start.y + size >= globalHM.GetLength(1))
            return;

        //Find minimum height value.
        float minHeight = float.MaxValue;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                if (minHeight > globalHM[x + start.x, z + start.y])
                    minHeight = globalHM[x + start.x, z + start.y];
            }
        }

        //Edit heightmap.
         for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                globalHM[x + start.x, z + start.y] = Mathf.Lerp(globalHM[x + start.x, z + start.y], minHeight, intensity[x,z] * Time.deltaTime);
            }
        }

        Vector2Int[] affectedChunks = FindChunkIndices(start, size);
        GenerateMeshes(affectedChunks[0], affectedChunks[1]);
        //Remember to regenerate collisions once you're done altering!
        //This doesn't happen everytime the heightmap is altered for performance reasons.
    }

    public void AlterColours(Color newColour, Vector2Int start, int size)
    {
        //Can't alter colours that are outside of the terrain.
        if (start.x < 0 || start.x + size >= globalColours.GetLength(0) || start.y < 0 || start.y + size >= globalColours.GetLength(1))
            return;

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
                //if you want to change this, you could lerp between the original colour and new one instead based on 
                //some parameter.
                //This would have the effect of allowing lower intensity brushes to only slightly affect colour.
                globalColours[x + start.x, z + start.y] = newColour;
        }

        Vector2Int[] affectedChunks = FindChunkIndices(start, size);
        GenerateTextures(affectedChunks[0], affectedChunks[1]);
    }

    //Takes in a square and tells you two things:
    //[0] = The chunk the square starts in.
    //[1] = the chunk the square ends in.
    //All other chunks can be inferred from this and the fact that it's a square, if necessary.
    Vector2Int[] FindChunkIndices (Vector2Int squareStart, int squareSize)
    {
        Vector2Int[] indices = new Vector2Int[2];


        indices[0] = new Vector2Int((squareStart.x) / (ChunkSize - 1), (squareStart.y) / (ChunkSize - 1));
        //If the start is on a place where two chunks overlap, the lower chunk is where it actually starts.
        /*
                246     247     248
        -------------------
                        -------------
        Start:   0       0       1
        End:     0       1       1
        */
        if (squareStart.x % (ChunkSize - 1) == 0 && squareStart.x != 0)
            indices[0].x -= 1;
        if (squareStart.y % (ChunkSize - 1) == 0 && squareStart.y != 0)
            indices[0].y -= 1;

        indices[1] = new Vector2Int((squareStart.x + squareSize - 1) / (ChunkSize - 1), (squareStart.y + squareSize - 1) / (ChunkSize - 1));

        return indices;
    }

    //Called when you try to change ChunkSize in the Unity Editor.
    void OnValidate()
    {
        //Enforce ChunkSize being a multiple of 8.
        ChunkSize -= ChunkSize % 8;
    }
}