using UnityEngine;
using System.Collections;

public class TutorialTreasureController : MonoBehaviour {

	[SerializeField] private int max_ammo;
    [SerializeField] private int max_health;
    [SerializeField] private AudioClip pickup_sound;

	private void OnCollisionEnter (Collision collision) {
		var weapon_backpack = collision.collider.GetComponent<WeaponBackpackController>();
        GameObject nlog = GameObject.FindWithTag("Log");
        string message = "";
        NotificationLog log = nlog.GetComponent<NotificationLog>(); 
		if(weapon_backpack != null) {
			if (pickup_sound != null)
            {
                AudioSource.PlayClipAtPoint(pickup_sound, transform.position);
            }
			var found_ammo = Random.Range(0, max_ammo + 1);
			weapon_backpack.ChangeAmmo(found_ammo);
			var player = GameObject.FindGameObjectWithTag("StartingWeapons");
			string pickupMessage = player.GetComponent<StartingWeapons>().PickupAll();
			Destroy(gameObject);
		}
        var health = collision.collider.GetComponent<HealthController>();
        if (health != null)
        {
            var collected_health = Random.Range(50, 76);
            health.ChangeHealth(collected_health);
            Destroy(gameObject);
        }
        log.PassMessage("You just picked up guns.\nPress r to toggle through\n");
	}
}
