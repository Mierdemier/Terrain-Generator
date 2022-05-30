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
		ComputeBuffer heightBuffer = new ComputeBuffer(heightMap.GetLength(0) * heightMap.GetLength(1), sizeof(float));
		ComputeBuffer biomeBuffer = new ComputeBuffer(biomes.Length, 5 * sizeof(float));
		ComputeBuffer colourBuffer = new ComputeBuffer(size * size, 4 * sizeof(float));

		heightBuffer.SetData(heightMap);

		colourCalculator.SetBuffer(0, "heightmap", heightBuffer);
		colourCalculator.SetBuffer(0, "biomes", biomeBuffer);
		colourCalculator.SetBuffer(0, "colours", colourBuffer);
		colourCalculator.SetInt("size", size);
		colourCalculator.SetInts("start", start.x, start.y);
		colourCalculator.SetInt("numBiomes", biomes.Length);

		colourCalculator.Dispatch(0, size / 8, size / 8, 1);

		Color[] colours = new Color[size * size];
		colourBuffer.GetData(colours);
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