using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScrollBarScript : MonoBehaviour {

	private Scrollbar scroll; 
	private LogScript log;

	private float scrollValue;
	private int offset;
	//private float scrollSize;

	// FIX SCROLL SIZE SOMEHOW
	void Start () {
		GameObject SB = GameObject.FindWithTag("Scroll");
		GameObject Log = GameObject.FindWithTag("Log");
		scroll = SB.GetComponent<Scrollbar>();
		log = Log.GetComponent<LogScript>();
		scrollValue =  1f; // very arbitrary value...
		scroll.size = 1f;
		offset = 0; // used for division by 0 purposes
	}
	
	// Update is called once per frame
	void Update () {

		if (log.GetShowString()) {
			if (log.Size() == 0) {
				offset = 1;
			} else {
				offset = 0;
			}
			scroll.size = (float) 1/(log.Size() + offset); // +1 to avoid division by 0 error
			scrollValue = (float) 1/(log.Size() + offset) + (scroll.size/2);

			if (Input.GetKeyDown("m")) {
				if (scroll.value > 0) {
					scroll.value -= scrollValue;
					if (scroll.value < 0) {
						scroll.value = 0;
					}
					log.ScrollDown();	
				}
			} else if (Input.GetKeyDown("n")) {
				if (scroll.value < 1) {
					scroll.value += scrollValue;
					if (scroll.value > 1) {
						scroll.value = 1;
					}
					log.ScrollUp();	
				}
			}
		} else {
			scroll.value = 1;
			scroll.size = 1;
		}
	}
}
