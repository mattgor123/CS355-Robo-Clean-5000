using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[SerializeField] private MovementController movement_controller;

	private Vector3 offset;

	void Start () {
		offset = transform.position;
	}

	void LateUpdate () {
		transform.position = movement_controller.transform.position + offset;
	}
}
