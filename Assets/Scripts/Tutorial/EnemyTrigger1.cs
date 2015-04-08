using UnityEngine;
using System.Collections;

public class EnemyTrigger1 : MonoBehaviour {

	[SerializeField] private GameObject enemy;
	[SerializeField] private Vector3 spawn_point;

	private bool triggered = false;

	void OnCollisionEnter(Collision collision) {
		if(!triggered) {
			Instantiate(enemy, spawn_point, Quaternion.identity);
			var log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();
			log.PassMessage("Click the left mouse\nbutton to shoot\n");
			triggered = true;
		}
	}
}
