using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour {

    [SerializeField]
    private PlayerController player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        light.spotAngle = player.getHP();
	}
}
