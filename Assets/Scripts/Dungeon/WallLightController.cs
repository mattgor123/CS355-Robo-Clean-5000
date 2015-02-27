using UnityEngine;
using System.Collections;

public class WallLightController : MonoBehaviour {
    [SerializeField]
    private TriggerTile TTile;

    [SerializeField]
    private float strength; //intensity of light when fully lit

    [SerializeField]
    private float dim;      //intensity of light when dimmed (after initial activation)

    private bool activated; //whether light has been activated

	// Use this for initialization
	void Start () {
	    light.intensity = 0;
        activated = false;
	}
	
    // Link a trigger tile
    public void LinkTile(Transform tile)
    {
        TTile = tile.GetComponent<TriggerTile>();
    }

	// Update is called once per frame
	void LateUpdate () {
        if (TTile.GetTriggerStatus())
        {
            light.intensity = strength;
            activated = true;
        }
        else if (activated)
        {
            light.intensity = dim;
        }
        else
            light.intensity = 0;       
	}
}
