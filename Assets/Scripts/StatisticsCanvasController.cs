using UnityEngine;
using System.Collections;

public class StatisticsCanvasController : MonoBehaviour {

	private GameObject statCanvas;
	private GameObject elevCanvas;
	// Use this for initialization
	void Start () {
		elevCanvas = GameObject.FindGameObjectWithTag ("ElevatorCanvas");
		statCanvas = GameObject.FindGameObjectWithTag ("StatCanvas");
		statCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (statCanvas == null) {
			Debug.Log ("stat null");
			statCanvas = GameObject.FindGameObjectWithTag ("StatCanvas");
		}
		if (elevCanvas == null) {
			Debug.Log ("elev null");
			elevCanvas = GameObject.FindGameObjectWithTag ("ElevatorCanvas");
		}
		if (statCanvas != null && elevCanvas != null) {
			statCanvas.SetActive (elevCanvas.activeSelf);
		}
	}
}
