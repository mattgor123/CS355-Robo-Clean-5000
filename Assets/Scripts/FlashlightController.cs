using UnityEngine;
using System.Collections;

public class FlashlightController : MonoBehaviour {

    private GameObject Player;
    private float height;
    private bool Active;
    private float Strength;
    private Light lt;

	// Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        height = 4f;
        Active = true;
        lt = GetComponent<Light>();
        Strength = lt.intensity; 
    }
	
	// Update is called once per frame
	void Update () {
	    //Face forward based on player
        transform.position = new Vector3(Player.transform.position.x, height, Player.transform.position.z);
        transform.forward = Player.transform.forward;
        transform.position += transform.forward * (0f);
	}

    //Toggle the flashlight
    public void ToggleFlashlight()
    {
        if (Active)
        {
            lt.intensity = 0;
            Active = false;
        }
        else
        {
            lt.intensity = Strength;
            Active = true;

        }
    }
}
