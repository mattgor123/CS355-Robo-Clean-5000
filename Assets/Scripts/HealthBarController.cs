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
		slider = GameObject.FindWithTag ("HealthSlider").GetComponent<Slider>();
		slider.minValue = 0;
		slider.maxValue = 0;
	}
	
	private void LateUpdate()
	{
		//Debug.Log(weapon_backpack_controller.GetWeaponName());
		if (health_controller != null) {
			slider.maxValue = health_controller.GetMaxHealth();
			slider.value = health_controller.GetCurrentHealth();
			//update color here, using RGBA values
			var barFill = GameObject.FindWithTag ("HealthBarFill");
			var imageFill = barFill.GetComponent<Image>();
			var oldColor = imageFill.color;
			oldColor.a = getA ();
			oldColor.b = getB ();
			oldColor.r = getR ();
			oldColor.g = getG();
			imageFill.color = oldColor;
		}
	}
	private float getR() {
		if (slider.value < (slider.maxValue / 2)) {
			return 1;
		} else {
			float f = slider.value - (slider.maxValue / 2);
			f = (f / (slider.maxValue / 2));
			return (1 - f);
		}
	}
	private float getG() {
		if (slider.value > (slider.maxValue / 2)) {
			return 1;
		} else {
			float f = slider.value;
			f = (f / (slider.maxValue / 2));
			return f;
		}
	}
	private float getB() {
		return 0;
	}
	private float getA() {
		return 1;
	}
}
