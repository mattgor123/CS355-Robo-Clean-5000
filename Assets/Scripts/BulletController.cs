using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	[SerializeField] private float damage;

	private float cleanup_delay;

	void Update () {
		if(!IsInvoking("SelfDestruct")) {
			Invoke("SelfDestruct", cleanup_delay);
		}
	}

	void SelfDestruct () {
		Destroy(gameObject);
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
