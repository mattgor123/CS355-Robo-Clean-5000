using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationLog : MonoBehaviour {

	private List<string> loggedNotifications;
	public Transform currentNotificationCanvas;
	//public Transform loggedNotificationCanvas;
	private Text currentNotification;
	private string currentNotificationString;
	private bool showString;
	private float displayTime;
	private float lastMessageTime;

	// Use this for initialization
	void Start () {
		loggedNotifications = new List<string> ();
		Transform currentNotificationCanvasInstance = Instantiate (currentNotificationCanvas) as Transform;
		//Transform loggedNotificationCanvasInstance = Instantiate (loggedNotificationCanvas) as Transform;
		DontDestroyOnLoad (currentNotificationCanvasInstance);
		//DontDestroyOnLoad (loggedNotificationCanvasInstance);
		currentNotificationString = "";
		showString = false;
		displayTime = 5;
		lastMessageTime = 0;
		currentNotification = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastMessageTime + displayTime > Time.time) {
			showString = false;
		}
		if (showString) {
			currentNotification.text = currentNotificationString;
		} else {
			currentNotification.text = "";
		}
	}

	void passMessage(string s) {
		bool showString = true;
		loggedNotifications.Add (s);
		currentNotificationString = s;
		lastMessageTime = Time.time;
	}

	void setDisplayTime(float f) {
		displayTime = f;
	}
}
