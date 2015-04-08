using UnityEngine;
using System.Collections;

public class LeaveTutorial : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		Application.LoadLevel("Game");
	}
}
