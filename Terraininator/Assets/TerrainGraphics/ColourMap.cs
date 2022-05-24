using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourMap : MonoBehaviour
{
	public float scaleStored = 50f;
	float[,] heightMapStored;
    public TerrainType[] biomes = new TerrainType[]
    {
        new TerrainType("water deep", 0.15f, Color.blue),
        new TerrainType("water shallow", 0.2f, Color.cyan),
        new TerrainType("sand", 0.25f, Color.yellow),
        new TerrainType("grass", 0.35f, Color.green),
        new TerrainType("rock", 0.90f, Color.grey),
        new TerrainType("snow", 1f, Color.white)
    };

	public MeshRenderer meshRenderer;

    public Color[] AddColour(float[,] heightMap, float scale) {
		scaleStored = scale;
		heightMapStored = heightMap;
        int xSize = heightMap.GetLength(0);
        int zSize = heightMap.GetLength(1);

        Color[] colourMap = new Color[xSize * zSize];
		for (int z = 0; z < zSize; z++) {
			for (int x = 0; x < xSize; x++) {
				float currentHeight = heightMap[x, z];
				for (int i = 0; i < biomes.Length; i++) {
					if (currentHeight <= biomes[i].height * scale) {
						colourMap[z * xSize + x] = biomes[i].colour;
						break;
					}
				}
			}
		}

		return colourMap;
    }

    public void TextureFromColourMap(Color[] colourMap, int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(colourMap);
		texture.Apply();
		
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.mainTexture = texture;
	}

	void OnValidate() {
		//Set the colours of the map.
        Color[] colourMap = AddColour(heightMapStored, scaleStored);
        TextureFromColourMap(colourMap, heightMapStored.GetLength(0), heightMapStored.GetLength(1));
	}
}

[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;
    
    public TerrainType(string name2, float height2, Color colour2) {
        name = name2;
        height = height2;
        colour = colour2;
    }
}