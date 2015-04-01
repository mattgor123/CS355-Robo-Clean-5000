using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MovementController))]
[RequireComponent(typeof (WeaponBackpackController))]
public class PlayerController : MonoBehaviour {
 
	private new Camera camera;
    private CameraController CamControl;
    private FlashlightController Flashlight;

	private MovementController movement_controller;
	private WeaponBackpackController weapon_backpack_controller;
	private HealthController healthController;
    private bool ControlScheme;  //True for mouse-oriented (W goes to mouse, S goes away from mouse)
                                //False for screen-oriented (W goes up, S goes down)

    private bool Drop;          //Whether to drop
    private bool TrackFace;     //Whether camera tracks facing or is map-fixed
    private Vector3 ScreenSize; //size of screen

    //If player presses button twice within this time
    [SerializeField]
    private float doubleTapTime;

    //Force player moves when dodging
    [SerializeField]
    private float dashForce;

    //time since dash started
    [SerializeField]
    private float dashTime;

	[SerializeField]
	private float dashCooldownTime;

    private float dashCountdown;

    private bool isDodging;

    private Vector2 dashDirection;

    //Time passed since last key down
    private float doubleTapCountdown;

	//Time passed since last dash down
	private float dashCooldownCountdown;

    //Number of times key is pressed, for double tap
    //in order of W,A, S,D
    private int[] count;

    private int currentFloor;
    private int deepestLevelVisited;
    private int dialogueLevel;
    private bool toggle_movement;
    private Animator anim;

	private void Start () {
        GameObject MC = GameObject.FindGameObjectWithTag("MainCamera"); //Find the camera
        camera = MC.GetComponent<Camera>();                             //Attach camera and controller components
        CamControl = MC.GetComponent<CameraController>();
        anim = GetComponent<Animator>();

		movement_controller = GetComponent<MovementController>();
		weapon_backpack_controller = GetComponent<WeaponBackpackController>();
		healthController = GetComponent<HealthController>();

        Flashlight = GetComponentInChildren<FlashlightController>();  //Attach the flashlight

        ControlScheme = true;
        Drop = false;
        TrackFace = true;
        this.toggle_movement = false;
        CamControl.SetTrackFace(TrackFace);

        ScreenSize = new Vector3(Screen.width, Screen.height);

        doubleTapCountdown = 0;
        //count = 0;
        count = new int[4];
        resetDoubleTapCount();
        dashDirection = new Vector2(0, 0);
		dashCooldownCountdown = 0;

        currentFloor = 0;
        dialogueLevel = 0;
        deepestLevelVisited = 0;
	}

	private void Update () {
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Drop = false;
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            if (this.toggle_movement) {
                this.toggle_movement = false;
            } else {
                this.toggle_movement = true;
            }

            if (this.toggle_movement) {
                if (anim.GetBool("isIdle")) {
                    anim.SetLayerWeight(1, 1);   
                } 
            } else {
                anim.SetLayerWeight(1, 0);
            }    

        }

        if(Input.GetMouseButtonDown(0)) {
            if (anim.GetBool("isIdle") && toggle_movement ||
                !(anim.GetBool("isIdle"))) {
                weapon_backpack_controller.StartFiring();
            }
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
			GameObject nlog = GameObject.FindWithTag("Log");
			NotificationLog log = nlog.GetComponent<NotificationLog>(); 
            if (ControlScheme)
            {
                ControlScheme = false;  
				log.PassMessage("cardinal controls");
            }
            else
            {
                ControlScheme = true;
				log.PassMessage("mouse oriented controls");
            }
        }

        //Track Facing toggle
        if (Input.GetKeyDown("k"))
        {
            if (TrackFace)
            {
                TrackFace = false;
            }
            else
            {
                TrackFace = true;
            }
            CamControl.SetTrackFace(TrackFace);
        }

        //Flashlight Toggle
        if (Input.GetKeyDown("f"))
        {
            Flashlight.ToggleFlashlight();
        }

