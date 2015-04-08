using UnityEngine;
using System.Collections;

public class ElevatorInfo : MonoBehaviour {

	private bool triggered = false;

	void OnCollisionEnter(Collision collision) {
		if(!triggered) {
			var log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();
			log.PassMessage("Find the elevator\nto continue...\n\nThis is your goal - you need\nto go kill enemıes to collect\naccess cards and reach the\n Time Machine!");
		}
	}
}
