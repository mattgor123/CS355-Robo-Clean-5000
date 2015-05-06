using UnityEngine;
using System.Collections;
using System;


public class LogScript : MonoBehaviour {

	private string[] Log;
	private int LOG_LENGTH = 25;
	private int size;
	private int head;
	private int tempHead;
	private int tail;
	private int scrollValue;
	private float scrollSize;
	private bool showString;
	
	void Awake () {
		Log = new string[LOG_LENGTH];
		size = 0;
		head = 0;
		tempHead = 0;
		tail = 0;
		scrollValue = 0;
		scrollSize = 1;
		showString = false;
	}

	public string getNotifications() {
		string str = "";
		tempHead = head;
		if (showString) {
			int i;
			tempHead = GetIndex(tempHead);
			// bc starting position may not be the head
 			int j = tempHead; 

 			for (i = 0; i < (size - scrollValue); i++) {
 				str += (Log[j] + "\n");
 				j = (j + 1) % Log.Length;
 			}
 		} else {
 			tempHead = head;
 			scrollValue = 0;
 		}	
 		return str;
	}

	private int GetIndex(int num) {
		num = (num + scrollValue) % Log.Length;
		return num;
	}

	public void ScrollUp() {
		scrollValue--;
	}

	public void ScrollDown() {
		scrollValue++;
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
 		if (head >= Log.Length) {
			head = 0;
		}
		if (tail >= Log.Length) {
			tail = 0;
			head = 1;
		}
 	}	

 	public void EnableNotifications() {
		showString = !showString;
	}

	public int Size() {
		return size;
	}

	public bool GetShowString() {
		return showString;
	}

	public bool CheckScrollValue() {
		return scrollValue > 0 && scrollValue < (Log.Length - 1); 
	}

	public int GetCapacity() {
		return LOG_LENGTH;
	}
}
