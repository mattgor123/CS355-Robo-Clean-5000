using UnityEngine;
using System.Collections;

public class EnemyTrigger2 : MonoBehaviour {

	[SerializeField] private GameObject enemy1;
	[SerializeField] private GameObject enemy2;
	[SerializeField] private Vector3 spawn_point;
	[SerializeField] private float change_in_height;

	private bool triggered = false;

	void OnCollisionEnter(Collision collision) {
		if(!triggered) {
			Instantiate(enemy1, spawn_point, Quaternion.identity);
			Instantiate(enemy2, spawn_point - new Vector3(0, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(0, change_in_height, 5.0f), Quaternion.identity);
			triggered = true;
		}
	}
}
