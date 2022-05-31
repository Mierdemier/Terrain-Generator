using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseInput : MonoBehaviour
{
    Brush selectedBrush;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData)){
                Vector3 hitPosition = hitData.point;

                //Do something
                selectedBrush.Apply(hitData);
                Debug.Log(hitPosition.ToString());
            }
        }
    }
}