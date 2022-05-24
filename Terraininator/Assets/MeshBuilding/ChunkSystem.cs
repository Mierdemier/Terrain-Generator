using UnityEngine;

[RequireComponent(typeof(ProceduralGenerator))]
public class ChunkSystem : MonoBehaviour
{
    [SerializeField][Range(1, 255)]
    int ChunkSize = 200;
    [SerializeField]
    Vector2Int numChunks;
    [SerializeField]
    GameObject ChunkPrefab;

    ProceduralGenerator generator;
    float[,] globalHM;  //Global heightmap, don't lose this!
    TerrainBuilder[,] chunks; //Contains references to every chunk.


    void Start()
    {
        //Find generator.
        generator = GetComponent<ProceduralGenerator>();
        
        //Initialise heightmap with procedural terrain.
        globalHM = generator.HeightMap(ChunkSize * numChunks.x, ChunkSize * numChunks.y);

        //Spawn terrain chunks and give them the correct section of the heightmap to render.
        chunks = new TerrainBuilder[numChunks.x , numChunks.y];
        for(int x = 0; x < numChunks.x; x++)
        {
            for (int z = 0; z < numChunks.y; z++)
            {
                chunks[x,z] = Instantiate(ChunkPrefab, 
                transform.position + new Vector3(x * (ChunkSize - 1), 0, z * (ChunkSize - 1)), 
                Quaternion.identity)
                              .GetComponent<TerrainBuilder>();
                
                chunks[x,z].GenerateMesh(globalHM, new Vector2Int(x * (ChunkSize - 1), z * (ChunkSize - 1)), ChunkSize);
            }
        }
    }

    //TODO
    //Functions that will help with altering parts of the heightmap (say, using a brush):

    //FindChunks(), a function that returns the chunks in a certain area,

    //AlterHM(), a function that changes the globalHM and updates the chunks in the right area.
    //(uses FindChunks to find which chunks)
}
