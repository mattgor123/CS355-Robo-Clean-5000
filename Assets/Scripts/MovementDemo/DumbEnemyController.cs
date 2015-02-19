using UnityEngine;
using System.Collections;

public class DumbEnemyController : MonoBehaviour {

	private Transform player_transform;
	private EnemySpawner enemy_spawner;
	private MovementController movement_controller;
	private HealthController health_controller;

	private void Start () {
		movement_controller = GetComponent<MovementController>();
		health_controller = GetComponent<HealthController>();
	}

	public void SetPlayerTransform (Transform transform) {
		player_transform = transform;
	}

	public void SetEnemySpawner (EnemySpawner spawner) {
		enemy_spawner = spawner;
	}

	private void FixedUpdate () {
		var pointer = new Vector3(player_transform.position.x - transform.position.x, 0, player_transform.position.z - transform.position.z);
		if(Vector3.Dot(pointer, transform.forward) < 0) {
			movement_controller.UpdateMovement(-0.7f, 0);
		}
		else {
			movement_controller.UpdateMovement(1.0f, 0);
		}
		var angle = Vector3.Angle(pointer.normalized, transform.forward);
		if(angle > 180.0f) {
			angle -= 360.0f;
		}
		if(angle > 10.0f) {
			movement_controller.UpdateRotation(new Vector3(0, angle, 0));
		}
	}

	private void Update () {
		if(health_controller.GetCurrentHealth() == 0) {
			Destroy(gameObject);
			enemy_spawner.DecrementCurrentEnemies();
		}
	}
}
