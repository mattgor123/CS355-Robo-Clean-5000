﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartingWeapons : MonoBehaviour {

	[SerializeField] private WeaponBackpackController weapon_backpack_controller;

	[SerializeField] private GameObject weapon1;
	[SerializeField] private GameObject weapon2;
	[SerializeField] private GameObject weapon3;

	//private GUIText weapon_name;
    //private Text weapon_name;

	private void Start () {
		weapon_backpack_controller.AddWeapon(weapon1);
		weapon_backpack_controller.AddWeapon(weapon2);
		weapon_backpack_controller.AddWeapon(weapon3);

        //weapon_name = GetComponent<GUIText>();
		//weapon_name = GetComponent<Text>();
	}

	private void LateUpdate () {
		//weapon_name.text = weapon_backpack_controller.GetWeaponName();
	}
}
