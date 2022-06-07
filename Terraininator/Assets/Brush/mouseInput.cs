using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunkSystem;

    Brush brush;
    int radius;
    int power;
    Color colour;
    public bool isCircle;

    void Start() {
        brush = new BrushFlatten();
        radius = 20;
        power = -5;
        isCircle = true;
        colour = Color.magenta;
    }

    // Update is called once per frame
    void Update()
    {
        //Left mouse button is clicked.
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData)){
                Vector3 hitPosition = hitData.point;

                //Do something
                brush.Apply(chunkSystem, hitData, this.radius, this.power, this.colour, this.isCircle);
            }
        }
    }
}