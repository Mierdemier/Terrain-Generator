using UnityEngine;

/*
    This class build a mesh for the terrain and displays it using the object's mesh filter.
    In the start function we get the mesh filter.

    In the GenerateMesh function we generate a complete mesh from scratch and assign it to the filter.
        As input it takes a 2-dimensional float array, which represent the heights of each point on the terrain.
        GenerateMesh() should be called by another script.
    AddTriangle() is just a utility function that should probably not be fucked with outside of this class.
*/
[RequireComponent(typeof(MeshFilter))]
public class TerrainBuilder : MonoBehaviour
{
    [SerializeField]
    MeshFilter filter; //Built-in Unity component that (helps) render the mesh.
    Vector3[] verts; //Vertices, the points of the model, stored as 3D coordinates.
    int[] tris; //Triangles, stored as collections of 3 array indices of vertices that they connect.
                //Obviously the length of this array must be a factor of 3, since a triangle has 3 points.
    Vector2[] uvs; //UV-coordinates are used to map textures to a mesh to colour it in.
    Mesh mesh;  //The data of the complete model.

    void Start()
    {
        filter = GetComponent<MeshFilter>();
    }

    //Generates a subsection of a heightmap. 
    //(This is more performant than creating a new heightmap from that subsection)
    public void GenerateMesh (float[,] heightmap, Vector2Int start, int size)
    {
        //Set up basic variables.
        mesh = new Mesh();
        verts = new Vector3[size * size];
        tris = new int[(size - 1) * (size - 1) * 6];

        //Create the shape.
        int v = 0, t = 0; //These numbers keep track of the index of the current vertex/triangle respectively.
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                //For every point on the heightmap, we spawn a new vertex in the terrain model.
                //This vertex has the height of the point on the map.
                verts[v] = new Vector3(x,  heightmap[x + start.x, z + start.y],  z);

                //Add triangles to this vertex to be able to render the terrain as a continuous mesh.
                if (x < size - 1 && z < size - 1) //Verts on the edge of the model should not generate tris!
                                                    //(array would be out of bounds)
                {
                    AddTriangle(v, v + size + 1, v + size, ref t);
                    AddTriangle(v + size + 1, v, v + 1, ref t);
                }

                v++;
            }
        } 

        //Attach uv coordinates to the shape in case it needs colours.
        uvs = new Vector2[verts.Length];
        for (int x = 0, i = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                uvs[i] = new Vector2((float)x / size, (float)z / size);
                i++;
            }
        }

        //Update the mesh with built-in Unity stuff.
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        //And tell the mesh filter about the new mesh.
        filter.mesh = mesh;
    }

    void AddTriangle(int a, int b, int c, ref int t)
    {
        //b\
        //| \
        //a--c (Or any other configuration where a, b, c are in clocwise order) will work.
        tris[t] = a;
        tris[t + 1] = b;
        tris[t + 2] = c;

        t += 3;
    }
}