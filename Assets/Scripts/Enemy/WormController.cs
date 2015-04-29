using UnityEngine;
using System.Collections;

public class WormController : EnemyController {
	[SerializeField] private GameObject crazy_gun;

	private void Start () {
		StartBody();
		GetComponent<WeaponBackpackController>().AddWeapon(crazy_gun);
	}
}
