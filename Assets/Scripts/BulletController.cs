using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	[SerializeField] private float damage;

	private float cleanup_delay;
	private float creation_time;

	private void Start () {
		creation_time = Time.time;
	}

	private void Update () {
		if(Time.time - creation_time > cleanup_delay) {
			Destroy(gameObject);
		}
	}

	public void SetCleanupDelay (float delay) {
		cleanup_delay = delay;
	}

	void OnCollisionEnter(Collision collision) {
		var victim_health = collision.gameObject.GetComponent<HealthController>();

		if(victim_health != null) {
			victim_health.ChangeHealth(-damage);
		}
	}
}
