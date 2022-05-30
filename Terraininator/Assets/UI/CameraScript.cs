using UnityEngine;

public class CameraScript : MonoBehaviour
{
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
        if(zoomDelta != 0f) {
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
        transform.localPosition = curr_position;
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
