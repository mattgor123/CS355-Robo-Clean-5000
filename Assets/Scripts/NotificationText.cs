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
		GameObject notificationLog = GameObject.FindWithTag("NotificationLog");
		log = notificationLog.GetComponent<NotificationLog>();
		currentNotification = GetComponent<Text>();
	}
	
	private void LateUpdate()
	{
		currentNotification.text = log.getCurrentNotification ();
	}
}