using UnityEngine;
using System.Collections;

public class ElevatorInfo : MonoBehaviour {

	private bool triggered = false;

	void OnCollisionEnter(Collision collision) {
		if(!triggered) {
			var log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();
			log.PassMessage("Find the elevator to continue...\nThis is your goal - you need to go kill enemıes to collect access cards and reach the Time Machine!");
		}
	}
}
