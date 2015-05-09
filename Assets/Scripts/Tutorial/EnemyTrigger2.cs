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
			Instantiate(enemy2, spawn_point - new Vector3(1, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(1, change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point - new Vector3(2, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(2, change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point - new Vector3(3, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(3, change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point - new Vector3(4, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(4, change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point - new Vector3(5, -change_in_height, 5.0f), Quaternion.identity);
			Instantiate(enemy2, spawn_point + new Vector3(5, change_in_height, 5.0f), Quaternion.identity);
			var log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();
			log.PassMessage("Watch your health!\nDon't let it run out!\n");
			triggered = true;
            Destroy(gameObject);
		}
	}
}
