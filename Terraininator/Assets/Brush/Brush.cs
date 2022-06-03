using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private ChunkSystem chunkSystem;
    private GameObject genObject;

    int radius;
    int power;
    public enum DrawMode {Circle, Square};
    public DrawMode mode = DrawMode.Square;

    void Start() {
        radius = 5;
        power = 10;
        genObject = GameObject.Find("GeneratorObject");
        chuckSystem = genObject.GetComponent<ChunkSystem>();
    }
    

    public void Apply(RaycastHit hitData){
        Vector3 hitPosition = hitData.point;
        int xElement = (int) hitPosition.x;
        int yElement = (int) hitPosition.z;

        //Get heightmap.
        float[,] map = chunkSystem.getHeightMap();
        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);

        //Edit terrain in heightmap.
        int threshold = this.radius * this.radius;
        int xSquared = 1;
        int ySquared = 1;
        for(int x = xElement-this.radius; x <= xElement+this.radius; x++) {
            if(x>=0 && x<xSize) { //Don't go out of bounds.
                xSquared = (x - xElement) * (x - xElement);
                for(int y = yElement-this.radius; y <= yElement+this.radius; y++) {
                    if(y>=0 && y<ySize) { //Don't go out of bounds.
                        ySquared = (y - yElement) * (y - yElement);
                        if(mode == DrawMode.Square) {
                            map[x,y] += this.power;
                        } else { //mode == DrawMode.Circle
                            if(xSquared+ySquared<=threshold) {
                                map[x,y] += this.power;
                            }
                        }
                    }
                }
            }
        }

        //Generate updated terrain.
        chuckSystem.GenerateFromMap(map);
    }
}