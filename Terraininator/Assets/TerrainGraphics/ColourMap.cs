using UnityEngine;

//The ColourMap class contains the methods and data necessary to create a texture for a terrain chunk.
//It has two public methods:
//	AddColour() procedurally creates an array of colours that we think fit well with the heightmap.
//		(e.g. snow for mountain tops, yellow-ish for beaches, etc.)
//		The plan is that the user will be able to alter the procedural colouring in a menu at some point!
//
//	TextureFromColourMap() creates a texture from an array of colours and assigns it to the terrain chunk.
//		This array of colours *could* be from AddColour(), or it could be something the user manually drew.
public class ColourMap : MonoBehaviour
{
	[SerializeField]
    TerrainType[] biomes; //The terrain types used for procedural colours.

	[SerializeField]
	MeshRenderer meshRenderer;
	[SerializeField]
	ComputeShader colourCalculator;

	private Color[] colours;

	//Returns an array of colours fitted to this subsection of the heightmap.
	//	It is not performant to calculate this on the CPU (takes about 30 seconds per chunk on my laptop)
	//	So we will use this method to dispatch a ComputeShader instead.
    public Color[] AddColour(float[,] heightMap, Vector2Int start, int size)
	{
		//Create shared memory with GPU.
		ComputeBuffer heightBuffer = new ComputeBuffer(size * size, sizeof(float));
		ComputeBuffer biomeHeightBuffer = new ComputeBuffer(biomes.Length, sizeof(float));
		ComputeBuffer biomeColourBuffer = new ComputeBuffer(biomes.Length, 4 * sizeof(float));
		ComputeBuffer colourBuffer = new ComputeBuffer(size * size, 4 * sizeof(float));

		//Turn everything into arrays of floats, since the GPU doesn't know what TerrainTypes or float[,] are.
		float[] biomeHeights = new float[biomes.Length];
		Color[] biomeColours = new Color[biomes.Length];
		for (int i = 0; i < biomes.Length; i++)
		{
			biomeHeights[i] = biomes[i].height;
			biomeColours[i] = biomes[i].colour;
		}
		float[] localHM = new float[size * size];
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
				localHM[y * size + x] = heightMap[start.x + x, start.y + y];
		}

		//Give GPU data through shared memory.
		heightBuffer.SetData(localHM);
		biomeHeightBuffer.SetData(biomeHeights);
		biomeColourBuffer.SetData(biomeColours);
		colourCalculator.SetBuffer(0, "heightmap", heightBuffer);
		colourCalculator.SetBuffer(0, "colours", colourBuffer);
		colourCalculator.SetBuffer(0, "biomeHeights", biomeHeightBuffer);
		colourCalculator.SetBuffer(0, "biomeColours", biomeColourBuffer);
		colourCalculator.SetInt("size", size);
		colourCalculator.SetInts("start", start.x, start.y);
		colourCalculator.SetInt("numBiomes", biomes.Length);

		//Run Compute Shader.
		colourCalculator.Dispatch(0, size / 8, size / 8, 1);
		this.colours = new Color[size * size];
		colourBuffer.GetData(this.colours);

		//Dispose of shared memory.
		heightBuffer.Dispose();
		colourBuffer.Dispose();
		biomeHeightBuffer.Dispose();
		biomeColourBuffer.Dispose();

		return this.colours;
    }

    public void TextureFromColourMap(Color[] colourMap, int width, int height) 
	{
		this.colours = colourMap;
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Bilinear; //Makes texture looks less blocky.
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colourMap);
		texture.Apply();
		
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_mainTexture", texture);
		meshRenderer.material.SetFloat("_blurRadius", 1f / (float)(height));
	}

	public Color[] getColours() {return this.colours; }
}

[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;
    
    public TerrainType(string name2, float height2, Color colour2) 
	{
		name = name2;
        height = height2;
        colour = colour2;
    }
}