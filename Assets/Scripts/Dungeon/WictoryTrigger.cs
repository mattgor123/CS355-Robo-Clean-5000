using UnityEngine;
using System.Collections;

public class WictoryTrigger : MonoBehaviour {

    private InventoryController inventory;
    private bool trigger = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!trigger)
        {
            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {
                trigger = true;
                inventory = other.GetComponent<InventoryController>();
                if (inventory.hasKey(9))
                {
                    Application.LoadLevel("Victory");
                }
            }
        }
    }
}