		//double tap for dash
        if (dashCountdown == 0 && dashCooldownCountdown == 0) {
			if (Input.GetKeyDown ("d") || Input.GetKeyDown (KeyCode.RightArrow)) {
				if (count [3] == 0) {
					resetDoubleTapCount ();
					count [3] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (count [3] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(0, dashForce, ControlScheme);
					dashDirection = new Vector2 (0, dashForce);

					//count[3] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					dashCountdown = dashTime;
				}

			}

			if (Input.GetKeyDown ("s") || Input.GetKeyDown (KeyCode.DownArrow)) {
				if (count [2] == 0) {
					resetDoubleTapCount ();
					count [2] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (count [2] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(-dashForce, 0, ControlScheme);
					dashDirection = new Vector2 (-dashForce, 0);
					//count[2] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					dashCountdown = dashTime;
				}

			}

			if (Input.GetKeyDown ("w") || Input.GetKeyDown (KeyCode.UpArrow)) {
				if (count [0] == 0) {
					resetDoubleTapCount ();
					count [0] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (count [0] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(dashForce, 0, ControlScheme);
					dashDirection = new Vector2 (dashForce, 0);
					//count[0] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					dashCountdown = dashTime;
				}

			}

			if (Input.GetKeyDown ("a") || Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (count [1] == 0) {
					resetDoubleTapCount ();
					count [1] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (count [1] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(0, -dashForce, ControlScheme);
					dashDirection = new Vector2 (0, -dashForce);
					//count[1] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					dashCountdown = dashTime;
				}

			}
		} else if (dashCooldownCountdown > 0) {
			dashCooldownCountdown = dashCooldownCountdown - Time.deltaTime;
		} else if (dashCooldownCountdown < 0) {
			dashCooldownCountdown = 0;
		}
        
        if (doubleTapCountdown > 0)
        {
            doubleTapCountdown = doubleTapCountdown - Time.deltaTime;
        }
        else if (doubleTapCountdown < 0)
        {
            resetDoubleTapCount();
            doubleTapCountdown = 0;
        }

        //Make sure cursor is in the right mode 
        if (TrackFace)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;  //keep cursor on screen for testing
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
        }
	}

    private void LateUpdate()
    {
        if (Time.timeScale == 0) return;
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

    private void resetDoubleTapCount()
    {
        for (int i = 0; i < count.Length; i++)
        {
            count[i] = 0;
        }
    }

    public void incrementCurrentFloor()
    {
        currentFloor += 1;
        
        if (currentFloor > deepestLevelVisited)
        {
            deepestLevelVisited = currentFloor;
        }
         
    }

    public void decrementCurrentFloor()
    {
        currentFloor -= 1;
    }

    public void setCurrentFloor(int floor)
    {
        currentFloor = floor;
        if (currentFloor > deepestLevelVisited)
        {
            deepestLevelVisited = currentFloor;
        }
    }

    public void incrementDialogueLevel()
    {
        dialogueLevel += 1;
    }

    public int getCurrentFloor()
    {
        return currentFloor;
    }

    public int getDialogueLevel()
    {
        return dialogueLevel;
    }

    public int getDeepestLevelVisited()
    {
        return deepestLevelVisited;
    }

    private void incrementDeepestLevelVisited()
    {
        deepestLevelVisited += 1;
    }

    public bool GetFlashlightActive()
    {
        return Flashlight.GetActive();
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
        if (dashCountdown == 0)
        {
            var z_axis = Input.GetAxis("Vertical");
            var x_axis = Input.GetAxis("Horizontal");
            movement_controller.UpdateMovement(z_axis, x_axis, ControlScheme);
        }
        else if (dashCountdown > 0)
        {
            movement_controller.UpdateMovement(dashDirection.x, dashDirection.y, ControlScheme);
            dashCountdown = dashCountdown - Time.deltaTime;
            //Debug.Log(Time.deltaTime);
        }
        else
        {
            dashCountdown = 0;
			dashCooldownCountdown = dashCooldownTime;
        }
	}

	private void UpdateRotation () {

        //player-fixed camera
        if (TrackFace)
        {
            float delta = Input.GetAxis("Mouse X");

            //Rotate proportional to mouse movement
            float angle = transform.eulerAngles.y + (4.0f) * delta;
            if (angle > 360)
            {
                angle -= 360;
            }
            else if (angle < 0)
            {
                angle += 360;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }
        //map-fixed camera
        else
        {
            var forward = transform.forward;
            var vector = camera.WorldToScreenPoint(transform.position) - Input.mousePosition;
            var angle = 90 - Mathf.Atan2(forward.y - vector.y, forward.x - vector.x) * Mathf.Rad2Deg;
            var new_rotation = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
            movement_controller.UpdateRotation(new_rotation);
        }
	}
}
