using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour {

    [SerializeField]
    private GeorgePlayerController player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        light.spotAngle = player.getHP(); //size of spotlight is player hp
	}
}
