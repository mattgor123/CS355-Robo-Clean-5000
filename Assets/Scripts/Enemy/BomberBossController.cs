using UnityEngine;
using System.Collections;

//Bomber Boss auxiliary logic
public class BomberBossController : MonoBehaviour {

    private Transform Core;
    private HealthController HC;
    private InventoryController IC;
    private PlayerController P;
    private Rigidbody RB;
    private float LaunchDelay = 1.0f;
    private Vector3 Delta = new Vector3(0f, 1f, 0f);
    private int floor;
    private RigidbodyConstraints Constraints;

    //Start with bomber deactivated at rest
	void Awake () {
        Core = gameObject.GetComponent<Transform>();
        PauseAI();
        HC = GetComponent<HealthController>();
        IC = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        P = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        RB = GetComponent<Rigidbody>();
        floor = P.getCurrentFloor() + 1;
        Constraints = RB.constraints;
        RB.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	void Update () {
        if (Core.position.y > 3)
        {
            ResumeAI();
        } else if (LaunchDelay < 0)        
            Core.position += Delta * Time.deltaTime;        
        else 
            LaunchDelay -= Time.deltaTime;

        //Give player key to current floor on death
        if (HC.GetCurrentHealth() <= 0 && !IC.hasKey(floor))
        {
            IC.collectKey(floor);
            GameObject.FindGameObjectWithTag("Log").GetComponent<LogScript>().PassMessage("Boss Defeated: Picked up key to next floor");
        }

        //Destroy if no on the proper boss floor
        if (P.getCurrentFloor() != 6)
        {
            Destroy(gameObject);
        }
        
	}

    private void PauseAI()
    {
        GetComponent<LMLongPatrol>().enabled = false;
        GetComponent<LMStrafingRun>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
    }

    private void ResumeAI()
    {
        GetComponent<LMLongPatrol>().enabled = true;
        GetComponent<LMStrafingRun>().enabled = true;
        GetComponent<EnemyController>().enabled = true;
        RB.constraints = Constraints;
    }

    public void SetLaunchDelay(float dl)
    {
        LaunchDelay = dl;
    }

}


