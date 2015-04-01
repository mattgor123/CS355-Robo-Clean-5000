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

	private Animator anim;
	private HealthController healthController;

	private void Awake() {
		anim = GetComponent<Animator>();
		if (anim.layerCount >= 2) {
			anim.SetLayerWeight(1, 1);
		}
		healthController = GetComponent<HealthController>();
	}

	private void Start () {
		// The rotation is not handled by physics
		GetComponent<Rigidbody>().freezeRotation = true;
		last_force = -force_delay;
		last_rotation = -rotation_delay;
	}

	public void UpdateMovement (float z_axis, float x_axis, bool ControlScheme) {        
		// if the player is still alive, apply movement logic
        Vector3 resulting_force;
        if (healthController.GetCurrentHealth() > 0)
        {
            // if the object is not standing still, move object
            if (z_axis != 0f || x_axis != 0f)
            {
                var force = walk_force;
                Vector3 counteraction = new Vector3(0f, 0f, 0f);      //counteracts animator applied force
                if (ControlScheme)
                {
                    var z_force = transform.forward * z_axis * force * Time.deltaTime * 2f;
                    var x_force = transform.right * x_axis * force * Time.deltaTime * 2.5f;
                    if (z_axis <= 0f)
                        counteraction = transform.forward * -7.5f;
                    resulting_force = z_force + x_force;
                }
                else
                {
                    var z_force = new Vector3(0f, 0f, z_axis);
                    var x_force = new Vector3(x_axis, 0f, 0f);
                    counteraction = transform.forward * -6.5f;
                    resulting_force = force * Time.deltaTime * 3.0f * (z_force + x_force).normalized;
                }
                anim.SetFloat("Speed", 5.5f, speedDampTime, Time.deltaTime);
                GetComponent<Rigidbody>().AddForce(resulting_force, ForceMode.Impulse);
                GetComponent<Rigidbody>().AddForce(counteraction, ForceMode.Impulse);
                last_force = Time.time;
            }
            else
            {
                anim.SetFloat("Speed", 0f);
            }
        }
        else
        {
            anim.SetBool("isDead", true);
            anim.SetLayerWeight(1, 0);
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
