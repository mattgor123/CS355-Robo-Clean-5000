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

    private bool AggroState;        //Whether enemy is aggroed and chasing player

	// Use this for initialization
	void Start () {
        //Automatically attach the player
        player = GameObject.FindGameObjectWithTag("Player");
        AggroState = false;
	}
	
	// Update 
	void LateUpdate () {
        Vector3 mvt = ((LMMove) GetComponent("LMMove")).MoveLogic(this, player);

        // If primary movement is zero, run auxiliary movement pattern
        if (mvt.magnitude == 0)
        {
            mvt = ((LMAuxMove)GetComponent("LMAuxMove")).AuxMoveLogic(this, player);
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
    public void SetAggroState(bool state)
    {
        AggroState = state;
    }

    #endregion
}
