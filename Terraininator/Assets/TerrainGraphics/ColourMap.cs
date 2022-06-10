using UnityEngine;

//The ColourMap class should be attached to a chunk of terrain. 
//It's purpose is to give the terrain a texture:
//	TextureFromColourMap() creates a texture from an array of colours and assigns it to the terrain chunk.
//		This array of colours *could* be from ProceduralGenerator.AddColour(), 
//			or it could be something the user manually drew.
[RequireComponent(typeof(MeshRenderer))]
public class ColourMap : MonoBehaviour
{
	MeshRenderer meshRenderer;
	Texture2D storedTexture; //Dont allocate an entire new texture every time.
	Color[] localColours;
	bool alreadyHasColours = false;

    public void TextureFromColourMap(Color[,] colourMap, Vector2Int start, int size) 
	{
		//Create a copy of just the part of the colour array you need.
		//texture.SetPixels() is a built-in Unity method, so we cannot change it to take in a start + size :(
		if(!alreadyHasColours)
		{
			//ColourMap size doesn't change at runtime. Size parameter is purely a development convenience.
			localColours = new Color[size * size];
			storedTexture = new Texture2D(size, size);
			storedTexture.filterMode = FilterMode.Bilinear; //Makes texture looks less blocky.
			storedTexture.wrapMode = TextureWrapMode.Clamp;

			alreadyHasColours = true;
		}

		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
				localColours[z * size + x] = colourMap[x + start.x, z + start.y];
		}

		//Create texture from colour map.
		storedTexture.SetPixels(localColours);
		storedTexture.Apply();
		
		//Apply texture to mesh.
		meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.SetTexture("_mainTexture", storedTexture);
		meshRenderer.material.SetFloat("_blurRadius", 1f / (float)(size));
	}
}