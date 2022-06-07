using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunkSystem;

    Brush brush;
    public int radius;
    public int power;
    public Color colour;
    public bool isCircle;
    private float timer;
    private float wait;

    void Start() {
        brush = new BrushFlatten();
        radius = 20;
        power = -5;
        isCircle = true;
        colour = Color.magenta;
        timer = 0f;
        wait = 1f/3f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= wait) {
            timer = 0;

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
}