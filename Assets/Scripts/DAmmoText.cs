using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DAmmoText : MonoBehaviour {
	
	//[SerializeField]
	private WeaponBackpackController weapon_backpack_controller;
	
	//private GUIText weapon_name;
	private Text player_ammo;
	
	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag("Player");
		weapon_backpack_controller = player.GetComponent<WeaponBackpackController>();
		player_ammo = GetComponent<Text>();	
	}
	
	private void LateUpdate()
	{
		//Debug.Log(weapon_backpack_controller.GetWeaponName());
		player_ammo.text = "Ammo: " + weapon_backpack_controller.GetAmmo().ToString();
	}
}
