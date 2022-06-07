using UnityEngine;
using System;

[RequireComponent(typeof(ProceduralGenerator))]
//The ChunkSystem is the central class that stores the state of the entire terrain as a whole.
//  In Start(), it prepares the terrain by creating the correct amount of chunks, and Generating From Scratch.
//  GenerateFromScratch() is currently the only public method. It fills the chunks with procedural terrain.
//                        This overwrites the terrain that's currently there!
public class ChunkSystem : MonoBehaviour
{

    [SerializeField][Range(8, 248)]
    //The size of each (square) chunk. Must be a multiple of 8!
    public int ChunkSize = 200;
    [SerializeField]
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

    public void GenerateMeshes()
    {

        //Give terrain chunks the correct section of the heightmap to render.
        for(int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int start = new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1));
                chunks[x,z].GenerateMesh(globalHM, start, ChunkSize);
            }
        }

        //Set camera zoom
        Camera.setZoom(numChunks.x * (-100f), generator.Scale * (-2f));
    }

    public void GenerateTextures()
    {
        //Create new colour map based on heights.
        globalColours = generator.AddColour(globalHM);

        //Give terrain chunks each a section of the colours to render.
        for(int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int start = new Vector2Int(x * ChunkSize , z * ChunkSize);
                chunks[x,z].GetComponent<ColourMap>().TextureFromColourMap(globalColours, start, ChunkSize);
            }
        }
    }

    public void GenerateFromScratch()
    {
        globalHM = generator.HeightMap(ChunkSize * numChunks.x, ChunkSize * numChunks.y);

        GenerateMeshes();
        GenerateTextures();
    }

    public void GenerateFromMap(Texture2D map)
    {
       int xSize = globalHM.GetLength(0);
       int zSize = globalHM.GetLength(1);

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
                globalHM[x,z] = map.GetPixel((x * map.width) / xSize, (z * map.height) / zSize).grayscale * 50;
        }

        GenerateMeshes();
        GenerateTextures();
    }

    //Build a mesh for a chunk.
    public void GenerateChunkMesh(Vector2Int chunkIndexes) {
        int x = chunkIndexes.x;
        int z = chunkIndexes.y;

        Vector2Int start = new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1));
        chunks[x,z].GenerateMesh(globalHM, start, ChunkSize);
    }

    void OnValidate()
    {
        //Enforce ChunkSize being a multiple of 8.
        ChunkSize -= ChunkSize % 8;
    }

    //Functions that will help with altering parts of the heightmap (say, using a brush):
    public float[,] getHeightMap() {return this.globalHM;}

    public void setHeightMap(float[,] heightMap) {this.globalHM = heightMap;}

    //Returns x and z indexes of a chunk given coordinates.
    public Vector2Int FindChunkIndexes(Vector2Int coors){
        return new Vector2Int(coors.x/ChunkSize, coors.y/ChunkSize);
    }

    //Returns chunk given indexes.
    public TerrainBuilder FindChunk(Vector2Int indexes){
        return chunks[indexes.x, indexes.y];
    }

    public bool IsValidChunkIndex(Vector2Int indexes){
        return (indexes.x >= 0 && indexes.x < numChunks.x) && (indexes.y >= 0 && indexes.y < numChunks.y);
    }

    //TODO
    //FindChunks(), a function that returns the chunks in a certain area,

    //AlterHM(), a function that changes the globalHM and updates the chunks in the right area.
    //(uses FindChunks to find which chunks)
}
