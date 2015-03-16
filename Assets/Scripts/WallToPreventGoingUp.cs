using UnityEngine;
using System.Collections;

public class WallToPreventGoingUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		PlayerController playerController = player.GetComponent<PlayerController>();
		
		if (playerController.getCurrentFloor () != 0) {
			Destroy(gameObject);
			
		}
	}

}
