using UnityEngine;

//The ColourMap class should be attached to a chunk of terrain. 
//It's purpose is to give the terrain a texture:
//	TextureFromColourMap() creates a texture from an array of colours and assigns it to the terrain chunk.
//		This array of colours *could* be from ProceduralGenerator.AddColour(), 
//			or it could be something the user manually drew.
public class ColourMap : MonoBehaviour
{
	[SerializeField]
    static TerrainType[] biomes; //The terrain types used for procedural colours.

	[SerializeField]
	MeshRenderer meshRenderer;
	[SerializeField]
	static ComputeShader colourCalculator;

    public void TextureFromColourMap(Color[,] colourMap, Vector2Int start, int size) 
	{
		Color[] localColours = new Color[size * size];
		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
				localColours[z * size + x] = colourMap[x + start.x, z + start.y];
		}

		Texture2D texture = new Texture2D (size, size);
		texture.filterMode = FilterMode.Bilinear; //Makes texture looks less blocky.
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels(localColours);
		texture.Apply();
		
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_mainTexture", texture);
		meshRenderer.material.SetFloat("_blurRadius", 1f / (float)(size));
	}
}