﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MovementController))]
public class PlayerController : MonoBehaviour {

	[SerializeField] private new Camera camera;

	private MovementController movement_controller;


	private void Start () {
		movement_controller = GetComponent<MovementController>();
	}

	void FixedUpdate () {
		UpdateMovement();
		UpdateRotation();
	}

	private void UpdateMovement () {
		var z_axis = Input.GetAxis("Vertical");
		var x_axis = Input.GetAxis("Horizontal");
		movement_controller.UpdateMovement(z_axis, x_axis);
	}

	private void UpdateRotation () {
		var forward = transform.forward;
		var vector = camera.WorldToScreenPoint(transform.position) - Input.mousePosition;
		var angle = 90 - Mathf.Atan2(forward.y - vector.y, forward.x - vector.x) * Mathf.Rad2Deg;
		var new_rotation = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
		movement_controller.UpdateRotation(new_rotation);
	}
}
