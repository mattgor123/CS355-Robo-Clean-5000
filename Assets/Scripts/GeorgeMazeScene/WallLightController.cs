using UnityEngine;
using System.Collections;

public class WallLightController : MonoBehaviour {
    [SerializeField]
    private TriggerTile TTile;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (TTile.GetTriggerStatus())
        {
            light.intensity = 1;
        }
        else
        {
            light.intensity = 0;
        }
	}
}
