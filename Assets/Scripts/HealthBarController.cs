using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour {
	
	//[SerializeField]
	private HealthController health_controller;
	
	//private GUIText weapon_name;
	private Slider slider;
	
	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag("Player");
		health_controller = player.GetComponent<HealthController>();
		slider = GameObject.FindWithTag ("HealthCanvas").GetComponent<Slider>();
		slider.minValue = 0;
		slider.maxValue = 0;
	}
	
	private void LateUpdate()
	{
		//Debug.Log(weapon_backpack_controller.GetWeaponName());
		if (health_controller != null) {
			slider.maxValue = health_controller.GetMaxHealth();
			slider.value = health_controller.GetCurrentHealth();
		}
	}
}