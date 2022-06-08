using UnityEngine;

//Brush is an abstract class that keeps track of brush settings and has a method that can be called
//to apply the brush to the chunk system.
public abstract class Brush : MonoBehaviour
{
    protected int radius = 5;
    protected float intensity = 0f;
    protected Color colour = Color.magenta;

    public abstract void Apply(ChunkSystem chunkSystem, RaycastHit hitData, float timeApplied);

    public int GetRadius() {return radius;}
}

/*

public class BrushFlatten : Brush
{
    public override void Apply(ChunkSystem chunkSystem, RaycastHit hitData, int radius, int power, Color colour, bool isCircle){
        Vector3 hitPosition = hitData.point;
        int xCoor = (int) hitPosition.x;
        int yCoor = (int) hitPosition.z;

        //Get heightmap.
        float[,] map = chunkSystem.getHeightMap();
        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);

        //Needed to make certain all affected chunks are updated.
        Vector2Int chunkIndexes = chunkSystem.FindChunkIndexes(new Vector2Int(xCoor, yCoor));
        if(!chunkSystem.IsValidChunkIndex(chunkIndexes)) { return; }//If this isn't a valid chunk, just end the function here.
        int size = chunkSystem.ChunkSize;
        bool multipleChunks = false; //Are several chunks affected?

        //Edit terrain in heightmap.
        int threshold = radius * radius; //Needed to draw circle.
        int xSquared = 1; //Needed to draw circle.
        int ySquared = 1; //Needed to draw circle.

        bool raiseTerrain = power > 0; //Decides whether the brush should raise terrain or lower it.
        int maxOrMin = 4; //Random value.
        if(raiseTerrain) {maxOrMin = 10000;} else {maxOrMin = 0;}

        //Find maximum or minimum height.
        for(int x = xCoor-radius; x <= xCoor+radius; x++) {
            if(x>=0 && x<xSize) { //Don't go out-of-bounds.
                if(x<chunkIndexes.x*size || x>=chunkIndexes.x*size+size) { multipleChunks = true; } //More than one chunk is affected.
                xSquared = (x - xCoor) * (x - xCoor); //Needed to draw circle.
                for(int y = yCoor-radius; y <= yCoor+radius; y++) {
                    if(y>=0 && y<ySize) { //Don't go out-of-bounds.
                        if(y<chunkIndexes.y*size || y>=chunkIndexes.y*size+size) { multipleChunks = true; } //More than one chunk is affected.
                        if(!isCircle) { //Measure in square.
                            if(raiseTerrain) { //Find lowest point to increase.
                                maxOrMin = (int)Mathf.Min(map[x,y], maxOrMin);
                            } else { //Or find highest point to decrease.
                                maxOrMin = (int)Mathf.Max(map[x,y], maxOrMin);
                            }
                        } else { //Measure in circle.
                            ySquared = (y - yCoor) * (y - yCoor); //Needed to draw circle.
                            if(xSquared+ySquared<=threshold) { //Makes certain a circle is drawn.
                                if(raiseTerrain) { //Find lowest point to increase.
                                    maxOrMin = (int)Mathf.Min(map[x,y], maxOrMin);
                                } else { //Or find highest point to decrease.
                                    maxOrMin = (int)Mathf.Max(map[x,y], maxOrMin);
                                }
                            }
                        }
                    }
                }
            }
        }
        //With maxOrMin set to the proper value, alter heightmap.
        int leveledHeight = maxOrMin+power;
        for(int x = xCoor-radius; x <= xCoor+radius; x++) {
            if(x>=0 && x<xSize) { //Don't go out-of-bounds.
                if(x<chunkIndexes.x*size || x>=chunkIndexes.x*size+size) { multipleChunks = true; } //More than one chunk is affected.
                xSquared = (x - xCoor) * (x - xCoor); //Needed to draw circle.
                for(int y = yCoor-radius; y <= yCoor+radius; y++) {
                    if(y>=0 && y<ySize) { //Don't go out-of-bounds.
                        if(y<chunkIndexes.y*size || y>=chunkIndexes.y*size+size) { multipleChunks = true; } //More than one chunk is affected.
                        if(!isCircle) { //Draw square.
                            if(raiseTerrain) { //Raise lowest terrain.
                                if(map[x,y]<leveledHeight) {map[x,y]=leveledHeight;}
                            } else { //Lower heighest terrain.
                                if(map[x,y]>leveledHeight) {map[x,y]=leveledHeight;}
                            }
                        } else { //Draw circle
                            ySquared = (y - yCoor) * (y - yCoor); //Needed to draw circle.
                            if(xSquared+ySquared<=threshold) { //Makes certain a circle is drawn.
                                if(raiseTerrain) { //Raise lowest terrain.
                                    if(map[x,y]<leveledHeight) {map[x,y]=leveledHeight;}
                                } else { //Lower heighest terrain.
                                    if(map[x,y]>leveledHeight) {map[x,y]=leveledHeight;}
                                }
                            }
                        }
                    }
                }
            }
        }
        chunkSystem.setHeightMap(map);

        //Generate updated terrain.
        if(multipleChunks) { //Update several chunks.
            for(int i = chunkIndexes.x -1; i <= chunkIndexes.x +1; i++) { //Update all chunks around clicked chunk (they may also be affected).
                for(int j = chunkIndexes.y -1; j <= chunkIndexes.y +1; j++) {
                    Vector2Int currChunkIndexes = new Vector2Int(i, j);
                    if(chunkSystem.IsValidChunkIndex(currChunkIndexes)) { //Check it isn't out-of-bounds.
                        chunkSystem.GenerateChunkMesh(currChunkIndexes);
                    }
                }
            }
        } else { //Update only one chunk.
            chunkSystem.GenerateChunkMesh(chunkIndexes);
        }
    }

}
*/