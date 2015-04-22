using UnityEngine;
using System.Collections;

//Logic Module: Damaging Shield
// deals damage to colliding foes when the shield is active
public class LMDamagingShield : MonoBehaviour {

    [SerializeField]
    private float Damage = -50f;

    private ShieldController Shield;
    private bool ShieldActive;


	// Use this for initialization
	void Start () {
        Shield = GetComponentInChildren<ShieldController>();
	}

    void LateUpdate()
    {
        ShieldActive = Shield.GetActive();
    }

    //Deal some damage on initial collision
    void OnCollisionEnter(Collision other)
    {
        //No damage if shield is off
        if (!ShieldActive)
            return;

        GameObject OGO = other.gameObject;
        if (OGO.tag == "Player")
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage);
        }
    }

    //Deal some sustained damage on continued collision
    void OnCollisionStay(Collision other)
    {
        if (!ShieldActive)
            return;

        GameObject OGO = other.gameObject;
        if (OGO.tag == "Player")
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage * 0.1f);
        }
        
    }
}
