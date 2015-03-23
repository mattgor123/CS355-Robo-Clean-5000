using UnityEngine;
using System.Collections;

public class TreasureController : MonoBehaviour {

	[SerializeField] private int max_ammo;
    [SerializeField]
    private AudioClip pickup_sound;

	private void OnCollisionEnter (Collision collision) {
		var weapon_backpack = collision.collider.GetComponent<WeaponBackpackController>();
		if(weapon_backpack != null) {
            if (pickup_sound != null)
            {
                AudioSource.PlayClipAtPoint(pickup_sound, transform.position);
            }
			var found_ammo = Random.Range(0, max_ammo + 1);
			weapon_backpack.ChangeAmmo(found_ammo);
			var player = GameObject.FindGameObjectWithTag("StartingWeapons");
			string pickupMessage = player.GetComponent<StartingWeapons>().RandomWeaponPickup();
			Destroy(gameObject);
			GameObject nlog = GameObject.FindWithTag("Log");
			NotificationLog log = nlog.GetComponent<NotificationLog>(); 
			log.PassMessage("Got " + found_ammo + " ammo, " + pickupMessage);
		}
	}
}
