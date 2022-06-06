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
        generator = GetComponent<ProceduralGenerator>();

        //Spawn terrain chunks
        chunks = new TerrainBuilder[numChunks.x , numChunks.y];
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

    public void GenerateFromScratch()
    {
        DateTime time = DateTime.Now;

        //Initialise heightmap with procedural terrain.
        globalHM = generator.HeightMap(ChunkSize * numChunks.x, ChunkSize * numChunks.y);
        //Colour in the heightmap.
        globalColours = generator.AddColour(globalHM);

        //Give terrain chunks the correct section of the heightmap to render.
        for(int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                //Build a mesh for the chunk.
                Vector2Int start = new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1));
                chunks[x,z].GenerateMesh(globalHM, start, ChunkSize);
                
                //Add procedural colours.
                start += new Vector2Int(x, z);
                chunks[x,z].GetComponent<ColourMap>().TextureFromColourMap(globalColours, start, ChunkSize);
            }
        }

        //Set camera zoom
        Camera.setZoom(numChunks.x * (-100f), generator.Scale * (-2f));

        Debug.Log("Completed in: " + (time - DateTime.Now).ToString());
    }

    void OnValidate()
    {
        //Enforce ChunkSize being a multiple of 8.
        ChunkSize -= ChunkSize % 8;
    }

    //TODO
    //Functions that will help with altering parts of the heightmap (say, using a brush):

    //FindChunks(), a function that returns the chunks in a certain area,

    //AlterHM(), a function that changes the globalHM and updates the chunks in the right area.
    //(uses FindChunks to find which chunks)
}
