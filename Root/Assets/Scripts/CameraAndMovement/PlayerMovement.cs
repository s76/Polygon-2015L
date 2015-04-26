using UnityEngine;
using System.Collections;
 
public class PlayerMovement : MonoBehaviour {
	
	public KeyCode moveUp; 
	public KeyCode moveDown; 
	public KeyCode moveLeft; 
	public KeyCode moveRight;
    public KeyCode zoomIn;
    public KeyCode zoomOut;

	public const float PLAYER_SPEED = 5.0f;

	public const float X_OFFSET = 0.0f;
	public const float Y_OFFSET = 15.0f;
	public const float Z_OFFSET = -10.0f;

    public const float MIN_ZOOM = 0.5f;
    public const float MAX_ZOOM = 2.0f;
    public const float ZOOM_SPEED = 0.01f;
	
	static Vector3 UP = new Vector3 (0, 0, 0);
	static Vector3 UP_RIGHT = new Vector3 (0, 45, 0);
	static Vector3 RIGHT = new Vector3 (0, 90, 0);
	static Vector3 DOWN_RIGHT = new Vector3 (0, 135, 0);
	static Vector3 DOWN = new Vector3 (0, 180, 0);
	static Vector3 DOWN_LEFT = new Vector3 (0, 225, 0);
	static Vector3 LEFT = new Vector3 (0, 270, 0);
	static Vector3 UP_LEFT = new Vector3 (0, 315, 0);

	float zoom = 1.0f;

    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    private Quaternion targetRotation;
    private CharacterController controller;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody rigidbody = GetComponent<Rigidbody> (); 
		Vector3 velocity = rigidbody.velocity;
		Quaternion rotation = rigidbody.rotation;
		Vector3 position = rigidbody.position;
		Vector3 cameraPosition = Camera.main.transform.position;

		if (Input.GetKey (zoomOut)) {
			if(zoom < MAX_ZOOM){
				zoom += ZOOM_SPEED;
			}
		} else if (Input.GetKey (zoomIn)) {
			if(zoom > MIN_ZOOM){
				zoom -= ZOOM_SPEED;
			}
		}

		//make camera track player
		cameraPosition.x = position.x+X_OFFSET*zoom;
		cameraPosition.y = position.y+Y_OFFSET*zoom;
		cameraPosition.z = position.z+Z_OFFSET*zoom;
    
		Camera.main.transform.position = cameraPosition;
	}
}
