using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBackpackController : MonoBehaviour {

	[SerializeField] private Transform weapon_spawn;
	[SerializeField] private int starting_ammo;

	private List<GameObject> weapons = new List<GameObject>();
	private int weapon_index;
	private int last_rendered_index;
	private GameObject weapon_instance;
	private int ammo;

	private void Start () {
		weapon_index = 0;
		last_rendered_index = -1;
		ammo = starting_ammo;
	}

	private void LateUpdate () {
		if(weapons.Count > 0) {
		    if(weapon_index != last_rendered_index) {
		    	var firing = false;
		   		if(last_rendered_index != -1) {
		   			if(weapon_instance.GetComponent<WeaponController>().IsFiring()) {
		   				firing = true;
		   			}
		   			Destroy(weapon_instance);
		   		}
		   		var position = weapon_spawn.position + weapons[weapon_index].transform.position;
		   		var rotation = weapon_spawn.rotation * weapons[weapon_index].transform.rotation;
		   		weapon_instance = (GameObject) Instantiate(weapons[weapon_index], position, rotation);
		   		weapon_instance.transform.parent = weapon_spawn;
		   		var weapon_controller = weapon_instance.GetComponent<WeaponController>();
		   		weapon_controller.SetBackpackController(this);
		   		last_rendered_index = weapon_index;
		   		if(firing) {
		   			weapon_controller.StartFiring();
		   		}
			}
		}
	}
        
	public int NumberOfWeapons () {
		return weapons.Count;
	}

	public void AddWeapon (GameObject weapon) {
		weapons.Add(weapon);
	}

	public void ChooseWeapon (int new_weapon_index) {
		weapon_index = new_weapon_index % weapons.Count;
	}

	public void NextWeapon () {
		weapon_index = ++weapon_index % weapons.Count;
	}

	public void LastWeapon () {
		weapon_index = --weapon_index % weapons.Count;
	}

	public void StartFiring () {
		if(weapon_instance != null) {
			weapon_instance.GetComponent<WeaponController>().StartFiring();
		}
	}

	public void StopFiring () {
		if(weapon_instance != null) {
			weapon_instance.GetComponent<WeaponController>().StopFiring();
		}
	}

	public string GetWeaponName () {
		return weapons[weapon_index].name;
	}

	public bool HasAmmo (int number) {
		if(ammo >= number) {
			return true;
		}
		return false;
	}

	public int GetAmmo () {
		return ammo;
	}

	public void ChangeAmmo (int change) {
		ammo += change;
	}
}
