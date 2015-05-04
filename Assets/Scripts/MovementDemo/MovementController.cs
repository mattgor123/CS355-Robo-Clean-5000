using UnityEngine;
using System.Collections;

[RequireComponent(typeof (Rigidbody))]
public class MovementController : MonoBehaviour {

	[SerializeField] private float Accel;      // The force to apply when running
	[SerializeField] private float force_delay;    // How long to wait in between applications of force
	[SerializeField] private float rotation_delay; // How long to wait in between rotations
	[SerializeField] private Transform wheel;
	[SerializeField] private Transform leftHand;
	[SerializeField] private Transform rightHand;
	[SerializeField] private Transform pelvis;
    [SerializeField] private Transform leftUpperArm;
    [SerializeField] private Transform leftForeArm;
    [SerializeField] private Transform rightUpperArm;
    [SerializeField] private Transform rightForeArm;
    [SerializeField] private Transform body;


	private float last_force;
	private float last_rotation;
	private float speedDampTime = 0.1f;
    private float deltaRotateDash = 0;
    private float deltaRotateHands = 5;
    private float deltaRotateForward = 0;


	private HealthController healthController;
    private Rigidbody RG;
    // rotation transforms
    private Transform wheelTransform;
    private Transform leftHandTransform;
    private Transform rightHandTransform;
    private Transform pelvisTransform;
    private Transform leftUpperArmTransform;
    private Transform leftForeArmTransform;
    private Transform rightUpperArmTransform;
    private Transform rightForeArmTransform;
    private Transform bodyTransform;


	private void Awake() {

		healthController = GetComponent<HealthController>();
        RG = GetComponent<Rigidbody>();
        wheelTransform = wheel.GetComponent<Transform>();
        leftHandTransform = leftHand.GetComponent<Transform>();
        rightHandTransform = rightHand.GetComponent<Transform>();
        pelvisTransform = pelvis.GetComponent<Transform>();
        leftUpperArmTransform = leftUpperArm.GetComponent<Transform>();
        leftForeArmTransform = leftForeArm.GetComponent<Transform>();
        rightUpperArmTransform = rightUpperArm.GetComponent<Transform>();
        rightForeArmTransform = rightForeArm.GetComponent<Transform>();
        bodyTransform = body.GetComponent<Transform>();
	}

	private void Start () {
		RG.freezeRotation = true;
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
                float force = Accel;

                var z_force = transform.forward * z_axis;
                var x_force = transform.right * x_axis;

                resulting_force = z_force + x_force;
                resulting_force.Normalize();
                resulting_force *= force * Time.deltaTime;

                if (z_axis > 1) {
                    Vector3 rot = new Vector3(-5, 0f, 0f);
                    if (deltaRotateDash < 90) {
                        leftUpperArmTransform.Rotate(rot);
                        rightUpperArmTransform.Rotate(rot);
                        leftForeArmTransform.Rotate(-rot);
                        rightForeArmTransform.Rotate(-rot);
                        if (deltaRotateForward < 50f) {
                            bodyTransform.Rotate(.3f, 0f, 0f);
                            deltaRotateForward += .5f;
                        }
                        deltaRotateDash += 5;
                        deltaRotateHands = 30;
                    }
                }

                // rotate hands when moving
            	leftHandTransform.Rotate(0, 0, deltaRotateHands);
        		rightHandTransform.Rotate(0, 0, -deltaRotateHands);
        		// rotate wheel to give "rolling" effect
            	wheelTransform.Rotate(3 * (z_axis + x_axis) * force * Time.deltaTime, 0, 0);
            	// rotate pelvis in direction of movement
            	Quaternion rotate = Quaternion.LookRotation(resulting_force);
            	pelvisTransform.rotation = rotate;


                RG.AddForce(resulting_force, ForceMode.Impulse);
                last_force = Time.time;
            }
            else
            {
                RG.velocity *= 0.5f;

                Vector3 rot = new Vector3(5, 0f, 0f);
                if (deltaRotateDash > 0f) {
                    leftUpperArmTransform.Rotate(rot);
                    rightUpperArmTransform.Rotate(rot);
                    leftForeArmTransform.Rotate(-rot);
                    rightForeArmTransform.Rotate(-rot);
                    if (deltaRotateForward > 0) {
                            bodyTransform.Rotate(-.3f, 0f, 0f);
                            deltaRotateForward -= .5f;
                        }
                    deltaRotateDash -= 5;
                }
                deltaRotateHands = 5;
            }

            //Keep velocity bounded
            if (RG.velocity.magnitude >= 10f)
            {
                RG.velocity = RG.velocity.normalized * 10f;
            }
        }
	}

	public void UpdateRotation (Vector3 euler_angles) {
		transform.eulerAngles = euler_angles;
	}


    public void SetWalkForce(int x) {
        Accel = x;
    }

    public float GetForce() {
        return Accel;
    }
}
