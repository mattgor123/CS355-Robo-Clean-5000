using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	[SerializeField] protected float max_health;		  // Maximum possible health
	[SerializeField] protected float regeneration_rate; // The regeneration rate of health
    [SerializeField]
    private AudioSource death_sound; //The audio file to play when they die
	protected float current_health;
    protected bool has_died = false;

    private ShieldController Shield;
    private bool HasShield = false;

	void Awake() {
		current_health = max_health;
        Shield = GetComponentInChildren<ShieldController>();
        if (Shield != null)
            HasShield = true;
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
        float net = change;     
        
        //Block damage with shields if applicable
        if (HasShield && net < 0)
        {
            net = Shield.Block(net);
        }
		current_health += net;

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

	public float GetMaxHealth() {
		return max_health;
	}
}
