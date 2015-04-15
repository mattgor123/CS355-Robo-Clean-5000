using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	[SerializeField] private float speed;         // The speed of the bullets exiting the gun
	[SerializeField] private float damage;        // The damage of each bullet
	[SerializeField] private float delay;		  // The delay between shots
	
	[SerializeField] private bool is_laser;       // Should the weapon go through enemies
	[SerializeField] private float cleanup_delay; // The amount of time to wait before deleting bullets
	[SerializeField] private int ammo_per_shot;   // The amount of ammo used per shot
	[SerializeField] private Transform muzzle;    // The location of the muzzle
	[SerializeField] private GameObject bullet;   // The bullet prefab -- must have a Rigidbody
    [SerializeField] private AudioSource shot_sound;      // The audio clip to play when bullet is fired

    private bool firing = false;
	private Quaternion bullet_rotation = new Quaternion(0.7f, 0, 0, 0.7f);
	private float last_fired = 0;
    private Transform owner;                        //The owner of the gun
    private WeaponBackpackController backpack_controller; // The backpack that contains this gun

    private void Start()
    {
        //var collider = this.GetComponent<BoxCollider>();
        //collider.enabled = false;
        owner = transform.parent.root;
        backpack_controller = owner.GetComponent<WeaponBackpackController>();        
    }

	private void LateUpdate () {
		if(firing && Time.time - last_fired > delay) {
            // Find the source of the bullet (true for player, false for enemy)
            bool source;            
            if (owner.tag == "Player")            
                source = true;        
            else
                source = false;
			Fire(source);
		}
        //Setup proper weapon position
        transform.position = owner.transform.position + new Vector3(0f, 2.8f, 0f);
        transform.position += owner.transform.forward * 1.5f;
	}

	private void Fire (bool source) {
		if(backpack_controller.HasAmmo(ammo_per_shot)) {
            if (shot_sound != null)
            {
                shot_sound.Play();
            }
			var instantiated_bullet = (GameObject) Instantiate(bullet, muzzle.position, muzzle.rotation * bullet_rotation);
            instantiated_bullet.GetComponent<Rigidbody>().velocity = muzzle.TransformDirection(Vector3.forward * speed);
			var bullet_controller = instantiated_bullet.AddComponent<BulletController>();
			bullet_controller.SetDamage(damage);
			bullet_controller.SetCleanupDelay(cleanup_delay);
			bullet_controller.SetLaser(is_laser);
        	instantiated_bullet.GetComponent<BulletController>().SetSource(source);
			last_fired = Time.time;
        	instantiated_bullet.AddComponent<Light>();
        	instantiated_bullet.GetComponent<Light>().intensity = 0.5f;
        	instantiated_bullet.GetComponent<Light>().range = 1f;
        	if (source)
        	{
        	    instantiated_bullet.GetComponent<Light>().color = Color.green;
        	}
        	else
        	{
        	    instantiated_bullet.GetComponent<Light>().color = Color.cyan;
        	}
        	backpack_controller.ChangeAmmo(-ammo_per_shot);
        }
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

	public void SetBackpackController(WeaponBackpackController new_backpack_controller) {
		backpack_controller = new_backpack_controller;
	}
}
