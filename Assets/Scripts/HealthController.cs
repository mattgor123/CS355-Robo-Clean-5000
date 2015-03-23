using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	[SerializeField] private float max_health;		  // Maximum possible health
	[SerializeField] private float regeneration_rate; // The regeneration rate of health
    [SerializeField]
    private AudioSource death_sound; //The audio file to play when they die
	private float current_health;
    private bool has_died = false;

	void Awake() {
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
			//We're dead
            current_health = 0;
            if (death_sound != null && has_died == false)
            {
                has_died = true;
                death_sound.Play();
            }
			return true;
		}

		return false;
	}
}
