using UnityEngine;
using System;

[System.Serializable]
public class Save
{
    //Unity Colours are not serializable.
    [System.Serializable]
    public struct ColourState
    {
        public float r;
        public float g;
        public float b;

        public ColourState(Color colour)
        {
            r = colour.r;
            g = colour.g;
            b = colour.b;
        }

        public Color ToColour()
        {
            return new Color(r, g, b, 1);
        }

    }
    public int xChunks;
    public int zChunks;

    //Stores state of terrain itself.
    public float[,] savedHM;
    public ColourState[,] savedColours;

    //Stores state of water.
    public float seaLevel;
    public ColourState shallowColour;
    public ColourState deepColour;
    public float depthBlend;

    //Stores skybox.
    public int skyIndex;

    public Save(int numX, int numZ, float[,] HM, Color[,] CM, float waterLevel, Color shallow, Color deep, float depth, int sky)
    {
        xChunks = numX;
        zChunks = numZ;

        savedHM = new float[HM.GetLength(0), HM.GetLength(1)];
        Array.Copy(HM, savedHM, HM.GetLength(0) * HM.GetLength(1));
        savedColours = new ColourState[CM.GetLength(0), CM.GetLength(1)];
        for (int x = 0; x < CM.GetLength(0); x++)
        {
            for (int z = 0; z < CM.GetLength(1); z++)
                savedColours[x,z] = new ColourState(CM[x,z]);
        }

        seaLevel = waterLevel;
        shallowColour = new ColourState(shallow);
        deepColour = new ColourState(deep);
        depthBlend = depth;

        skyIndex = sky;
    }
}
