using UnityEngine;
using System.Collections;

public class FireBomb : MonoBehaviour {

    //Timers
    private float Timer = 0f;       //Life timer
    [SerializeField]
    private float TTBD = 2.0f;      //Time to burst damage 
    [SerializeField]
    private float TBD = 1.5f;       //Time of burst damage existence

    [SerializeField]
    private float Damage = -50f;    //Damage
    [SerializeField]
    private float BurstRadius = 2.5f;// Burst damage radius

    private bool bursting = false;  //whether it is bursting (at end of life/on contact)


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;

        //Destroy on burst sequence completion
        if (Timer > (TTBD + TBD) && bursting)
            GameObject.Destroy(gameObject);

        //At point of burst, start burst sequence
        if (Timer > TTBD)        
            StartBurst();        
	}



    private void StartBurst()
    {
        if (!bursting)
        {
            bursting = true;
            GetComponent<SphereCollider>().radius = BurstRadius;
        }
    }


    //Deal damage on intial contact
    void OnTriggerEnter(Collider other)
    {
        GameObject OGO = other.gameObject;
        if ((OGO.tag == "Player") || (OGO.tag == "Enemy"))
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage);
        }
    }

    //Deal continuous damage on stay
    void OnTriggerStay(Collider other)
    {
        if (bursting)
        {
            GameObject OGO = other.gameObject;
            if ((OGO.tag == "Player") || (OGO.tag == "Enemy"))
            {
                HealthController HC = OGO.GetComponent<HealthController>();
                HC.ChangeHealth(Damage * 0.1f);
            }
        }
    }

    public void SetDamage(float dmg)
    {
        Damage = dmg;
    }
}
