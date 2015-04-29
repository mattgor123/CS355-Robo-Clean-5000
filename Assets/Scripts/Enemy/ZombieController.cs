using UnityEngine;
using System.Collections;

public class ZombieController : EnemyController {
	[SerializeField] private GameObject melee_weapon;

	private void Start () {
		StartBody();
		GetComponent<WeaponBackpackController>().AddWeapon(melee_weapon);
	}
}
