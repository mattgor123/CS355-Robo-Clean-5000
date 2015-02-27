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

    public void SetExplosion(GameObject new_explosion)
    {
        explosion = new_explosion;
    }



	// Use this for initialization
	void Start () {
        //Automatically attach the player
        player = GameObject.FindGameObjectWithTag("Player");
        health_controller = GetComponent<HealthController>();
        //SetExplosion(explosion);
        AggroState = false;
	}
	
	// Update 
	void LateUpdate () {
        if (health_controller.GetCurrentHealth() == 0)
        {
            var explosion_instantiation = (GameObject)Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }

        Vector3 mvt = ((LMMove) GetComponent("LMMove")).MoveLogic(this, player);

        // If primary movement is zero, run auxiliary movement pattern
        if (mvt.magnitude == 0)
        {
            var aux = (LMAuxMove)GetComponent("LMAuxMove");
            if (aux != null) {
                mvt = aux.AuxMoveLogic(this, player);
            }
        }
        
        PrevTime = Time.deltaTime;
        mvt = mvt * speed * PrevTime;
        rigidbody.AddForce(mvt);

        PrevMvt = transform.position - PrevPos;     //save the net amount of movement done
        PrevPos = transform.position;
        
        WallHitTimer -= PrevTime;
        if (WallHitTimer < 0)
        {
            WallHitTimer = 0;
            WallHit = false;
        }

        //Perform attack
        ((LMAttack)GetComponent("LMAttack")).AttackLogic(this, player);

        UpdateRotation();

	}

    private void UpdateRotation()
    {
        var forward = transform.forward;
        var angle = 90 - Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);    
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
