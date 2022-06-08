using UnityEngine;
using System;

[RequireComponent(typeof(ProceduralGenerator))]
//The ChunkSystem is the central class that stores the state of the entire terrain as a whole.
//  In Start(), it prepares the terrain by creating the correct amount of chunks, and Generating From Scratch.
//  GenerateFromScratch() is currently the only public method. It fills the chunks with procedural terrain.
//                        This overwrites the terrain that's currently there!
public class ChunkSystem : MonoBehaviour
{

    [Range(8, 248)]
    //The size of each (square) chunk. Must be a multiple of 8!
    public int ChunkSize = 200;
    public Vector2Int numChunks;
    [SerializeField]
    GameObject ChunkPrefab;
    [SerializeField]
    CameraScript Camera;


    ProceduralGenerator generator;
    float[,] globalHM;  //Global heightmap, don't lose this!
    Color[,] globalColours; //Global colour map.

    TerrainBuilder[,] chunks; //Contains references to every chunk.

    void Start()
    {
        //Find generator.
        this.generator = GetComponent<ProceduralGenerator>();

        //Spawn terrain chunks
        this.chunks = new TerrainBuilder[numChunks.x , numChunks.y];
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

        //Render the heightmap using the chunks you made.
        GenerateFromScratch();
    }

    //END IS INCLUSIVE!!!
    public void GenerateMeshes(Vector2Int start, Vector2Int end)
    {
        //Precondition
        if (start.x < 0 || start.y < 0 || end.x < start.x || end.y < start.y || end.x > numChunks.x || end.y > numChunks.y)
            return;

        //Give terrain chunks the correct section of the heightmap to render.
        for(int x = start.x; x <= end.x; x++)
        {
            for (int z = start.y; z <= end.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int chunkStart = new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1));
                chunks[x,z].GenerateMesh(globalHM, chunkStart, ChunkSize);
            }
        }
    }

    public void GenerateTextures(Vector2Int start, Vector2Int end)
    {

        //Precondition
        if (start.x < 0 || start.y < 0 || end.x < start.x || end.y < start.y || end.x > numChunks.x || end.y > numChunks.y)
            Debug.LogError("Can only generate meshes for chunks that exist!");

        //Give terrain chunks each a section of the colours to render.
        for(int x = 0; x <= end.x; x++)
        {
            for (int z = 0; z <= end.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int chunkStart = new Vector2Int(x * ChunkSize , z * ChunkSize);
                chunks[x,z].GetComponent<ColourMap>().TextureFromColourMap(globalColours, chunkStart, ChunkSize);
            }
        }
    }

    public void GenerateFromScratch()
    {
        globalHM = generator.HeightMap(ChunkSize * numChunks.x, ChunkSize * numChunks.y);
        globalColours = generator.AddColour(globalHM);

        GenerateMeshes(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
        GenerateTextures(Vector2Int.zero, numChunks - new Vector2Int(1, 1));

        //Set camera zoom
        Camera.setZoom(numChunks.x * (-100f), generator.Scale * (-2f));
    }

    public void GenerateFromMap(Texture2D map)
    {

        globalColours = generator.AddColour(globalHM);
        int xSize = globalHM.GetLength(0);
        int zSize = globalHM.GetLength(1);

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
                globalHM[x,z] = map.GetPixel((x * map.width) / xSize, (z * map.height) / zSize).grayscale * 50;
        }

        GenerateMeshes(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
        GenerateTextures(Vector2Int.zero, numChunks - new Vector2Int(1, 1));
    }

    public void AlterHM(float[,] alteration, Vector2Int start)
    {
        int size = alteration.GetLength(0);
        //Precondition: alteration is a square!
        if (size != alteration.GetLength(1))
            Debug.LogError("Please only make square alterations to heightmap!");

        //Make sure alteration does not go off the heightmap!
        if (start.x < 0)
        {
            size += start.x;
            start.x = 0; 
        }
        if (start.y < 0)
        {
            size += start.y;
            start.y = 0;
        }
        if (start.x + size >= globalHM.GetLength(0))
            size = globalHM.GetLength(0) - start.x;
        if (start.y + size >= globalHM.GetLength(1))
            size = globalHM.GetLength(1) - start.y;
        

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                globalHM[x + start.x, z + start.y] += alteration[x, z];
                globalHM[x + start.x, z + start.y] = Mathf.Max(globalHM[x + start.x, z + start.y], 0);
            }

        }

        Vector2Int[] afffectedChunks = FindChunkIndices(start, size);
        GenerateMeshes(afffectedChunks[0], afffectedChunks[1]);
    }

    public void FlattenHM(float[,] intensity, Vector2Int start)
    {
        int size = intensity.GetLength(0);
        //Precondition: intensity is a square!
        if (size != intensity.GetLength(1))
            Debug.LogError("Please only make square alterations to heightmap!");

        //Make sure alteration does not go off the heightmap!
        if (start.x + size >= globalHM.GetLength(0))
            size = globalHM.GetLength(0) - start.x;
        if (start.y + size >= globalHM.GetLength(1))
            size = globalHM.GetLength(1) - start.y;

        //Find medium height value.
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
                globalHM[x + start.x, z + start.y] = Mathf.Lerp(globalHM[x + start.x, z + start.y], minHeight, intensity[x,z]);
            }
        }

        Vector2Int[] afffectedChunks = FindChunkIndices(start, size);
        GenerateMeshes(afffectedChunks[0], afffectedChunks[1]);
    }

    public void AlterColours(Color newColour, Vector2Int start, int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
                //Optionally, you could lerp between the original colour and new one instead based on some parameter.
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
    Vector2Int[] FindChunkIndices (Vector2Int squareStart, int squareSize){
        Vector2Int[] indices = new Vector2Int[2];

        indices[0] = new Vector2Int(squareStart.x / (ChunkSize - 1), squareStart.y / (ChunkSize - 1));
        //If the start is on a place where two chunks overlap, the lower chunk is where it actually starts.
        /*
                246     247     248
        -------------------
                        -------------
        Start:   0       0       1
        End:     0       1       1
        */
        if (squareStart.x % (ChunkSize - 1) == 0)
            indices[0].x -= 1;
        if (squareStart.y % (ChunkSize - 1) == 0)
            indices[0].y -= 1;

        indices[1] = new Vector2Int((squareStart.x + squareSize) / (ChunkSize - 1), (squareStart.y + squareSize) / (ChunkSize - 1));

        return indices;
    }

        void OnValidate()
    {
        //Enforce ChunkSize being a multiple of 8.
        ChunkSize -= ChunkSize % 8;
    }
}