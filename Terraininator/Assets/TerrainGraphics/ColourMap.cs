using UnityEngine;

public class ColourMap : MonoBehaviour
{
	[SerializeField]
    TerrainType[] biomes = new TerrainType[]
    {
        new TerrainType("water deep", 0.15f, Color.blue),
        new TerrainType("water shallow", 0.2f, Color.cyan),
        new TerrainType("sand", 0.25f, Color.yellow),
        new TerrainType("grass", 0.35f, Color.green),
        new TerrainType("rock", 0.90f, Color.grey),
        new TerrainType("snow", 1f, Color.white)
    };

	[SerializeField]
	MeshRenderer meshRenderer;

    public Color[] AddColour(float[,] heightMap, Vector2Int start, int size, float scale)
	{
        Color[] colourMap = new Color[size * size];
		for (int z = 0; z < size; z++) 
		{
			for (int x = 0; x < size; x++) 
			{
				float currentHeight = heightMap[x + start.x, z + start.y];
				for (int i = 0; i < biomes.Length; i++) 
				{
					if (currentHeight <= biomes[i].height * scale) 
					{
						colourMap[z * size + x] = biomes[i].colour;
						break;
					}
				}
			}
		}

		return colourMap;
    }

    public void TextureFromColourMap(Color[] colourMap, int width, int height) 
	{
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colourMap);
		texture.Apply();
		
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.mainTexture = texture;
	}
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