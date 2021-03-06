using UnityEngine;
using UnityEngine.EventSystems;

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
        //Only show the brush when the mouse is not over the UI.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData))
            {
                //Update decal.
                Decal.gameObject.SetActive(true);
                Decal.position = hitData.point + Vector3.up;
                Decal.localScale = new Vector3(selectedBrush.radius , selectedBrush.radius, 0.1f);

                //Left mouse button is clicked.
                if(Input.GetMouseButton(0))
                {
                    //Apply brush to affected area.
                    selectedBrush.Apply(chunkSystem, hitData);
                }
                if (Input.GetMouseButtonUp(0))
                    selectedBrush.Finish(chunkSystem, hitData);
            }
            else
                Decal.gameObject.SetActive(false);
        }
        else
            Decal.gameObject.SetActive(false);
    }

    public void SetBrush(Brush brush)
    {
        selectedBrush = brush;
    }

}