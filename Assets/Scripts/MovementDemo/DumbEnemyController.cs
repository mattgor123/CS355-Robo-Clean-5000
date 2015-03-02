using UnityEngine;
using System.Collections;

public class DumbEnemyController : MonoBehaviour {

	private GameObject explosion;
	private Transform player_transform;
	private EnemySpawner enemy_spawner;
	private MovementController movement_controller;
	private HealthController health_controller;

	private void Start () {
		movement_controller = GetComponent<MovementController>();
		health_controller = GetComponent<HealthController>();
	}

	public void SetExplosion (GameObject new_explosion) {
		explosion = new_explosion;
	}

	public void SetPlayerTransform (Transform transform) {
		player_transform = transform;
	}

	public void SetEnemySpawner (EnemySpawner spawner) {
		enemy_spawner = spawner;
	}

	private void FixedUpdate () {
		movement_controller.UpdateMovement(1.0f, 0, true);
		movement_controller.LookAt(player_transform.position);
	}

	private void Update () {
		if(health_controller.GetCurrentHealth() == 0) {
			var explosion_instantiation = (GameObject) Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
			enemy_spawner.DecrementCurrentEnemies();
		}
	}
}