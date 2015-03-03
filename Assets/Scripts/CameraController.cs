using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player_Movement_Controller;


	private Vector3 offset;

	void Start () {
        Player_Movement_Controller = GameObject.FindGameObjectWithTag("Player");
		offset = transform.position;
	}

	void LateUpdate () {

		transform.position = Player_Movement_Controller.transform.position + offset;
        //this.transform.forward = Player_Movement_Controller.transform.position.x;
        transform.LookAt(Player_Movement_Controller.transform);
        transform.Translate(Vector3.right * Time.deltaTime);
        transform.forward = Player_Movement_Controller.transform.forward;
        transform.rotation = Quaternion.AngleAxis(60f, Vector3.right);


	}
}
