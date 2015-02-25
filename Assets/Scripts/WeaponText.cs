using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponText : MonoBehaviour {

    //[SerializeField]
    private WeaponBackpackController weapon_backpack_controller;

    //private GUIText weapon_name;
    private Text weapon_name;

	// Use this for initialization
	void Start () {
        GameObject player = GameObject.FindWithTag("Player");
        weapon_backpack_controller = player.GetComponent<WeaponBackpackController>();
        weapon_name = GetComponent<Text>();
	
	}

    private void LateUpdate()
    {
        //Debug.Log(weapon_backpack_controller.GetWeaponName());
        weapon_name.text = weapon_backpack_controller.GetWeaponName();
    }
}
