using UnityEngine;

public class mouseInput : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunkSystem;

    [SerializeField]
    Brush selectedBrush;
    [SerializeField]
    Transform Decal;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData))
        {
            //Update decal.
            Decal.position = hitData.point + Vector3.up;
            Decal.localScale = new Vector3(selectedBrush.GetRadius() , selectedBrush.GetRadius(), 0.1f);

            //Left mouse button is clicked.
            if(Input.GetMouseButton(0))
            {
                //Apply brush to affected area.
                selectedBrush.Apply(chunkSystem, hitData, Time.deltaTime);
            }
        }
    }

}