using UnityEngine;
using System.Collections;

public class EnergyBall : MonoBehaviour {

    private float Timer = 0f;       //Life timer
    private float TTL = 5.0f;       //Time to live: how long until energy ball dissipates
    private float TTT = 2.0f;       //Time to track: how long after firing for tracking to activate
    private float TTB = 2.5f;       //Duration of burst sequence
    private float TTBD = 2.0f;      //Time to burst damage 

    private float MaxSpeed = 3.0f;  //Maximum speed
    private float Accel = 300f;     //Acceleration
    private float Damage = -50f;    //Damage
    private float BurstRadius = 1.5f;// Burst damage radius

    private bool bursting = false;  //whether it is bursting (at end of life/on contact)
    private bool damaging = false;   //whether the burst damage is happening

    GameObject Player;              //The player
    Rigidbody RB;                   //The rigidbody
    Transform coreEO;               //core effect object
    Transform sparkEO;              //spark effect object
    Transform burstEO;              //burst effect object

    [SerializeField]
    private Transform Burst;        //Burst effect

    [SerializeField]
    private Transform Core;         //Core effect

    [SerializeField]
    private Transform Spark;        //Spark effect


	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        RB = GetComponent<Rigidbody>();

        //Instantiate core & spark effects
        coreEO = Instantiate(Core);
        coreEO.position = this.transform.position;
        coreEO.eulerAngles = new Vector3(0f, 180f, 0f);
        coreEO.SetParent(this.transform);

        sparkEO = Instantiate(Spark);
        sparkEO.position = this.transform.position;
        sparkEO.eulerAngles = new Vector3(0f, 0f, 0f);
        sparkEO.SetParent(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;

        
        //Destroy on burst sequence completion
        if (Timer > TTB && bursting)        
            GameObject.Destroy(gameObject);
        
        //At point of burst damage application, expand collider size
        if (Timer > TTBD && bursting)
        {
            GetComponent<SphereCollider>().radius = BurstRadius;
            damaging = true;
        }

        //If lifetime expired, begin burst sequence
        if (Timer > TTL)
            StartBursting();

        //Only perform movement if not bursting
        if (!bursting)
        {
            //If tracking is active, follow player
            if (Timer > TTT)
            {
                Vector3 mvt = Player.transform.position - this.transform.position;
                mvt.Normalize();
                RB.AddForce(mvt * Accel * Time.deltaTime);
            }

            // Cap speed and face in direction of movement
            transform.forward = RB.velocity;
            if (RB.velocity.magnitude > MaxSpeed)
            {
                RB.velocity = RB.velocity.normalized * MaxSpeed;
            }
        }

	}

    public void SetInitialVelocity(Vector3 direction)
    {
        RB.velocity = direction.normalized * MaxSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        //Start bursting on hitting a thing
        StartBursting();
        
        //Deal damage on initial contact; hits enemies too
        GameObject OGO = other.gameObject;
        if ((OGO.tag == "Player") || (OGO.tag == "Enemy"))
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage);
        }  
    }

    void OnTriggerStay(Collider other)
    {
        //During damage portion of burst, deal continuous damage
        if (damaging)
        {
            GameObject OGO = other.gameObject;
            if ((OGO.tag == "Player") || (OGO.tag == "Enemy"))
            {
                HealthController HC = OGO.GetComponent<HealthController>();
                HC.ChangeHealth(Damage*0.1f);
            }  
        }
    }


    //Starts burst sequence if not started
    private void StartBursting()
    {
        if (!bursting)
        {
            bursting = true;
            Timer = 0;

            //destroy core & spark
            GameObject.Destroy(coreEO.gameObject);
            GameObject.Destroy(sparkEO.gameObject);

            //instantiate burst
            burstEO = Instantiate(Burst);
            burstEO.position = this.transform.position;
            burstEO.eulerAngles = new Vector3(0f, 0f, 0f);
            burstEO.SetParent(this.transform);

            //halt movement
            RB.velocity = new Vector3(0f, 0f, 0f);
        }
    }
}
