using UnityEngine;
using System.Collections;

public class LogScript : MonoBehaviour {

	private string[] Log;
	private int LOG_LENGTH = 15;
	private int size;
	private int head;
	private int tail;
	
	void Awake () {
		Log = new string[LOG_LENGTH];
		size = 0;
		head = 0;
		tail = 0;
	}

	void Update() {
		if (size >= 1) {
			PrintLog();
		}
	}

	public void AddToLog(string message) {
		CheckValues();
		Log[tail] = message;
		tail++;
		if (size < LOG_LENGTH) {
			size++;
		} else {
			head++;
		}
		CheckValues(); // only bc head could have gone out of bounds before PrintLog()
 	}

 	public void PrintLog() {
 		int i;
 		int j = head;
 		for (i = 0; i < size; i++) {
 			Debug.Log(Log[j]);
 			j = (j + 1) % Log.Length;
 		}
 	}

 	private void CheckValues() {
 		if (head > Log.Length) {
			head = 0;
		}
		if (tail > Log.Length) {
			tail = 0;
			head = 1;
		}
 	}
}
