using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField]
    ChunkSystem chunkSystem;
    

    public void Apply(RaycastHit hitData){
        Vector3 hitPosition = hitData.point;
        float[,] map = chunkSystem.getHeightMap();

        //Get the heightmap, up terrain, and generate the terrain again.
        // chuckSystem.GenerateFromMap(map);
        //"map" is a reference to memory, right?
    }
}