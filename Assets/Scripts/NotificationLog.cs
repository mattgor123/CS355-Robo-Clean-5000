using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationLog : MonoBehaviour {

	private List<string> loggedNotifications;
	private string currentNotificationString;
	private bool showString;
	private float displayTime;
	private float lastMessageTime;

	// Use this for initialization
	void Start () {
		loggedNotifications = new List<string> ();
		currentNotificationString = "";
		showString = false;
		displayTime = 5;
		lastMessageTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (showString);
		if (lastMessageTime + displayTime < Time.time) {
			showString = true;
		} else {
			showString = false;
		}
	}

	public string getCurrentNotification() {
		if (showString) {
			return currentNotificationString;
		} else {
			return "";
		}
	}

	public void PassMessage(string s) {
		//Debug.Log ("passing");
		bool showString = true;
		loggedNotifications.Add (s);
		currentNotificationString = s;
		lastMessageTime = Time.time;
	}

	public void SetDisplayTime(float f) {
		displayTime = f;
	}
}
