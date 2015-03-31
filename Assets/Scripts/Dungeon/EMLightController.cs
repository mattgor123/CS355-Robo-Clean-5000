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

    [SerializeField]
    private float Ftime;  //minimum time between flickers; does not flicker if == 0

    private bool activated; //whether light has been activated
    private Light[] EML;      //the light object children
    private int numLights;      //number of lights

    private float Foffset;  //flicker offset (to keep different lights unaligned)

    // Use this for initialization
    void Start()
    {
        //Attach subcomponent light
        EML = GetComponentsInChildren<Light>();
        numLights = EML.Length;
        Foffset = Random.value * Ftime;

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

        //flicker 
        if (Ftime != 0 && Time.time % (Ftime + Foffset + Random.value*3f) <= 0.2) {
            Flicker();
        }
        

    }

    //Set intensity of all lights
    private void SetIntensity(float intense)
    {
        for (int i = 0; i < numLights; i++)
        {
            EML[i].intensity = intense;
        }
    }

    //Flicker the lights
    private void Flicker()
    {
        if (Time.time % Random.value <= 0.2)
        {
            SetIntensity(0f);
        }

    }

    //set flicker time externally
    public void SetFlickerTime(float f)
    {
        Ftime = f;
    }
}

