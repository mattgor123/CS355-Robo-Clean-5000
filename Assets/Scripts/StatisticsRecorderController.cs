using UnityEngine;
using System.Collections;

public class StatisticsRecorderController : MonoBehaviour {

	[SerializeField] private GameObject statCanvas;

	// Use this for initialization
	void Start () {
		statCanvas = GameObject.FindGameObjectWithTag ("StatCanvas");
		statCanvas.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
