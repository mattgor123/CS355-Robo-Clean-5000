    using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	[SerializeField]
	private float speed;

    [SerializeField]
    private CameraController cam;

	void Start() {
	}

	//Physics Code
	void FixedUpdate () {
		float moveVertical = Input.GetAxis ("Vertical");    //forward movement; turning linked to camera

        float angle = cam.getAngle() * Mathf.PI/180;

        float z = Mathf.Cos(angle) * moveVertical;
        float x = Mathf.Sin(angle) * moveVertical;
  
		Vector3 mvt = new Vector3(x, 0.0f, z);
		rigidbody.AddForce (mvt * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {

	}
	
}