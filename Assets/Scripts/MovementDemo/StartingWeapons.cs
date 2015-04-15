using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StartingWeapons : MonoBehaviour {

	[SerializeField] private WeaponBackpackController weapon_backpack_controller;
	
	[SerializeField] private GameObject start_weapon;
	[SerializeField] private GameObject weapon1;
	[SerializeField] private GameObject weapon2;
	[SerializeField] private GameObject weapon3;
	private int initialNumWeapons; //set equal to number of weapons that exist (other than start_weapon)
	private int initialNumNullDrops;
	private bool hasWeapon1;
	private bool hasWeapon2;
	private bool hasWeapon3;

	//private GUIText weapon_name;
    //private Text weapon_name;

	private void Start () {
		weapon_backpack_controller.AddWeapon(start_weapon);
		hasWeapon1 = false;
		hasWeapon2 = false;
		hasWeapon3 = false;
		initialNumWeapons = 3;
		initialNumNullDrops = 2;

        //weapon_name = GetComponent<GUIText>();
		//weapon_name = GetComponent<Text>();
	}

	private void LateUpdate () {
		//weapon_name.text = weapon_backpack_controller.GetWeaponName();
	}

	public string RandomWeaponPickup() {
		System.Random rnd = new System.Random ();
		int pickupNumber = rnd.Next (1, initialNumWeapons+initialNumNullDrops); 
		if (pickupNumber == 1 && !hasWeapon1) {
			weapon_backpack_controller.AddWeapon (weapon1);
			hasWeapon1 = true;
			return ("Picked up " + weapon1.name);
		} else if (pickupNumber == 2 && !hasWeapon2) {
			weapon_backpack_controller.AddWeapon (weapon2);
			hasWeapon2 = true;
			return ("Picked up " + weapon2.name);
		} else if (pickupNumber == 3 && !hasWeapon3) {
			weapon_backpack_controller.AddWeapon (weapon3);
			hasWeapon3 = true;
			return ("Picked up " + weapon3.name);
		}
		return "";
	}

	public string PickupAll() {
		string result = "";
		if (!hasWeapon1) {
			weapon_backpack_controller.AddWeapon (weapon1);
			hasWeapon1 = true;
			result += "Picked up " + weapon1.name + "\n";
		}
		if (!hasWeapon2) {
			weapon_backpack_controller.AddWeapon(weapon2);
			hasWeapon2 = true;
			result += "Picked up " + weapon2.name + "\n";
		}
		if (!hasWeapon3) {
				weapon_backpack_controller.AddWeapon (weapon3);
				hasWeapon3 = true;
				result += "Picked up " + weapon3.name + "\n";
		}
		return result;
	}
}
