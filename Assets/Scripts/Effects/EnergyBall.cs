using UnityEngine;
using System.Collections;

public class EnergyBall : MonoBehaviour {

    //Timers
    private float Timer = 0f;       //Life timer
    [SerializeField]
    private float TTL = 5.0f;       //Time to live: how long until energy ball dissipates
    [SerializeField]
    private float TTT = 2.0f;       //Time to track: how long after firing for tracking to activate
    [SerializeField]
    private float TTBD = 2.0f;      //Time to burst damage 
    [SerializeField]
    private float TBD = 0.5f;       //Time of burst damage existence

    private float BDmodifier = 1.0f;

    //Other params
    [SerializeField]
    private float MaxSpeed = 3.0f;  //Maximum speed
    [SerializeField]
    private float Accel = 300f;     //Acceleration
    [SerializeField]
    private float Damage = -50f;    //Damage
    [SerializeField]
    private float BurstRadius = 1.5f;// Burst damage radius

    private bool bursting = false;  //whether it is bursting (at end of life/on contact)
    private bool damaging = false;   //whether the burst damage is happening

    private GameObject Player;              //The player
    private Rigidbody RB;                   //The rigidbody
    private Transform coreEO;               //core effect object
    private Transform sparkEO;              //spark effect object
    private Transform burstEO;              //burst effect object

    [SerializeField]
    private Transform Burst;        //Burst effect

    [SerializeField]
    private Transform Core;         //Core effect

    [SerializeField]
    private Transform Spark;        //Spark effect


	// Use this for initialization
	void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player");
        RB = GetComponent<Rigidbody>();
        BDmodifier = TBD / 0.5f;

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

        //Keep from hitting floor prematurely
        if (transform.position.y < 1)
            transform.position += new Vector3(0f, 0.1f, 0f);
        
        //Destroy on burst sequence completion
        if (Timer > (TTBD + TBD) && bursting)        
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

            //else just keep going forward
            else
                RB.AddForce(transform.forward * Accel * Time.deltaTime);
            

            // Cap speed and face in direction of movement
            if (RB.velocity.magnitude != 0) 
                transform.forward = RB.velocity.normalized;
            if (RB.velocity.magnitude > MaxSpeed)
            {
                RB.velocity = RB.velocity.normalized * MaxSpeed;
            }
        }

	}

    public void SetInitDirection(Vector3 direction)
    {
        RB.AddForce(direction * MaxSpeed);
        transform.forward = direction;
        
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

            //Adjust duration of burst to fit TBD
            //Default durations:  Sphere 0.3,  Circle 0.5, Dust 3
            ParticleSystem[] PSs = burstEO.GetComponentsInChildren<ParticleSystem>();
            
            
            foreach (ParticleSystem p in PSs)
            {
                if (!(p.gameObject.name == burstEO.name))
                {
                    p.startLifetime *= BDmodifier;
                }
            }

            //halt movement
            RB.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    public void SetBurstDuration(float duration)
    {
        TBD = duration;
        BDmodifier = TBD / 0.5f;
    }
}
