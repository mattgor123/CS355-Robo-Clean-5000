using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {

    private bool shooting;
    private GameObject player;

	// Use this for initialization
	void Start () {
        shooting = false;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale == 0) return;

        if (shooting)
        {

        }
	
	}

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;
        //gameObject.transform.rotation = player.transform.position - gameObject.transform.position;
        Vector3 target = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        gameObject.transform.LookAt(target);
    }
}
