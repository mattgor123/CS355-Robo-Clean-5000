using UnityEngine;
using System.Collections;

/* Generic Enemy Class
 *  
 */

public class EnemyController : MonoBehaviour {
    [SerializeField] 
    private GameObject player;

    // Internal Variables
    // Positional/Movement variables
    [SerializeField]
    private float speed;
    public GameObject explosion;

    private HealthController health_controller;
    private Vector3 PrevPos;    //previous position
    private Vector3 PrevMvt;    //distance moved in previous movement step
    private float PrevTime;     //the previous time interval
    private bool WallHit;       //whether it is hitting a wall
    private float WallHitTimer; //timer for keeping wall hit status

    // Combat variables
    [SerializeField]
    private float OptimalRange;     //optimal firing range

    [SerializeField]
    private float AggroRadius;      //Distance mob will keep following

    [SerializeField]
    private float AttackCooldown;   //Time interval between attacks

    [SerializeField]
    private GameObject Projectile;  //The projectile this enemy fires

    private float AttackTimer;      //Time since last attack
    private bool AggroState;        //Whether enemy is aggroed and chasing player

    //Animations
    private Animator anim;

    public void SetExplosion(GameObject new_explosion)
    {
        explosion = new_explosion;
    }

	// Use this for initialization
	void Start () {
        //Automatically attach the player & various components
        player = GameObject.FindGameObjectWithTag("Player");
        health_controller = GetComponent<HealthController>();
        anim = GetComponent<Animator>();
        AggroState = false;
	}
	
	// Update 
	void LateUpdate () {
        //Die if at zero hp
        if (health_controller.GetCurrentHealth() == 0)
        {
            var explosion_instantiation = (GameObject)Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }

        //Movement
        //call primary movement logic to get movement direction
        Vector3 mvt = ((LMMove) GetComponent("LMMove")).MoveLogic(this, player);

        // If primary movement is zero, run auxiliary movement pattern
        if (mvt.magnitude == 0)
        {
            var aux = (LMAuxMove)GetComponent("LMAuxMove");
            if (aux != null) {
                mvt = aux.AuxMoveLogic(this, player);
            }
        }
        
        //apply the movement
        PrevTime = Time.deltaTime;
        mvt = mvt * speed * PrevTime;
        rigidbody.AddForce(mvt);

        PrevMvt = transform.position - PrevPos;     //save the net amount of movement done
        PrevPos = transform.position;
        transform.forward = mvt; 

        //decrement wall hit timer
        WallHitTimer -= PrevTime;
        if (WallHitTimer < 0)
        {
            WallHitTimer = 0;
            WallHit = false;
        }

        //Perform attack & apply proper facing 
        transform.forward = ((LMAttack)GetComponent("LMAttack")).AttackLogic(this, player);       

        //Apply animations
        Debug.Log(rigidbody.velocity.magnitude);
        if(rigidbody.velocity.magnitude > 0.5) {
			anim.SetFloat("Speed", 5.5f, 0.1f, Time.deltaTime);
		} else {
			anim.SetFloat("Speed", 0f);
		}

	}
  

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Obstacle")
        {
            WallHit = true;
            WallHitTimer = 1.5f;
        }
    }



    #region Get Functions
    // Get Functions
    public GameObject GetProjectile()
    {
        return Projectile;
    }

    public float GetAttackTimer()
    {
        return AttackTimer;
    }

    public float GetAttackCD()
    {
        return AttackCooldown;
    }

    public bool GetWallHit()
    {
        return WallHit;
    }

    public float GetWallHitTimer()
    {
        return WallHitTimer;
    }

    public float GetAggroRadius()
    {
        return AggroRadius;
    }

    public Vector3 GetPrevMvt()
    {
        return PrevMvt;
    }

    public float GetPrevTime() {
        return PrevTime;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetOptimalRange()
    {
        return OptimalRange;
    }
    #endregion

    #region Set Functions
    public void SetAttackTimer(float timer)
    {
        AttackTimer = timer;
    }

    public void SetAggroState(bool state)
    {
        AggroState = state;
    }

    #endregion
}
