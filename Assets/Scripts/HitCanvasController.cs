﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitCanvasController : MonoBehaviour {

	private float displayTime;
	private bool hitScored;
	private bool killScored;
	private float hitTime;
	private float killTime;
	private GameObject redDot;
	private GameObject blackDot;
	private GameObject damaged;
	private HealthController hcontroller;
	private float health;
	//private RawImage redDotImage;
	//private RawImage blackDotImage;

	// Use this for initialization
	void Start () {
		hitScored = false;
		killScored = false;
		hitTime = 0;
		killTime = 0;
		displayTime = .25f;
		health = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (hitScored && (Time.time - hitTime >= displayTime)) {
			hitScored = false;
		}
		if (killScored && (Time.time - killTime >= displayTime)) {
			killScored = false;
		}
		if (redDot == null) {
			redDot = GameObject.FindGameObjectWithTag ("RedDot");
		}
		//if (redDot != null && redDotImage == null) {
		//	redDotImage = redDot.GetComponent<RawImage> ();
		//}
		if (blackDot == null) {
		    blackDot = GameObject.FindGameObjectWithTag ("BlackDot");
		}
		//if (blackDot != null && blackDotImage == null) {
		//	blackDotImage = blackDot.GetComponent<RawImage> ();
		//}
		if (redDot != null) {
			redDot.SetActive(killScored);
		}
		if (blackDot != null) {
			if (!killScored && hitScored) {
				blackDot.SetActive(true);
			} else {
				blackDot.SetActive(false);
			}
		}
		if (damaged == null) {
			damaged = GameObject.FindGameObjectWithTag("Damaged");
		}
		if (hcontroller == null) {
			hcontroller = GameObject.FindGameObjectWithTag ("Player").GetComponent<HealthController> ();
		}
		if (damaged != null && hcontroller != null) {
			bool dtaken = (health > hcontroller.GetCurrentHealth());
			health = hcontroller.GetCurrentHealth();
			damaged.SetActive(dtaken);
		}
	}

	public void scoreKill() {
		killScored = true;
		killTime = Time.time;
	}

	public void scoreHit() {
		hitScored = true;
		hitTime = Time.time;
	}
}