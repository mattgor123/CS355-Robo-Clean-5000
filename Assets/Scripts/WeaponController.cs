using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private GameObject player;
	private StatisticsRecorderController stats;

    //private List<GameObject> bullets;
    //private int bulletAmount = 10;

    //private ObjectPooling pool;

    private void Start()
    {
        //var collider = this.GetComponent<BoxCollider>();
        //collider.enabled = false;
        owner = transform.parent.root;
        backpack_controller = owner.GetComponent<WeaponBackpackController>();

        //bullets = new List<GameObject>();
        //StartCoroutine(makeBullets());

        //pool = GameObject.Find("ObjectPool").GetComponent<ObjectPooling>();
    }

    /*
    private void Awake()
    {
        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPooling>();
    }
     * */

	private void LateUpdate () {
		if(firing && Time.time - last_fired > delay) {
            bool source = owner.tag == "Player";
			Fire(source);
		}
        //Gun position set to position of Gun Location child object
        transform.position = owner.transform.FindChild("Gun Location").position;
	}

	private void Fire (bool source) {
		if(backpack_controller.HasAmmo(ammo_per_shot)) {
			//start code for stat tracking
			if (player == null) {
				player = GameObject.FindGameObjectWithTag("Player");
			}
			if (player != null && stats == null) {
				stats = player.GetComponent<StatisticsRecorderController>();
			}
			if (stats != null) {
				if(source) {
					stats.fireWeapon(backpack_controller.GetWeaponName());
					stats.fireShot();
				}
			}
			//end code for stat tracking
            if (shot_sound != null)
            {
                shot_sound.Play();
            }
			//var instantiated_bullet = (GameObject) Instantiate(bullet, muzzle.position, muzzle.rotation * bullet_rotation);

            GameObject instantiated_bullet = (GameObject)Instantiate(bullet, muzzle.position, muzzle.rotation * bullet_rotation); ;
            /*
            if (gameObject.name.Contains("Pistol"))
            {
                instantiated_bullet = pool.getPistolBullet();
            }
            else if (gameObject.name.Contains("Crazy Gun"))
            {
                instantiated_bullet = pool.getCrazyGunBullet();
            }
            else if (gameObject.name.Contains("Machine Gun"))
            {
                instantiated_bullet = pool.getMachineGunBullet();
            }
            else if (gameObject.name.Contains("Ray"))
            {
                instantiated_bullet = pool.getRayGunBullet();
            }
            else
            {
                return;
            }
             * */
            //instantiated_bullet = getBullet();

            //Initialize the bullet
			BulletController BC = instantiated_bullet.GetComponent<BulletController>();
            BC.StartUp(damage, source, cleanup_delay);
			BC.SetLaser(is_laser);
			last_fired = Time.time;

            //setup position & velocity
            instantiated_bullet.transform.position = muzzle.position;
            instantiated_bullet.transform.rotation = muzzle.rotation * bullet_rotation;
            instantiated_bullet.GetComponent<Rigidbody>().velocity = muzzle.TransformDirection(Vector3.forward * speed);
            instantiated_bullet.GetComponent<Rigidbody>().WakeUp();
            instantiated_bullet.SetActive(true);

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

    /*
    private IEnumerator makeBullets()
    {
        while (bullets.Count < bulletAmount)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject b = (GameObject)Instantiate(bullet);
                b.SetActive(false);
                bullets.Add(b);
            }
            yield return null;
        }
    }

    private GameObject getBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }
        GameObject t = (GameObject)Instantiate(bullet);
        //t.SetActive(true);
        bullets.Add(t);
        return t;
    }
     * */
}
