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
    private float Ftime;  //minimum time between flickers; does not flicker if == 0; if negative, stays off and flickers on

    [SerializeField]
    private float red;      //input light color
    [SerializeField]
    private float green;
    [SerializeField]
    private float blue;

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
        SetColor(red, green, blue);
    }

    // Link a trigger tile
    public void LinkTile(Transform tile)
    {
        TTile = tile.GetComponent<TriggerTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Time.timeScale == 0) return;
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

        //If flickertime is negative, stay off but flickers on
        if (Ftime < 0)
        {
            SetIntensity(0f);
        }

        //flicker 
        if (Ftime != 0 && Time.time % (Mathf.Abs(Ftime) + Foffset + Random.value*3f) <= 0.2) {
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
            if (Ftime > 0)
                SetIntensity(0f);
            else if (Ftime < 0)
                SetIntensity(dim);
        }

    }

    //set flicker time externally
    public void SetFlickerTime(float f)
    {
        Ftime = f;
    }

    //Set color (with rgb values)
    public void SetColor(float r, float g, float b)
    {
        Color c = new Color(r, g, b);
        SetColor(c);
    }

    //Set color (with color object)
    public void SetColor(Color c)
    {
        red = c.r;
        green = c.g;
        blue = c.b;
        for (int i = 0; i < numLights; i++)
        {
            EML[i].color = c;
        }
    }
}

