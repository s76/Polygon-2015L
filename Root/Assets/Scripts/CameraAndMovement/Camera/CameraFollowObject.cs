using UnityEngine;
using System.Collections;

public class CameraFollowObject : MonoBehaviour {

    public GameObject objectToFollow;

    public KeyCode zoomIn;
    public KeyCode zoomOut;

    public const float X_OFFSET = 0.0f;
    public const float Y_OFFSET = 15.0f;
    public const float Z_OFFSET = -10.0f;

    public const float MIN_ZOOM = 0.5f;
    public const float MAX_ZOOM = 2.0f;
    public const float ZOOM_SPEED = 0.01f;

    float zoom = 1.0f;


    public float rotationSpeed = 450;
    private Quaternion targetRotation;
    private CharacterController controller;

    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Vector3 position = objectToFollow.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        if (Input.GetKey(zoomOut)) {
            if (zoom < MAX_ZOOM) {
                zoom += ZOOM_SPEED;
            }
        }
        else if (Input.GetKey(zoomIn)) {
            if (zoom > MIN_ZOOM) {
                zoom -= ZOOM_SPEED;
            }
        }

        //make camera track player
        cameraPosition.x = position.x + X_OFFSET * zoom;
        cameraPosition.y = position.y + Y_OFFSET * zoom;
        cameraPosition.z = position.z + Z_OFFSET * zoom;

        Camera.main.transform.position = cameraPosition;
    }
}
