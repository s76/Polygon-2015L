using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float currentSpeed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * currentSpeed);
    }
	
}
