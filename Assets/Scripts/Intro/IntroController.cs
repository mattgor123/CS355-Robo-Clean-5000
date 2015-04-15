using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour {

	[SerializeField] private Text thought_text;
	[SerializeField] private float thought_delay;
	[SerializeField] private Light spotlight;
	[SerializeField] private Light flashlight;

	private int last_message;
	private float last_message_time;
	private string[] thoughts = { "SYSTEM: Booting Security Bot Robo-Clean 5000...",
								  "ROOT: Starting System...",
								  "!! New directive loaded: Id: 1003399k98",
								  "INFO: Military research bot systems have failed in the basement subsystems.",
								  "MISSION: Descend to the control facility to disable the rougue bots.",
								  "END TRANSMISSION" };

	void Start () {
		last_message_time = -thought_delay;
		last_message = -1;
	}

	void Update () {
		if(Time.time - last_message_time > thought_delay) {
			++last_message;
			if(last_message == 1) {
				spotlight.enabled = true;
			} else if(last_message == 2) {
				flashlight.enabled = true;
				spotlight.color = Color.yellow;
			} else if(last_message == 3) {
				spotlight.color = Color.green;
			} else if(last_message == thoughts.Length) {
				Application.LoadLevel("Tutorial");
				return;
			}
			thought_text.text = thoughts[last_message];
			last_message_time = Time.time;
		}
	}
}
