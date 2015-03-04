using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthText : MonoBehaviour {
	
	//[SerializeField]
	private HealthController health_controller;
	
	//private GUIText weapon_name;
	private Text player_health;
	
	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag("Player");
		health_controller = player.GetComponent<HealthController>();
		player_health = GetComponent<Text>();
	}
	
	private void LateUpdate()
	{
			//Debug.Log(weapon_backpack_controller.GetWeaponName());
		if (health_controller != null) {
			player_health.text = "Health: " + health_controller.GetCurrentHealth ().ToString ();
		}
	}
}