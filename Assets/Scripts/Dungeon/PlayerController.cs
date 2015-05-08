using UnityEngine;
using System.Collections;

[RequireComponent(typeof (MovementController))]
[RequireComponent(typeof (WeaponBackpackController))]
public class PlayerController : MonoBehaviour {
 
    private CameraController CamControl;
    private FlashlightController Flashlight;
    private LogScript Log;

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

    // level system
    private int exp;

    //private int attack;

    private float defense;

    private int level;

    private int currLevel;

    // stun variable
    private bool isStunned;

    private float stunCountdown;

    [SerializeField]
    private float stunTime;

    private int currentFloor;
    private int deepestLevelVisited;
    private int dialogueLevel;
    private bool toggle_movement;
    private GameObject player;
    private GameObject hud;
    private GameObject camera;
    
    private void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera"); //Find the camera
        CamControl = camera.GetComponent<CameraController>();
        hud = GameObject.FindGameObjectWithTag("HUD");
        player = GameObject.FindGameObjectWithTag("Player");
        
        GameObject notificationLog = GameObject.FindWithTag("Log");
        Log = notificationLog.GetComponent<LogScript>();

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
        SetStunElectricity(false);

        currentFloor = 0;
        dialogueLevel = 0;
        deepestLevelVisited = 0;
        currLevel = 0;
        level = 0;

        defense = 1f;
	}

	private void Update () {
        if (Time.timeScale == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Drop = false;
            return;
        }

        if (healthController.GetCurrentHealth() == 0 || transform.position.y < -10)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject.DestroyImmediate(camera);
            GameObject.DestroyImmediate(hud);
            GameObject.DestroyImmediate(player);
            if (camera == null && hud == null && player == null)
            Application.LoadLevel("GameOver");
            
        }

        if (isStunned)
        {
            stunCountdown -= Time.deltaTime;
            if (stunCountdown < 0)
            {
                isStunned = false;
                SetStunElectricity(false);
            }
            return;
        }

        if(Input.GetMouseButtonDown(0)) {
            weapon_backpack_controller.StartFiring();
        }
        else if(Input.GetMouseButtonUp(0)) {
            weapon_backpack_controller.StopFiring();
        }

        if (Input.GetKeyDown("z")) {
            Log.EnableNotifications();
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
		} 
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

        // level checking and updating
        CheckLevel();

        if (!(GetCurrentLevel() == GetLevel())) {
            UpdateStats(GetCurrentLevel());
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

    private void SetStunElectricity(bool active)
    {
        foreach (ParticleSystem p in Particles)
        {
            if (p.name == "Stun")
            {
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

                healthController.ChangeHealth(defense * dashSelfDamage * RB.velocity.magnitude);
            }
            else if (OGO.name.Contains("Wall"))
            {
             //   Debug.Log("HIT WALL");
                healthController.ChangeHealth(defense * dashSelfDamage * RB.velocity.magnitude);
                isStunned = true;
                SetStunElectricity(true);
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

                healthController.ChangeHealth(defense * dashSelfDamage * RB.velocity.magnitude * 0.1f);
            }
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
        EndDashing();
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
            if (dashForceCurrent < dashForceMax)
            {
                //float dForce = 0.5f * (Mathf.Pow(dashVelocity + dashAccel, 2) - Mathf.Pow(dashVelocity, 2));
                //dashVelocity += dashAccel;
                dashForceCurrent = dashAccel * Mathf.Pow((Time.realtimeSinceStartup - dashStartTime), 2) + 1;
               // Debug.Log("Dash current: " + dashForceCurrent);
                //dashForceCurrent += dForce;
            }
            else
            {
              //  Debug.Log("dash above 10");
            }
            movement_controller.UpdateMovement(dashForceCurrent, 0);
            //movement_controller.UpdateMovement(dashDirection.x, dashDirection.y);
            healthController.ChangeHealth(defense * dashSelfDamage);
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

    public void AddExp(int comboNum, string enemyName) {

        // add enemies here as needed
        switch(enemyName.ToLower()) {
            case "droneenemy":
                exp += 10 * comboNum;
                break;
            case "warriorenemy":
                exp += 15 * comboNum;
                break;
            default:
                break;    
        }
    }

    public int GetExp() {
        return exp;
    }

    public int GetCurrentLevel() {
        return currLevel;
    }

    private int GetLevel() {
        return level;
    }

    private void CheckLevel() {
        int result = exp / 500;
        switch (result) {
            case 1:
                SetCurrLevel(1);
                break;
            case 2:
                SetCurrLevel(2); 
                break;   
            case 3:
                SetCurrLevel(3); 
                break; 
            case 4:
                SetCurrLevel(4); 
                break;     
            case 5: 
                SetCurrLevel(5); 
                break;     
            case 6:
                SetCurrLevel(6); 
                break;     
            case 7:
                SetCurrLevel(7); 
                break;     
            case 8:
                SetCurrLevel(8); 
                break;     
            case 9:
                SetCurrLevel(9); 
                break;     
            case 10:
                SetCurrLevel(10); 
                break;     
        }
    }

    private void UpdateStats(int currlvl) {
        switch (currlvl) {
            case 1:
                movement_controller.SetWalkForce(95);
                SetLevel(1);
                break;
            case 2:
                movement_controller.SetWalkForce(140);
                SetLevel(2);
                break;   
            case 3:
                movement_controller.SetWalkForce(185);
                SetLevel(3);
                break;     
            case 4:
                movement_controller.SetWalkForce(230);
                SetLevel(4);
                break;    
            case 5:
                movement_controller.SetWalkForce(275);
                SetLevel(5);
                break;
            case 6:       
                movement_controller.SetWalkForce(320);
                SetLevel(6);
                break;    
            case 7:
                movement_controller.SetWalkForce(365);
                SetLevel(7);
                break;  
            case 8:
                movement_controller.SetWalkForce(410);
                SetLevel(8);
                break;    
            case 9:
                movement_controller.SetWalkForce(455);
                SetLevel(9);
                break;    
            case 10:
                movement_controller.SetWalkForce(500);
                SetLevel(10);
                break;    
            default:
                break;            
        }
        AddDefense();
        string str = "Level Up: You are now at level " + GetLevel() + "\n" + "Defense: " + (int) ((1-GetDefense()) * 100) + "% \n";
        Log.PassMessage(str);
        //FINISH LOG STUFF HERE
    }

    private void SetLevel(int x) {
        level = x;
    }

    private void SetCurrLevel(int x) {
        currLevel = x;
    }

    private void AddDefense() {
        defense -= .05f;
    }

    public float GetDefense() {
        return defense;
    }
}
