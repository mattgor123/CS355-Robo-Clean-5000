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
    private bool justSpawned;

	// Use this for initialization
	void Awake () {
        boss = GameObject.FindGameObjectWithTag("Broodmother");
        health = GetComponent<HealthController>();
        backpack = GetComponent<WeaponBackpackController>();
        anim = GetComponent<Animator>();
        timeSinceSpawn = 0;
        animTimer = 0;
        justSpawned = false;

	}
	

	// Update is called once per frame
	void Update () {
        timeSinceSpawn += Time.deltaTime;
        animTimer += Time.deltaTime;

        if (firstPhase && health.GetCurrentHealth() < 1600.0)
        {
            secondPhase = true;
            timeSinceSpawn = 0; //timer needs to begin from the moment secondPhase begins
            firstPhase = false;
        }

        if (secondPhase && timeSinceSpawn >= animStart && !justSpawned)
        {
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
        }



	}

    


    public void WakeUp()
    {
        firstPhase = true;
        if (anim != null) anim.SetBool("PlayerInRoom", true);
    }
}
