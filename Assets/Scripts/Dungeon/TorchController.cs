using UnityEngine;
using System.Collections;

public class TorchController : MonoBehaviour {
    [SerializeField]
    private TriggerTile TTile;

    [SerializeField]
    private float strength; //intensity of light when fully lit

    [SerializeField]
    private float dim;      //intensity of light when dimmed (after initial activation)

    [SerializeField]
    private float MinAngle; //Minimum spot angle

    [SerializeField]
    private float MaxAngle; //Maximum spot angle

    private bool activated; //whether light has been activated
    private Light[] Lights; //Array of torch's lights 
    private float[] offsets; //Array  offsets for angle cycling
    private float[] mults;    //array of multiplers for angle cycling
    
    private int numLights;  //number of torchlights
    private float mean;     //angle mean
    private float var;      //angle variance

    // Use this for initialization
    void Start()
    {
        //Attach subcomponent lights & initialize to 
        Lights = GetComponentsInChildren<Light>();
        numLights = Lights.Length;
        SetIntensity(0f);

        mean = (MaxAngle + MinAngle) / 2;
        var = (MaxAngle - MinAngle) / 2;

        activated = false;

        offsets = new float[numLights];
        mults = new float[numLights];
        for (int i = 0; i < numLights; i++)
        {
            offsets[i] = Random.value * 2 * Mathf.PI;
            mults[i] = Random.value;
        }
    }

    // Link a trigger tile
    public void LinkTile(Transform tile)
    {
        TTile = tile.GetComponent<TriggerTile>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //On trigger, flag activated
        if (TTile.GetTriggerStatus())
        {
            SetIntensity(strength);
            activated = true;
        }
        //Dim if not directly being triggered 
        else if (activated)
            SetIntensity(dim);
        else
            SetIntensity(0f);

        //Modulate spot angles
        //Random angle from min to max, inclusive
        for (int i = 0; i < numLights; i++)
        {
            Lights[i].spotAngle = mean + Mathf.Sin(Time.time*mults[i] + offsets[i]) * var;
        }

    }

    //Set intensity of all lights
    private void SetIntensity(float intense)
    {
        for (int i = 0; i < numLights; i++)
        {
            Lights[i].intensity = intense;
        }
    }
}
