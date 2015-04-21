using UnityEngine;
using System.Collections;

//Generic Shield Controller 
//Absorbs incoming damage when active
//Attach to a shield object; assumes the thing with the health controller is direct parent
public class ShieldController : MonoBehaviour {

    [SerializeField]
    private float Radius;               //Radius of shield sphere

    [SerializeField]
    private float MaxShield;            //Maximum shield value

    [SerializeField]
    private float ShieldRegen;          //Shield regen value

    [SerializeField]
    private float RegenCD;              //Cooldown on increased shield regen

    private float CurrShield;           //Current shield value
    private float RegenTimer = 0;       //Increased regen timer
    private bool Active = true;         //Whether shields are active

    private HealthController ParentHC;  //Parent's shield-capable health controller
    private ParticleSystem PS;          //Shield effect system


	// Use this for initialization
	void Start () {
        CurrShield = MaxShield;
        ParentHC = gameObject.GetComponentInParent<HealthController>();
        PS = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        Regen();

        //Scale number of particles by the strength of the shield
        PS.maxParticles = Mathf.FloorToInt(10* (CurrShield / MaxShield));
	}

    //Regenerate shields        
    //  quadruple regen after not getting hit for RegenCD time
    private void Regen()
    {
        //No regen when inactive
        if (!Active) 
            return;            

        float rg = ShieldRegen * Time.deltaTime;
        if (RegenTimer > 0)
            RegenTimer -= Time.deltaTime;
        else
            rg *= 4;
        CurrShield += rg;

        if (CurrShield > MaxShield)        
            CurrShield = MaxShield;            
        
    }

    // Block damage (negative amount)
    //  returns amount unblocked
    public float Block(float dmg)
    {
        //If inactive, do not block
        if (!Active)        
            return dmg;
        

        float net = dmg;
        //insufficient shields, block as much as possible
        if (CurrShield < Mathf.Abs(net))
        {
            net += CurrShield;
            CurrShield = 0;
            return net;
        }
        else
        {
            CurrShield += net;
            return 0;
        }
    }

    //Get current shields
    public float GetShield()
    {
        return CurrShield;
    }

    //Activate shields
    public void Activate()
    {
        if (!Active)
        {
            Active = true;
            PS.Play();
        }
    }

    //Deactivates shields
    public void Deactivate()
    {
        if (Active)
        {
            Active = false;
            PS.Stop();
        }
    }
}
