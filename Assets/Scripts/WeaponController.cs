using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	[SerializeField] private float speed;         // The speed of the bullets exiting the gun
	[SerializeField] private float damage;        // The damage of each bullet
	[SerializeField] private float delay;		  // The delay between shots
	[SerializeField] private float cleanup_delay; // The amount of time to wait before deleting bullets
	[SerializeField] private Transform muzzle;    // The location of the muzzle
	[SerializeField] private GameObject bullet;   // The bullet prefab -- must have a Rigidbody

	private bool firing = false;
	private Quaternion bullet_rotation = new Quaternion(0.7f, 0, 0, 0.7f);
	private float last_fired = 0;
	
	private void Update () {
		if(firing && Time.time - last_fired > delay) {
			Fire();
		}
	}

	private void Fire () {
		var instantiated_bullet = (GameObject) Instantiate(bullet, muzzle.position, muzzle.rotation * bullet_rotation);
		instantiated_bullet.rigidbody.velocity = muzzle.TransformDirection(Vector3.forward * speed);
		var bullet_controller = instantiated_bullet.AddComponent<BulletController>();
		bullet_controller.SetDamage(damage);
		bullet_controller.SetCleanupDelay(cleanup_delay);
		last_fired = Time.time;
	}

	public void StartFiring () {
		firing = true;
	}

	public void StopFiring () {
		firing = false;
	}

	public bool IsFiring() {
		return firing;
	}
}
