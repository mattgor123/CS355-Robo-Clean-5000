﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class MovementController : MonoBehaviour {

	[SerializeField] private float walk_force;     // The force to apply when walking
	[SerializeField] private float run_force;      // The force to apply when running
	[SerializeField] private float force_delay;    // How long to wait in between applications of force
	[SerializeField] private float rotation_delay; // How long to wait in between rotations
	[SerializeField] private Transform wheel;

	private float last_force;
	private float last_rotation;
	private float speedDampTime = 0.1f;

	private HealthController healthController;
    private Rigidbody RG;
    private Transform wheelTransform;

	private void Awake() {

		healthController = GetComponent<HealthController>();
        RG = GetComponent<Rigidbody>();
        wheelTransform = wheel.GetComponent<Transform>();

	}

	private void Start () {
		// The rotation is not handled by physics
		GetComponent<Rigidbody>().freezeRotation = true;
		last_force = -force_delay;
		last_rotation = -rotation_delay;
	}

	public void UpdateMovement (float z_axis, float x_axis) {        
		// if the player is still alive, apply movement logic
        Vector3 resulting_force;
        if (healthController.GetCurrentHealth() > 0)
        {
            // if the object is not standing still, move object
            if (z_axis != 0f || x_axis != 0f)
            {
                var force = walk_force;

                var z_force = transform.forward * z_axis * force * Time.deltaTime * 2f;
                var x_force = transform.right * x_axis * force * Time.deltaTime * 2f;

                resulting_force = z_force + x_force;
            
            	wheelTransform.Rotate(3 * z_axis * force * Time.deltaTime, 0, 0);
            	Debug.Log("Rotation: " + wheelTransform.rotation);

                RG.AddForce(resulting_force, ForceMode.Impulse);
                last_force = Time.time;
            }
            else
            {
                RG.velocity *= 0.5f;
            }

            //Keep velocity bounded
            if (RG.velocity.magnitude >= 5.5f)
            {
                RG.velocity = RG.velocity.normalized * 10f;
            }
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
