using UnityEngine;
using System.Collections;


// Generic Emergency Light controller
// Activates upon trigger & stays on if linked to a trigger tile
// Else stays on
// Note: position is 4.5 from a wall, in the direction of its face
public class EMLightController : MonoBehaviour {

    [SerializeField]
    private TriggerTile TTile;

    [SerializeField]
    private float strength; //intensity of light when fully lit

    [SerializeField]
    private float dim;      //intensity of light when not directly triggered

    private bool activated; //whether light has been activated
    private Light[] EML;      //the light object children
    private int numLights;      //number of lights

    // Use this for initialization
    void Start()
    {
        //Attach subcomponent light
        EML = GetComponentsInChildren<Light>();
        numLights = EML.Length;

    }

    // Link a trigger tile
    public void LinkTile(Transform tile)
    {
        TTile = tile.GetComponent<TriggerTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //If no tile attached, or active but not triggered, keep dim
        if (TTile == null || activated) 
        {
            SetIntensity(dim);
        }

        //On trigger, flag activated and keep at full strength       
        else if (TTile.GetTriggerStatus())
        {
            SetIntensity(strength);
            activated = true;
        }
        else
            SetIntensity(0f);


    }

    //Set intensity of all lights
    private void SetIntensity(float intense)
    {
        for (int i = 0; i < numLights; i++)
        {
            EML[i].intensity = intense;
        }
    }
}

