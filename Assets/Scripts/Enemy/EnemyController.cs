﻿using UnityEngine;
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

    [SerializeField]
    private float SpeedLimit;   //maximum velocity magnitude

    public GameObject explosion;
    public GameObject treasure_chest;

    private HealthController health_controller;
    private Vector3 PrevPos;    //previous position
    private Vector3 PrevMvt;    //distance moved in previous movement step
    private float PrevTime;     //the previous time interval
    private bool WallHit;       //whether it is hitting a wall
    private float WallHitTimer; //timer for keeping wall hit status

    private Rigidbody RBody;

    // Combat variables
    [SerializeField]
    private float OptimalRange;     //optimal firing range

    [SerializeField]
    private float AggroRadius;      //Distance mob will keep following

    [SerializeField]
    private float AttackCooldown;   //Time interval between attacks

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
        RBody = GetComponent<Rigidbody>();

        //transform.position -= new Vector3(0f, transform.position.y, 0f);
	}
	
	// Update 
	void LateUpdate () {
        if (Time.timeScale == 0) return;

        //Die if at zero hp
        if (health_controller.GetCurrentHealth() == 0)
        {
            anim.SetBool("Dead", true);
            // need delay so enemy dying animations can show
            var explosion_instantiation = (GameObject)Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            var treasure_instantiation = (GameObject)Instantiate(treasure_chest, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            StageBuilder.EnemyDied();
        }

        //Movement
        //call primary movement logic to get movement direction
        Vector3 mvt = ((LMMove) GetComponent("LMMove")).MoveLogic(this, player);

        // If primary movement is zero, and not aggroed, run auxiliary movement pattern
        if (mvt.magnitude == 0 && !AggroState)
        {
            var aux = (LMAuxMove)GetComponent("LMAuxMove");
            if (aux != null) {
                mvt = aux.AuxMoveLogic(this, player);
            }
        }

        //apply the movement
        PrevTime = Time.deltaTime;
        mvt = mvt * speed * PrevTime;
        GetComponent<Rigidbody>().AddForce(mvt);

        PrevMvt = transform.position - PrevPos;     //save the net amount of movement done
        PrevPos = transform.position;

        //Face in the direction of movement
        if (RBody.velocity.magnitude > 0)
            transform.forward = RBody.velocity;
        
        //Do not exceed maximum speed
        if (RBody.velocity.magnitude > SpeedLimit)
        {
            RBody.velocity = RBody.velocity.normalized*SpeedLimit;
        }

        //decrement wall hit timer
        WallHitTimer -= PrevTime;
        if (WallHitTimer < 0)
        {
            WallHitTimer = 0;
            WallHit = false;
        }

        //Perform attack & face player if aggroed
        if (AggroState) { 
           
            transform.forward = ((LMAttack)GetComponent("LMAttack")).AttackLogic(this, player);
            transform.LookAt(player.transform);
        }

        //Apply animations
        if(RBody.velocity.magnitude > 0) {
			anim.SetBool("Moving", true);
		} else {
			anim.SetBool("Moving", false);
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

    //If player flashlight is off, aggro radius is at 75%
    public float GetAggroRadius()
    {
        if (player.GetComponentInChildren<FlashlightController>().GetActive())
            return AggroRadius;
        else
            return AggroRadius * 0.75f;
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