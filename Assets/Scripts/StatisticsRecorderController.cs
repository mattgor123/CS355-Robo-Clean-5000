using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatisticsRecorderController : MonoBehaviour {

	private Text timeText;
	private float timeAtStart;
	private float timeNow;
	private float timeElapsed;

	// Use this for initialization
	void Start () {
		timeAtStart = Time.time;
		timeNow = Time.time;
		timeElapsed = timeAtStart - timeNow;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeText == null) {
			timeText = GameObject.FindGameObjectWithTag ("TimeText").GetComponent<Text> ();
		}
		if (timeText != null) {
			timeNow = Time.time;
			timeElapsed = timeNow - timeAtStart;
			timeText.text = "Time Elapsed: " + timeElapsed.ToString();
		}
	}
}
