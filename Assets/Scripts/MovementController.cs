using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class MovementController : MonoBehaviour {

	[SerializeField] private float walk_force;
	[SerializeField] private float run_force;

	private void Start () {
		// The rotation is not handled by physics
		rigidbody.freezeRotation = true;
	}

	public void UpdateMovement (float z_axis, float x_axis) {
		var is_walking = Mathf.Abs(z_axis) < 0.5f && Mathf.Abs(x_axis) < 0.5f;
		var force = is_walking ? walk_force : run_force;
		var z_force = transform.forward * z_axis * force;
		var y_force = transform.right * x_axis * force;
		rigidbody.AddForce(z_force + y_force);
	}

	public void UpdateRotation (Vector3 euler_angles) {
		transform.eulerAngles = euler_angles;
	}
}
