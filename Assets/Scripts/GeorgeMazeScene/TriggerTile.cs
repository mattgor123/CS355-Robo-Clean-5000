using UnityEngine;
using System.Collections;

public class TriggerTile : MonoBehaviour {
    private bool TriggerStatus;     //Used to check whether trigger is active (player stepping on it)
    private float TriggerDuration;  //Used to hold duration of trigger activation 


	// Use this for initialization
	void Start () {
        TriggerStatus = false;
        TriggerDuration = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (TriggerStatus == true)
        {   //Increment duration that trigger has been on
            TriggerDuration += Time.deltaTime;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerStatus = true;
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TriggerStatus = false;
        }   
    }

    //Sets the size of the trigger tile
    public void SetTileSize(Vector3 size)
    {
        transform.localScale = size;
    }

    //Gets the trigger status
    public bool GetTriggerStatus()
    {
        return TriggerStatus;
    }

    //Gets the trigger duration
    public float GetTriggerDuration()
    {
        return TriggerDuration;
    }
}
