using UnityEngine;
using UnityEngine.EventSystems;

//Moves the camera.
public class CameraScript : MonoBehaviour
{
    [SerializeField]
    ChunkSystem chunkSysScript;

    Transform swivel, stick;
    [SerializeField]
    float stickMinZoom, stickMaxZoom;
    [SerializeField]
    float swivelMinZoom, swivelMaxZoom;
    [SerializeField]
    float moveSpeedMinZoom, moveSpeedMaxZoom;
    [SerializeField]
    float rotationSpeed;
    
    float zoom = 1f;
    float rotationAngle;

    void Awake() {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
    }

    void Update() {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if(zoomDelta != 0f && !EventSystem.current.IsPointerOverGameObject()) {
            AdjustZoom(zoomDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if(xDelta != 0f || zDelta != 0f) {
            AdjustPosition(xDelta, zDelta);
        }

        float rotationDelta = Input.GetAxis("Rotation");
        if(rotationDelta != 0f) {
            AdjustRotation(rotationDelta);
        }
    }

    public void setZoom(float minZoom, float maxZoom) {
        stickMaxZoom = maxZoom;
        stickMinZoom = minZoom;
    }

    void AdjustZoom(float delta) {
        zoom = Mathf.Clamp01(zoom + delta);
        
        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);
        
        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    void AdjustPosition(float xDelta, float zDelta) {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * Time.deltaTime;

        Vector3 curr_position = transform.localPosition;
        curr_position += direction * distance;
        transform.localPosition = ClampPosition(curr_position);
    }

    Vector3 ClampPosition(Vector3 position) {
        float xMax = chunkSysScript.numChunks.x * chunkSysScript.ChunkSize;
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float zMax = chunkSysScript.numChunks.y * chunkSysScript.ChunkSize;
        position.z = Mathf.Clamp(position.z, 0f, zMax);

        return position;
    }

    void AdjustRotation(float rotationDelta) {
        rotationAngle += rotationDelta * rotationSpeed * Time.deltaTime;

        if(rotationAngle < 0f) {
            rotationAngle += 360f;
        } else if (rotationAngle >= 360f){
            rotationAngle -= 360f;
        }

        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

}
