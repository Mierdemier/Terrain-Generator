using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brush
{
    public abstract void Apply(ChunkSystem chunkSystem, RaycastHit hitData, int radius, int power, Color colour, bool isCircle);
}


public class BrushTerrain : Brush
{
    public override void Apply(ChunkSystem chunkSystem, RaycastHit hitData, int radius, int power, Color colour, bool isCircle){
        Vector3 hitPosition = hitData.point;
        int xCoor = (int) hitPosition.x;
        int yCoor = (int) hitPosition.z;

        //Get heightmap.
        float[,] map = chunkSystem.getHeightMap();
        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);

        //Edit terrain in heightmap.
        int threshold = radius * radius;
        int xSquared = 1;
        int ySquared = 1;
        for(int x = xCoor-radius; x <= xCoor+radius; x++) {
            if(x>=0 && x<xSize) { //Don't go out-of-bounds.
                xSquared = (x - xCoor) * (x - xCoor);
                for(int y = yCoor-radius; y <= yCoor+radius; y++) {
                    if(y>=0 && y<ySize) { //Don't go out-of-bounds.
                        if(!isCircle) { //Draw square
                            map[x,y] += power;
                        } else { //Draw circle
                            ySquared = (y - yCoor) * (y - yCoor);
                            if(xSquared+ySquared<=threshold) {
                                map[x,y] += power;
                            }
                        }
                    }
                }
            }
        }

        //Generate updated terrain.
        chunkSystem.setHeightMap(map);
        Vector2Int chunkIndexes = chunkSystem.FindChunkIndexes(new Vector2Int(xCoor, yCoor));
        for(int i = chunkIndexes.x -1; i <= chunkIndexes.x +1; i++) { //Update all chunks around clicked chunk (they may also be affected).
            for(int j = chunkIndexes.y -1; j <= chunkIndexes.y +1; j++) {
                Vector2Int currChunkIndexes = new Vector2Int(i, j);
                if(chunkSystem.IsValidChunkIndex(currChunkIndexes)) { //Check it isn't out-of-bounds.
                    chunkSystem.GenerateChunkMesh(currChunkIndexes);
                }
            }
        }
    }
}


public class BrushColour : Brush
{
    //NOTE: In the version I'm working on now it only updates one chunk!
    public override void Apply(ChunkSystem chunkSystem, RaycastHit hitData, int radius, int power, Color colour, bool isCircle){
        Vector3 hitPosition = hitData.point;
        int xCoor = (int) hitPosition.x;
        int yCoor = (int) hitPosition.z;

        //Get colourmap.
        Vector2Int chunkIndexes = chunkSystem.FindChunkIndexes(new Vector2Int(xCoor, yCoor));
        TerrainBuilder chunk = chunkSystem.FindChunk(chunkIndexes);
        Color[] colourMap = chunk.GetComponent<ColourMap>().getColours();
        int size = chunkSystem.ChunkSize;

        //Works only for one chunk!!
        int xMin = chunkIndexes.x * size;
        int xMax = xMin + size;
        int yMin = chunkIndexes.y * size;
        int yMax = yMin + size;

        //Edit colourMap.
        int threshold = radius * radius;
        int xSquared = 1;
        int ySquared = 1;
        for(int x = xCoor-radius; x <= xCoor+radius; x++) {
            if(x>=xMin && x<xMax) { //Don't go out-of-bounds.
                xSquared = (x - xCoor) * (x - xCoor);
                for(int y = yCoor-radius; y <= yCoor+radius; y++) {
                    if(y>=yMin && y<yMax) { //Don't go out-of-bounds.
                        int xElement = x % size;
                        int yElement = y % size;

                        if(!isCircle) { //Draw square
                            colourMap[xElement + yElement*size] = colour;
                        } else { //Draw circle
                            ySquared = (y - yCoor) * (y - yCoor);
                            if(xSquared+ySquared<=threshold) {
                                colourMap[xElement + yElement*size] = colour;
                            }
                        }
                    }
                }
            }
        }

        //Generate updated terrain colour.
        chunk.GetComponent<ColourMap>().TextureFromColourMap(colourMap, size, size);
    }
}