using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatisticsRecorderController : MonoBehaviour {

	private Text timeText;
	private Text dealtText;
	private Text takenText;
	private Text killedText;
	private Text accuracyText;
	private Text weaponText;
  private Text highestComboText;
	private float timeAtStart;
	private float timeNow;
	private float timeElapsed;
	private float damageDealt;
	private float damageTaken;
	private int enemiesKilled;
	private float bulletsFired;
	private float bulletsHit;
	private float accuracy;
	private Dictionary<string, int> dict;
	private string favoriteweapon;
  private int highestCombo;

	// Use this for initialization
	void Start () {
		timeAtStart = Time.time;
		timeNow = Time.time;
		timeElapsed = timeAtStart - timeNow;
		damageDealt = 0;
		damageTaken = 0;
		enemiesKilled = 0;
		bulletsFired = 0;
		bulletsHit = 0;
		accuracy = 0;
		dict = new Dictionary<string, int> ();
		favoriteweapon = "";
    highestCombo = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeText == null && GameObject.FindGameObjectWithTag("TimeText") != null) {
			timeText = GameObject.FindGameObjectWithTag ("TimeText").GetComponent<Text> ();
		}
		if (dealtText == null && GameObject.FindGameObjectWithTag("DealtText") != null) {
			dealtText = GameObject.FindGameObjectWithTag ("DealtText").GetComponent<Text> ();
		}
		if (takenText == null && GameObject.FindGameObjectWithTag("TakenText") != null) {
			takenText = GameObject.FindGameObjectWithTag ("TakenText").GetComponent<Text> ();
		}
		if (killedText == null && GameObject.FindGameObjectWithTag("KilledText") != null) {
			killedText = GameObject.FindGameObjectWithTag ("KilledText").GetComponent<Text> ();
		}
		if (accuracyText == null && GameObject.FindGameObjectWithTag("AccuracyText") != null) {
			accuracyText = GameObject.FindGameObjectWithTag ("AccuracyText").GetComponent<Text> ();
		}
		if (weaponText == null && GameObject.FindGameObjectWithTag("WeaponText") != null) {
			weaponText = GameObject.FindGameObjectWithTag ("WeaponText").GetComponent<Text> ();
		}
		if (highestComboText == null && GameObject.FindGameObjectWithTag("HighestComboText") != null) {
			highestComboText = GameObject.FindGameObjectWithTag ("HighestComboText").GetComponent<Text> ();
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
		if (accuracyText != null) {
			accuracy = (bulletsHit / bulletsFired) * 100.0f;
			accuracyText.text = "Shot Accuracy: " + accuracy.ToString("0.0");
		}
		if (weaponText != null) {
			getFavorite ();
			weaponText.text = "Favorite Weapon: " + favoriteweapon;
		}
		if (highestComboText != null) {
			highestComboText.text = "Highest Combo: X" + highestCombo.ToString();
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
	public void fireShot() {
		bulletsFired = bulletsFired + 1.0f;
	}
	public void hitShot() {
		bulletsHit = bulletsHit + 1.0f;
	}
	public void fireWeapon(string s) {
		if (!dict.ContainsKey (s)) {
			dict.Add (s, 1);
		} else {
			dict[s] = dict[s] + 1;
		}
	}
	public void getFavorite() {
		int max = 0;
		string favorite = "";
		foreach (KeyValuePair<string, int> entry in dict) {
			if (entry.Value > max) {
				favorite = entry.Key;
				max = entry.Value;
			}
		}
		favoriteweapon = favorite;
	}
  public void setCombo(int combo) {
    if(combo > highestCombo) {
      highestCombo = combo;
    }
  }
}
