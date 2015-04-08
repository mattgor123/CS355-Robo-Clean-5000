using UnityEngine;
using System.Collections;

public class EnemyTrigger1 : MonoBehaviour {

	[SerializeField] private GameObject enemy;
	[SerializeField] private Vector3 spawn_point;
	[SerializeField] private GameObject treasure;
	[SerializeField] private int extra_treasure;

	private bool triggered = false;
	private GameObject enemy_instance;

	void OnCollisionEnter(Collision collision) {
		if(!triggered) {
			enemy_instance = Instantiate(enemy, spawn_point, Quaternion.identity) as GameObject;
			triggered = true;
		}
	}

	void Update () {
		if(triggered) {
			if(enemy_instance.GetComponent<HealthController>().GetCurrentHealth() == 0) {
				for(int i = 0; i < extra_treasure; ++i) {
					Instantiate(treasure, enemy_instance.transform.position, enemy_instance.transform.rotation);
				}
			}
		}
	}
}
