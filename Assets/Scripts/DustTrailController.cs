using UnityEngine;
using System.Collections;

public class DustTrailController : MonoBehaviour {

    GameObject Player;
    Rigidbody PlayerRB;
    ParticleSystem PS;
    float yrot;
    float xrot = -30f;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerRB = Player.GetComponent<Rigidbody>();
        PS = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        //No dust trail when stationary
        if (PlayerRB.velocity.magnitude < 1) {
            PS.Stop();
        }

        //Otherwise, have trail in direction opposite to movement
        else
        {
            PS.Play();
            Vector3 RDir = -1*PlayerRB.velocity.normalized;
            transform.forward = RDir;
            transform.eulerAngles = new Vector3(xrot, transform.eulerAngles.y, 0f);
        }

	}
}
