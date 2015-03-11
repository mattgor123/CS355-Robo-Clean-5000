using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    [SerializeField] private Transform weapon_canvas;
    [SerializeField] private Transform health_canvas;
    [SerializeField] private Transform ammo_canvas;

    private string message;
    private float timer;
    private float delay;

	void Start () {
        var player_instance = Instantiate(player, new Vector3(0f, 0.09f, 0f), Quaternion.identity) as GameObject;
        var camera_instance = Instantiate(camera, camera.transform.position, camera.transform.rotation) as GameObject;
        var weapon_canvas_instance = Instantiate(weapon_canvas) as Transform;
        var health_canvas_instance = Instantiate(health_canvas) as Transform;
        var ammo_canvas_instance = Instantiate(ammo_canvas) as Transform;
        message = "Press W to follow the mouse";
        timer = Time.time;
        delay = 3.0f;
	}
	
	void Update () {
        if (Time.time - timer > delay) {
            message = "";
        }
	}

    public string GetMessage() {
        return message;
    }

    public void SetMessage(string new_message, float new_delay) {
        message = new_message;
        timer = Time.time;
        delay = new_delay;
    }
}
