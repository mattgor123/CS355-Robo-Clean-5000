using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponBackpackController : MonoBehaviour {

	[SerializeField] private Transform weapon_spawn;

	private List<GameObject> weapons = new List<GameObject>();
	private int weapon_index;
	private int last_rendered_index;
	private GameObject weapon_instance;

	private void Start () {
		weapon_index = 0;
		last_rendered_index = -1;
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
		   		last_rendered_index = weapon_index;
		   		if(firing) {
		   			weapon_instance.GetComponent<WeaponController>().StartFiring();
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
}
