using UnityEngine;
using System.Collections;

public class DemoListener : MonoBehaviour {

	[SerializeField] private WeaponController weapon;
	[SerializeField] private HealthController health;
	[SerializeField] private GUIText health_text;

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			weapon.StartFiring();
		}
		else if(Input.GetMouseButtonUp(0)) {
			weapon.StopFiring();
		}

		health_text.text = health.GetCurrentHealth().ToString();
	}
}
