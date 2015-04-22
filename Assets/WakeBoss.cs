using UnityEngine;
using System.Collections;

public class WakeBoss : MonoBehaviour {

    private bool trigger;
    private BroodmotherController boss;


	// Use this for initialization
	void Start () {
        boss = GameObject.FindGameObjectWithTag("Broodmother").GetComponent<BroodmotherController>();
        trigger = false;
	}

    void OnTriggerEnter(Collider other)
    {

        if (!trigger)
        {

            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {
                trigger = true;
                boss.WakeUp();
//                Debug.Log("Wake up Broodmother");

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
