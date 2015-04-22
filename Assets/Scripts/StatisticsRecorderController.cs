using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatisticsRecorderController : MonoBehaviour {

	private Text timeText;
	private Text dealtText;
	private Text takenText;
	private Text killedText;
	private float timeAtStart;
	private float timeNow;
	private float timeElapsed;
	private float damageDealt;
	private float damageTaken;
	private int enemiesKilled;

	// Use this for initialization
	void Start () {
		timeAtStart = Time.time;
		timeNow = Time.time;
		timeElapsed = timeAtStart - timeNow;
		damageDealt = 0;
		damageTaken = 0;
		enemiesKilled = 0;
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
		if (killedText == null) {
			killedText = GameObject.FindGameObjectWithTag ("KilledText").GetComponent<Text> ();
		}
		if (timeText != null) {
			timeNow = Time.time;
			timeElapsed = timeNow - timeAtStart;
			timeText.text = "Time Elapsed: " + timeElapsed.ToString("0.0");
		}
		if (takenText != null) {
			takenText.text = "Damage Taken: " + damageTaken.ToString("0.0");
		}
		if (dealtText != null) {
			dealtText.text = "Damage Dealt: " + damageDealt.ToString("0.0");
		}
		if (killedText != null) {
			killedText.text = "Enemies Killed: " + enemiesKilled.ToString();
		}
	}
	public void takeDamage(float f) {
		damageTaken += f;
	}
	public void dealDamage(float f) {
		damageDealt += f;
	}
	public void killEnemy() {
		enemiesKilled++;
	}
	public void resetStats() {
		Start ();
	}
}
