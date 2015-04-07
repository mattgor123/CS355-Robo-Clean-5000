using UnityEngine;
using System.Collections;

public class TurretTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<WeaponBackpackController>().StartFiring();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<WeaponBackpackController>().StopFiring();
        }
    }
}
