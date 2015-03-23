using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationText : MonoBehaviour {
	
	//[SerializeField]
	private NotificationLog log;
	//private GUIText weapon_name;
	private Text currentNotification;
	
	// Use this for initialization
	void Start () {
		GameObject notificationLog = GameObject.FindWithTag("Log");
		log = notificationLog.GetComponent<NotificationLog>();
		currentNotification = GetComponent<Text>();
		DontDestroyOnLoad (currentNotification);
	}
	
	private void LateUpdate()
	{
		currentNotification.text = "" + log.getCurrentNotification ();
	}
}