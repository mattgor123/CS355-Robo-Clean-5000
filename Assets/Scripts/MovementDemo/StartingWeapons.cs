using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StartingWeapons : MonoBehaviour {

	[SerializeField] private WeaponBackpackController weapon_backpack_controller;
	
	[SerializeField] private GameObject start_weapon;
	[SerializeField] private GameObject weapon1;
	[SerializeField] private GameObject weapon2;
	private int initialNumWeapons; //set equal to number of weapons that exist (other than start_weapon)
	private int initialNumNullDrops;
	private bool hasWeapon1;
	private bool hasWeapon2;

	//private GUIText weapon_name;
    //private Text weapon_name;

	private void Start () {
		weapon_backpack_controller.AddWeapon(start_weapon);
		hasWeapon1 = false;
		hasWeapon2 = false;
		initialNumWeapons = 2;
		initialNumNullDrops = 2;

        //weapon_name = GetComponent<GUIText>();
		//weapon_name = GetComponent<Text>();
	}

	private void LateUpdate () {
		//weapon_name.text = weapon_backpack_controller.GetWeaponName();
	}

	public string RandomWeaponPickup() {
		GameObject nlog = GameObject.FindWithTag("Log");
		NotificationLog log = nlog.GetComponent<NotificationLog>(); 
		System.Random rnd = new System.Random ();
		int pickupNumber = rnd.Next (1, initialNumWeapons+initialNumNullDrops); 
		if (pickupNumber == 1 && !hasWeapon1) {
			weapon_backpack_controller.AddWeapon (weapon1);
			hasWeapon1 = true;
			return ("Picked up " + weapon1.name);
		} else if (pickupNumber == 2 && !hasWeapon2) {
			weapon_backpack_controller.AddWeapon(weapon2);
			hasWeapon2 = true;
			return ("Picked up " + weapon2.name);
		}
		return "";
	}
}
