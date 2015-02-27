using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	private float damage;
	private float cleanup_delay;
	private float creation_time;

    private bool source_player;   //whether this is fired by the player (and hits enemies)

	private void Start () {
		creation_time = Time.time;
	}

	private void Update () {
		if(Time.time - creation_time > cleanup_delay) {
			Destroy(gameObject);
		}
	}

    public void SetSource(bool is_player)
    {
        source_player = is_player;
    }

	public void SetDamage (float new_damage) {
		damage = new_damage;
	}

	public void SetCleanupDelay (float delay) {
		cleanup_delay = delay;
	}

	void OnCollisionEnter(Collision collision) {
        //Player-fired bullets do not hit the player
        if (source_player && collision.gameObject.tag == "Player")        
            return;        

        //Enemy-fired bullets do not hit enemies
        if (!source_player && collision.gameObject.tag != "Player")
            return;

		var victim_health = collision.gameObject.GetComponent<HealthController>();
		if(victim_health != null) {
			victim_health.ChangeHealth(-damage);
		}
        Destroy(gameObject);
	}
}
