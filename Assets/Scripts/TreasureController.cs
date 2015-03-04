﻿using UnityEngine;
using System.Collections;

public class TreasureController : MonoBehaviour {

	[SerializeField] private int max_ammo;

	private void OnCollisionEnter (Collision collision) {
		var weapon_backpack = collision.collider.GetComponent<WeaponBackpackController>();
		if(weapon_backpack != null) {
			var found_ammo = Random.Range(0, max_ammo + 1);
			weapon_backpack.ChangeAmmo(found_ammo);
			Destroy(gameObject);
		}
	}
}