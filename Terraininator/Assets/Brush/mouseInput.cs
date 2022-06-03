using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour
{
    Brush brush = new Brush();

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
                brush.Apply(hitData);
                Debug.Log(hitPosition.ToString());
            }
        }
    }
}