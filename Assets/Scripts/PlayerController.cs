using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MovementController))]
[RequireComponent(typeof (WeaponBackpackController))]
public class PlayerController : MonoBehaviour {
 
	private new Camera camera;

	private MovementController movement_controller;
	private WeaponBackpackController weapon_backpack_controller;
	private HealthController healthController;
    private bool ControlScheme;  //True for mouse-oriented (W goes to mouse, S goes away from mouse)
                                //False for screen-oriented (W goes up, S goes down)

    private bool Drop;          //Whether to drop

	private void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		movement_controller = GetComponent<MovementController>();
		weapon_backpack_controller = GetComponent<WeaponBackpackController>();
		healthController = GetComponent<HealthController>();
        ControlScheme = true;
        Drop = false;
	}

	private void Update () {
		if(Input.GetMouseButtonDown(0)) {
			weapon_backpack_controller.StartFiring();
		}
		else if(Input.GetMouseButtonUp(0)) {
			weapon_backpack_controller.StopFiring();
		}

		if(Input.GetKeyDown("r")) {
			weapon_backpack_controller.NextWeapon();
		}

        //Control scheme toggle
        if (Input.GetKeyDown("c"))
        {
            if (ControlScheme)
            {
                ControlScheme = false;
            }
            else
            {
                ControlScheme = true;
            }
        }
	}

    private void LateUpdate()
    {
        //Drop if drop flag is on (not colliding and thus floating)
        if (Drop)
        {
            transform.position += new Vector3(0f, -0.05f, 0f);
        }
    }

    //Deactivate drop flag if colliding with something (the floor)
    private void OnCollisionStay(Collision other)
    {
        //If colliding with a ramp, move up a bit so that player can go up ramp)
        //May cause some bouncing if staying put on a ramp
        if (other.gameObject.tag == "Ramp")
        {
            transform.position += new Vector3(0f, 0.01f, 0f);
        }
        Drop = false;
    }


	public void AddWeapon (GameObject weapon) {
		weapon_backpack_controller.AddWeapon(weapon);
	}

	private void FixedUpdate () {
		if (healthController.GetCurrentHealth() > 0) {
			UpdateRotation();
		}
        UpdateMovement();
        Drop = true;
	}

	private void UpdateMovement () {
		var z_axis = Input.GetAxis("Vertical");
		var x_axis = Input.GetAxis("Horizontal");
		movement_controller.UpdateMovement(z_axis, x_axis, ControlScheme);
	}

	private void UpdateRotation () {
		var forward = transform.forward;
		var vector = camera.WorldToScreenPoint(transform.position) - Input.mousePosition;
		var angle = 90 - Mathf.Atan2(forward.y - vector.y, forward.x - vector.x) * Mathf.Rad2Deg;
		var new_rotation = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
		movement_controller.UpdateRotation(new_rotation);
	}
}
