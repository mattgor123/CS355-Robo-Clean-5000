using UnityEngine;
using System.Collections;

public class LogScript : MonoBehaviour {

	private string[] Log;
	private int LOG_LENGTH = 25;
	private int size;
	private int head;
	private int tail;
	private bool showString;
	
	void Awake () {
		Log = new string[LOG_LENGTH];
		size = 0;
		head = 0;
		tail = 0;
		showString = false;
	}

	void Update() {
		//string notif = getNotifications();
		//currentNotification.text = "" + log.getCurrent
	}

	public string getNotifications() {
		string str = "";
		if (showString) {
			//Debug.Log("Getting notifications");
			int i;
 			int j = head;
 			for (i = 0; i < size; i++) {
 				str += (Log[j] + "\n");
 				j = (j + 1) % Log.Length;
 			}
 		}	
 		//Debug.Log("Notifications are:");
 		//Debug.Log(str);
 		return str;
	}

	public void PassMessage(string message) {
		Log[tail] = message;
		tail++;
		if (size < LOG_LENGTH) {
			size++;
		} else {
			head++;
		}
		CheckValues();
 	}

 	private void CheckValues() {
 		//Debug.Log("Entered CheckValues");
 		if (head >= Log.Length) {
			head = 0;
		}
		if (tail >= Log.Length) {
			tail = 0;
			head = 1;
		}
		//Debug.Log("Tail is: " + tail);
 	}	

 	public void EnableNotifications() {
		showString = !showString;
	}

 	/*public void PrintLog() {
 		int i;
 		int j = head;
 		for (i = 0; i < size; i++) {
 			Debug.Log(Log[j]);
 			j = (j + 1) % Log.Length;
 		}
 	}*/
}
