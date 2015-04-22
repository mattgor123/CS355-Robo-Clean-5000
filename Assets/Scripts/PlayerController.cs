﻿using UnityEngine;
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
    private Rigidbody RB;

    private bool Drop = false;          //Whether to drop
    private bool TrackFace;     //Whether camera tracks facing or is map-fixed
    private Vector3 ScreenSize; //size of screen
    private ParticleSystem[] Particles;    //particle systems

    //If player presses button twice within this time
    [SerializeField]
    private float doubleTapTime;

    //Force player moves when dodging
    [SerializeField]
    private float dashForceMax;

    [SerializeField]
    private float dashAccel;

    [SerializeField]
    private float dashSelfDamage;

    [SerializeField]
    private float dashOtherDamage;

    //time since dash started
    //[SerializeField]
    //private float dashTime;

	//[SerializeField]
	//private float dashCooldownTime;

    //private float dashCountdown;

    private bool isDashing;

    private Vector2 dashDirection;

    private float dashForceCurrent;

    private float dashStartTime;

    //Time passed since last key down
    private float doubleTapCountdown;

	//Time passed since last dash down
	//private float dashCooldownCountdown;

    //Number of times key is pressed, for double tap
    //in order of W,A, S,D
    //private int[] doubleTapCounts;
    private int doubleTapCount;

    private bool isStunned;

    private float stunCountdown;

    [SerializeField]
    private float stunTime;

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
        RB = GetComponent<Rigidbody>();

        Flashlight = GetComponentInChildren<FlashlightController>();  //Attach the flashlight
        Particles = GetComponentsInChildren<ParticleSystem>();       //Attach the powerfists
        SetPowerfists(false);


        Drop = false;
        TrackFace = true;
        this.toggle_movement = false;

        ScreenSize = new Vector3(Screen.width, Screen.height);

        doubleTapCountdown = 0;
        //doubleTapCounts = new int[4];
        doubleTapCount = 0;
        //resetDoubleTapCount();
        dashDirection = new Vector2(0, 0);
		//dashCooldownCountdown = 0;
        this.isDashing = false;
        dashForceCurrent = 1;
        dashStartTime = 0;
        isStunned = false;
        stunCountdown = 0;

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

        if (healthController.GetCurrentHealth() == 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true; 
            GameObject.FindGameObjectWithTag("HUD").SetActive(false);
            Application.LoadLevel("GameOver");
            
        }

        if (isStunned)
        {
            stunCountdown -= Time.deltaTime;
            if (stunCountdown < 0)
            {
                isStunned = false;
            }
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

        //Flashlight Toggle
        if (Input.GetKeyDown("f"))
        {
            Flashlight.ToggleFlashlight();
        }

		//double tap for dash
        //if (!isDashing && dashCooldownCountdown == 0) {
        if (!isDashing) {
            /*
			if (Input.GetKeyDown ("d") || Input.GetKeyDown (KeyCode.RightArrow)) {
				if (doubleTapCounts [3] == 0) {
					resetDoubleTapCount ();
					doubleTapCounts [3] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (doubleTapCounts [3] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(0, dashForce, ControlScheme);
					dashDirection = new Vector2 (0, dashForce);
                    isDashing = true;
					//count[3] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					//dashCountdown = dashTime;
				}

			}

			if (Input.GetKeyDown ("s") || Input.GetKeyDown (KeyCode.DownArrow)) {
				if (doubleTapCounts [2] == 0) {
					resetDoubleTapCount ();
					doubleTapCounts [2] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (doubleTapCounts [2] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(-dashForce, 0, ControlScheme);
					dashDirection = new Vector2 (-dashForce, 0);
                    isDashing = true;
					//count[2] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					//dashCountdown = dashTime;
				}

			}

             * */

			if (Input.GetKeyDown ("w") || Input.GetKeyDown (KeyCode.UpArrow)) {
				//if (doubleTapCounts [0] == 0) {
                if (doubleTapCount == 0) {
					//resetDoubleTapCount ();
					//doubleTapCounts [0] += 1;
                    doubleTapCount += 1;
					doubleTapCountdown = doubleTapTime;

				} //else if (doubleTapCounts [0] == 1 && doubleTapCountdown > 0) {
                else if (doubleTapCount == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(dashForce, 0, ControlScheme);
					dashDirection = new Vector2 (dashForceMax, 0);
                    isDashing = true;
                    dashStartTime = Time.realtimeSinceStartup;
					//count[0] = 0;
					//resetDoubleTapCount ();
                    doubleTapCount = 0;
					doubleTapCountdown = 0;
					//dashCountdown = dashTime;

                    CamControl.SmallShake();
                    SetPowerfists(true);

				}

			}

            /*
			if (Input.GetKeyDown ("a") || Input.GetKeyDown (KeyCode.LeftArrow)) {
				if (doubleTapCounts [1] == 0) {
					resetDoubleTapCount ();
					doubleTapCounts [1] += 1;
					doubleTapCountdown = doubleTapTime;

				} else if (doubleTapCounts [1] == 1 && doubleTapCountdown > 0) {
					//dash
					//movement_controller.UpdateMovement(0, -dashForce, ControlScheme);
					dashDirection = new Vector2 (0, -dashForce);
                    isDashing = true;
					//count[1] = 0;
					resetDoubleTapCount ();
					doubleTapCountdown = 0;
					//dashCountdown = dashTime;
				}

			}
             * */
		} 
        //else if (dashCooldownCountdown > 0) {
			//dashCooldownCountdown = dashCooldownCountdown - Time.deltaTime;
		//} else if (dashCooldownCountdown < 0) {
		//	dashCooldownCountdown = 0;
		//}
        else 
        {
            //isDashing
            if (Input.GetKeyUp("w") || Input.GetKeyUp(KeyCode.UpArrow))
            {
                EndDashing();
            }
        }


        if (doubleTapCountdown > 0)
        {
            doubleTapCountdown = doubleTapCountdown - Time.deltaTime;
        }
        else if (doubleTapCountdown < 0)
        {
            //resetDoubleTapCount();
            doubleTapCountdown = 0;
            doubleTapCount = 0;
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


    //Set powerfists
    private void SetPowerfists(bool active)
    {
        foreach (ParticleSystem p in Particles)
        {
            if (p.name == "Powerfist") {
                if (active)
                    p.Play();
                else
                    p.Stop();
            }
        }
    }

    private void EndDashing()
    {
        isDashing = false;
        dashForceCurrent = 1;
        dashStartTime = 0;
        CamControl.StopShaking();
        SetPowerfists(false);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (isDashing)
        {
            GameObject OGO = other.gameObject;
            if (OGO.tag == "Enemy")
            {
                HealthController HC = OGO.GetComponent<HealthController>();
                HC.ChangeHealth(dashOtherDamage*RB.velocity.magnitude);

                healthController.ChangeHealth(dashSelfDamage * RB.velocity.magnitude);
            }
            else if (OGO.name.Contains("Wall"))
            {
                Debug.Log("HIT WALL");
                healthController.ChangeHealth(dashSelfDamage * RB.velocity.magnitude);
                isStunned = true;
                stunCountdown = stunTime;
                EndDashing();
            }
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


        //deal damage to enemy when dashing
        if (isDashing)
        {
            GameObject OGO = other.gameObject;
            if (OGO.tag == "Enemy")
            {
                HealthController HC = OGO.GetComponent<HealthController>();
                HC.ChangeHealth(dashOtherDamage*RB.velocity.magnitude*0.1f);
                Debug.Log(HC.GetCurrentHealth());

                healthController.ChangeHealth(dashSelfDamage * RB.velocity.magnitude * 0.1f);
            }
        }

    }

    /*
    private void resetDoubleTapCount()
    {
        for (int i = 0; i < doubleTapCounts.Length; i++)
        {
            doubleTapCounts[i] = 0;
        }
    }
     * */

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

    public void incrementDeepestLevelVisited()
    {
        deepestLevelVisited += 1;
    }

    public bool getIsDashing()
    {
        return this.isDashing;
    }

    public float getDashDamage()
    {
        return this.dashOtherDamage;
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
        if (isStunned)
        {
            return;
        }
        if (!isDashing)
        {
            var z_axis = Input.GetAxis("Vertical");
            var x_axis = Input.GetAxis("Horizontal");
            movement_controller.UpdateMovement(z_axis, x_axis);
        }
        //else if (dashCountdown > 0)
        else
        {
            Debug.Log("velocity " + GetComponent<Rigidbody>().velocity);
            if (dashForceCurrent < dashForceMax)
            {
                //float dForce = 0.5f * (Mathf.Pow(dashVelocity + dashAccel, 2) - Mathf.Pow(dashVelocity, 2));
                //dashVelocity += dashAccel;
                dashForceCurrent = dashAccel * Mathf.Pow((Time.realtimeSinceStartup - dashStartTime), 2) + 1;
                Debug.Log("Dash current: " + dashForceCurrent);
                //dashForceCurrent += dForce;
            }
            else
            {
                Debug.Log("dash above 10");
            }
            movement_controller.UpdateMovement(dashForceCurrent, 0);
            //movement_controller.UpdateMovement(dashDirection.x, dashDirection.y);
            healthController.ChangeHealth(dashSelfDamage);
            //dashCountdown = dashCountdown - Time.deltaTime;
        }
        //else
        //{
        //    dashCountdown = 0;
		//	dashCooldownCountdown = dashCooldownTime;
        //}
	}

	private void UpdateRotation () {
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
}
