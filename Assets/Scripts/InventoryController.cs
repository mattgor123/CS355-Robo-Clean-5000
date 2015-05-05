using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {

    private bool[] keys = new bool[] {true, false, false, false, false, false, false, false, false, false};


    void Awake()
    {


    }

	// Use this for initialization
	void Start () {
	}


	
	// Update is called once per frame
	void Update () {
	
	}

    //adds the key to inventory
    public void collectKey(int floor) //gives floor access card. but array is 0-based, so mind that!
    { 
        keys[floor] = true;
    }

    public bool hasKey(int floor) //returns if specified key is already in inventory
    {
        return keys[floor];
    }


}
