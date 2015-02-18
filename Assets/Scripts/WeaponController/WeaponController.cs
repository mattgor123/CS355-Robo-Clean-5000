using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	[SerializeField] private float speed;         // The speed of the bullets exiting the gun
	[SerializeField] private float delay;		  // The delay between shots
	[SerializeField] private float cleanup_delay; // The amount of time to wait before deleting bullets
	[SerializeField] private Transform muzzle;    // The location of the muzzle
	[SerializeField] private GameObject bullet;   // The bullet prefab -- must have a Rigidbody

	private bool firing = false;
	private Quaternion bullet_rotation = new Quaternion(0.7f, 0, 0, 0.7f);
	
	void Update () {
		if(firing) {
			StartCoroutine("Fire");
		}
	}

	IEnumerator Fire () {
		yield return new WaitForSeconds(delay);

		var instantiated_bullet = (GameObject) Instantiate(bullet, muzzle.position, muzzle.rotation * bullet_rotation);

		instantiated_bullet.rigidbody.velocity = muzzle.TransformDirection(Vector3.forward * speed);

		var bullet_controller = instantiated_bullet.GetComponent<BulletController>();

		bullet_controller.SetCleanupDelay(cleanup_delay);

		StopCoroutine("Fire");
	}

	public void StartFiring () {
		firing = true;
	}

	public void StopFiring () {
		firing = false;
	}
}
