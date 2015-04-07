using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarController : MonoBehaviour {
	
	//[SerializeField]
	private HealthController health_controller;
	
	//private GUIText weapon_name;
	private Slider slider;

	private float flashFor; //flash for how many seconds after being hit?
	private float flashUntil; //stop flashing when this time is reached
	private float mostRecentHealth; //use to check if health has dropped since last frame

	// Use this for initialization
	void Start () {
		GameObject player = GameObject.FindWithTag("Player");
		health_controller = player.GetComponent<HealthController>();
		slider = GameObject.FindWithTag ("HealthSlider").GetComponent<Slider>();
		slider.minValue = 0;
		slider.maxValue = 0;
		flashFor = 2.0f;
		flashUntil = 0;
		mostRecentHealth = 0;
	}
	
	private void LateUpdate()
	{
		//Debug.Log(weapon_backpack_controller.GetWeaponName());
		if (health_controller != null) {
			if (health_controller.GetCurrentHealth() < mostRecentHealth) {
				flashUntil = Time.time + flashFor;
			}
			mostRecentHealth = health_controller.GetCurrentHealth();
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
		if (flashUntil < Time.time) {
			return 0.75f;
		} else {
			float remaining = flashUntil - Time.time;
			float fr = remaining / flashFor;
			if ((fr < .1) || (fr > .2 && fr < .3) || (fr > .4 && fr <.5) || (fr > .6 && fr < .7) || (fr > .8 && fr < .9)) {
				return 1;
			} else {
				return 0.5f;
			}
		}
	}
}
