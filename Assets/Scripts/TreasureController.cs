using UnityEngine;
using System.Collections;

public class TreasureController : MonoBehaviour {

    [SerializeField]
    private int max_ammo;
    [SerializeField]
    private int max_health;
    [SerializeField]
    private AudioClip pickup_sound;

	private void OnCollisionEnter (Collision collision) {
		var weapon_backpack = collision.collider.GetComponent<WeaponBackpackController>();
        var inventory = collision.collider.GetComponent<InventoryController>();
        GameObject nlog = GameObject.FindWithTag("Log");
        string message = "";
        LogScript log = nlog.GetComponent<LogScript>(); 
		if(weapon_backpack != null) {
            if (pickup_sound != null)
            {
                AudioSource.PlayClipAtPoint(pickup_sound, transform.position);
            }
			var found_ammo = Random.Range(0, max_ammo + 1);
			weapon_backpack.ChangeAmmo(found_ammo);
			var player = GameObject.FindGameObjectWithTag("StartingWeapons");
			string pickupMessage = player.GetComponent<StartingWeapons>().RandomWeaponPickup();
			//Destroy(gameObject);
            gameObject.SetActive(false);
            message += "Collected " + found_ammo + " ammo\n";
            message += pickupMessage + "\n";

		}
        var health = collision.collider.GetComponent<HealthController>();
        if (health != null)
        {
            if (pickup_sound != null)
            {
                AudioSource.PlayClipAtPoint(pickup_sound, transform.position);
            }
            var collected_health = Random.Range(20, 76);
            health.ChangeHealth(collected_health);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            message += "Regained " + collected_health + " health\n";
        }
        if (inventory != null)
        {
            
            int currentLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getCurrentFloor();
            if (!inventory.hasKey(currentLevel + 1)) //&& Random.Range(0, 1f) < 0.25f
            {
                inventory.collectKey(currentLevel + 1); //because array of keys is 0-based, this unlocks the floor above
                message += "Collected B" + (currentLevel + 1) + " Access Card\n";
            }
        }
        Debug.Log(message);
        log.PassMessage(message);

	}
}
