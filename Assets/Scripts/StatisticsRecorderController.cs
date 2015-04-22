using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatisticsRecorderController : MonoBehaviour {

	private Text timeText;
	private Text dealtText;
	private Text takenText;
	private float timeAtStart;
	private float timeNow;
	private float timeElapsed;
	private float damageDealt;
	private float damageTaken;

	// Use this for initialization
	void Start () {
		timeAtStart = Time.time;
		timeNow = Time.time;
		timeElapsed = timeAtStart - timeNow;
		damageDealt = 0;
		damageTaken = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeText == null) {
			timeText = GameObject.FindGameObjectWithTag ("TimeText").GetComponent<Text> ();
		}
		if (dealtText == null) {
			dealtText = GameObject.FindGameObjectWithTag ("DealtText").GetComponent<Text> ();
		}
		if (takenText == null) {
			takenText = GameObject.FindGameObjectWithTag ("TakenText").GetComponent<Text> ();
		}
		if (timeText != null) {
			timeNow = Time.time;
			timeElapsed = timeNow - timeAtStart;
			timeText.text = "Time Elapsed: " + timeElapsed.ToString();
		}
		if (takenText != null) {
			takenText.text = "Damage Taken: " + damageTaken.ToString();
		}
		if (dealtText != null) {
			dealtText.text = "Damage Dealt: " + damageDealt.ToString();
		}
	}
	public void takeDamage(float f) {
		damageTaken += f;
	}
	public void dealDamage(float f) {
		damageDealt += f;
	}
}
