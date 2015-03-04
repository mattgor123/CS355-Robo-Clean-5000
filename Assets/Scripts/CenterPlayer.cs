using UnityEngine;
using System.Collections;

public class CenterPlayer : MonoBehaviour {

	void Start () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0, 0.1f, 0);
        }
	
	}
}
