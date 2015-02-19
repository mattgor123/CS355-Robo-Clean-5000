using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	[SerializeField] private float max_health;		  // Maximum possible health
	[SerializeField] private float regeneration_rate; // The regeneration rate of health

	private float current_health;

	void Start () {
		current_health = max_health;
	}
	
	void Update () {
		if(current_health > 0) {
			current_health += regeneration_rate * Time.deltaTime;

			if(current_health > max_health) {
				current_health = max_health;
			}
		}
	}

	public float GetCurrentHealth () {
		return current_health;
	}

	public bool ChangeHealth (float change) {
		current_health += change;

		if(current_health > max_health) {
			current_health = max_health;
		}
		else if(current_health <= 0) {
			current_health = 0;

			return true;
		}

		return false;
	}
}
