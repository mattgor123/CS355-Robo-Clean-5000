using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] private Transform player_transform;
	[SerializeField] private GameObject enemy;
	[SerializeField] private int enemy_count;

	private int current_enemy_count;
	private GUIText enemy_count_text;

	private void Start () {
		for(int i = 0; i < enemy_count; ++i) {
			SpawnEnemy();
		}
		current_enemy_count = enemy_count;
		enemy_count_text = GetComponent<GUIText>();
		enemy_count_text.text = enemy_count.ToString();
	}

	private void SpawnEnemy () {
		var x = Random.Range(-100.0f, 100.0f);
		var z = Random.Range(-100.0f, 100.0f);
		var position = new Vector3(x, 1.0f, z);
		var enemy_instantiation = (GameObject) Instantiate(enemy, position, new Quaternion());
		var dumb_enemy_controller = enemy_instantiation.GetComponent<DumbEnemyController>();
		dumb_enemy_controller.SetPlayerTransform(player_transform);
		dumb_enemy_controller.SetEnemySpawner(this);
	}

	public void DecrementCurrentEnemies () {
		--current_enemy_count;
	}

	private void Update () {
		enemy_count_text.text = current_enemy_count.ToString();
	}
}