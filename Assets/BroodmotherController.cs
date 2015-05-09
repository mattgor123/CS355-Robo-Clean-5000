using UnityEngine;
using System.Collections;

public class BroodmotherController : MonoBehaviour {

    [SerializeField]
    private GameObject spiders;
    private bool firstPhase;
    private bool secondPhase;
    private bool thirdPhase;
    private GameObject boss;
    private HealthController health;
    private WeaponBackpackController backpack;
    private Animator anim;
    private float timeSinceSpawn;
    private float animTimer;
    private float animStart = 4.0f; //When hatching animation begins
    private float spawnRate = 5.0f; //When spider is spawned mid-hatch
    private float animEnd = 6.0f; //When hatching animation is finished
    private float phaseTransition = 2.5f;
    private bool justSpawned;
    private bool keygiven = false;

    private InventoryController IC;
    private PlayerController P;
    private int floor;

	// Use this for initialization
	void Awake () {
        boss = GameObject.FindGameObjectWithTag("Broodmother");
        health = GetComponent<HealthController>();
        backpack = GetComponent<WeaponBackpackController>();
        anim = GetComponent<Animator>();
        timeSinceSpawn = 0;
        animTimer = 0;
        justSpawned = false;
        PauseAI();

        IC = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        P = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        floor = P.getCurrentFloor() + 1;
	}
	

	// Update is called once per frame
	void Update () {
        timeSinceSpawn += Time.deltaTime;
        animTimer += Time.deltaTime;

        if (firstPhase && timeSinceSpawn > phaseTransition)
        {
            ResumeAI();
        }

        if (firstPhase && health.GetCurrentHealth() < 1600.0)
        {
            secondPhase = true;
            timeSinceSpawn = 0; //timer needs to begin from the moment secondPhase begins
            firstPhase = false;
        }

        if (secondPhase && timeSinceSpawn >= animStart && !justSpawned)
        {
            PauseAI();
            anim.SetBool("isHatching", true);

        }
        if (secondPhase && timeSinceSpawn >= spawnRate && !justSpawned)
        {
            Vector3 spawnAngle = boss.transform.position + new Vector3(Random.Range(-2f, 2f), -1, Random.Range(0, 2));
            GameObject.Instantiate(spiders, spawnAngle, Quaternion.identity);
            justSpawned = true;
        }

        if (secondPhase && timeSinceSpawn >= animEnd && justSpawned)
        {
            anim.SetBool("isHatching", false);
            timeSinceSpawn = 0;
            justSpawned = false;
            ResumeAI();

        }

        //Give player key to current floor on death
        if (health.GetCurrentHealth() <= 0 && !IC.hasKey(floor))
        {
            IC.collectKey(floor);
            GameObject.FindGameObjectWithTag("Log").GetComponent<LogScript>().PassMessage("Boss Defeated: Picked up key to next floor");
        }
        


	}

    private void PauseAI()
    {
        GetComponent<LMLongPatrol>().enabled = false;
        GetComponent<LMChase>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
    }

    private void ResumeAI()
    {
        GetComponent<LMLongPatrol>().enabled = true;
        GetComponent<LMChase>().enabled = true;
        GetComponent<EnemyController>().enabled = true;
    }


    public void WakeUp()
    {
        firstPhase = true;
        if (anim != null) anim.SetBool("PlayerInRoom", true);
        timeSinceSpawn = 0;
    }
}
