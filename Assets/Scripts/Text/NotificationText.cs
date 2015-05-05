using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationText : MonoBehaviour {
	
	//[SerializeField]
	//private NotificationLog log;
	private LogScript log;
	//private GUIText weapon_name;
	private Text currentNotification;
	private bool showString;
	
	// Use this for initialization
	void Start () {
		GameObject notificationLog = GameObject.FindWithTag("Log");
		//log = notificationLog.GetComponent<NotificationLog>();
		log = notificationLog.GetComponent<LogScript>();
		currentNotification = GetComponent<Text>();
		DontDestroyOnLoad (currentNotification);
	}
	
	void Update() {	
		currentNotification.text = "" + log.getNotifications();
		Debug.Log(currentNotification.text);
	}
}
