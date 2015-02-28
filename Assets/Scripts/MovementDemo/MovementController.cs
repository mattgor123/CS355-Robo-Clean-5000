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
	private float speedDampTime = 0.1f;
	//[SerializeField] 
	private float HP;

	private Animator anim;

	private void Awake() {
		anim = GetComponent<Animator>();
		HP = 100;
	}

	private void Start () {
		// The rotation is not handled by physics
		rigidbody.freezeRotation = true;
		last_force = -force_delay;
		last_rotation = -rotation_delay;
	}

	public void UpdateMovement (float z_axis, float x_axis) {
		if (HP > 0) {
			if(z_axis != 0f || x_axis != 0f) {
				var force = walk_force;
				var z_force = transform.forward * z_axis * force * Time.deltaTime;
            	var x_force = transform.right * x_axis * force * Time.deltaTime;
				anim.SetFloat("Speed", 5.5f, speedDampTime, Time.deltaTime);
				rigidbody.AddForce(z_force + x_force, ForceMode.Impulse);
				last_force = Time.time;
			} else {
				anim.SetFloat("Speed", 0f);
			}

        	if (z_axis == 0 && x_axis == 0)
        	{
            	rigidbody.velocity *= 0.5f;
        	}
        } else {
        	anim.SetBool("Dead", true);
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
