using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class MovementController : MonoBehaviour {

	[SerializeField] private float walk_force;     // The force to apply when walking
	[SerializeField] private float run_force;      // The force to apply when running
	[SerializeField] private float force_delay;    // How long to wait in between applications of force
	[SerializeField] private float rotation_delay; // How long to wait in between rotations

	private float last_force;
	private float last_rotation;

	private void Start () {
		// The rotation is not handled by physics
		rigidbody.freezeRotation = true;
		last_force = -force_delay;
		last_rotation = -rotation_delay;
	}

	public void UpdateMovement (float z_axis, float x_axis) {
		if(Time.time - last_force > force_delay) {
			var is_walking = Mathf.Abs(z_axis) < 0.5f && Mathf.Abs(x_axis) < 0.5f;
			var force = is_walking ? walk_force : run_force;
			var z_force = transform.forward * z_axis * force;
			var y_force = transform.right * x_axis * force;
			rigidbody.AddForce(z_force + y_force);
			last_force = Time.time;
		}
	}

	public void UpdateRotation (Vector3 euler_angles) {
		transform.eulerAngles = euler_angles;
	}

	public void LookAt (Vector3 target) {
		if(Time.time - last_rotation > rotation_delay) {
			var direction = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);
			var projected_forward = Vector3.Scale(transform.forward, new Vector3(1.0f, 0, 1.0f)).normalized;
			var y_angle = Vector3.Angle(projected_forward, direction);
			if(Vector3.Dot(Vector3.right, direction) <= 0) {
				y_angle = -y_angle;
			}
			transform.eulerAngles += new Vector3(0, y_angle, 0);
			last_rotation = Time.time;
		}
	}
}
