using UnityEngine;

public class ColourMap : MonoBehaviour
{
	[SerializeField]
    TerrainType[] biomes;

	[SerializeField]
	MeshRenderer meshRenderer;
	[SerializeField]
	ComputeShader colourCalculator;

    public Color[] AddColour(float[,] heightMap, Vector2Int start, int size, float scale)
	{
		//Create shared memory with GPU.
		ComputeBuffer heightBuffer = new ComputeBuffer(size * size, sizeof(float));
		ComputeBuffer biomeHeightBuffer = new ComputeBuffer(biomes.Length, sizeof(float));
		ComputeBuffer biomeColourBuffer = new ComputeBuffer(biomes.Length, 4 * sizeof(float));
		ComputeBuffer colourBuffer = new ComputeBuffer(size * size, 4 * sizeof(float));

		//Turn everything into arrays of floats, since the GPU doesn't know what TerrainTypes float[,] are.
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
		Color[] colours = new Color[size * size];
		colourBuffer.GetData(colours);

		//Dispose of shared memory.
		heightBuffer.Dispose();
		colourBuffer.Dispose();
		biomeHeightBuffer.Dispose();
		biomeColourBuffer.Dispose();

		return colours;
    }

    public void TextureFromColourMap(Color[] colourMap, int width, int height) 
	{
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Bilinear;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colourMap);
		texture.Apply();
		
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_mainTexture", texture);
		meshRenderer.material.SetFloat("_blurRadius", 1f / (float)(height));
	}
}

[System.Serializable]
public struct TerrainType
{
	public float height;
	public Color colour;
    
    public TerrainType(float height2, Color colour2) 
	{
        height = height2;
        colour = colour2;
    }
}