using UnityEngine;
using System.Collections;

//Logic Module: Damaging Shield
// deals damage to colliding foes when the shield is active
public class LMDamagingShield : MonoBehaviour {

    [SerializeField]
    private float Damage = -50f;

    private ShieldController Shield;

	// Use this for initialization
	void Start () {
        Shield = GetComponentInChildren<ShieldController>();
	}

    //Deal some damage on initial collision
    void OnCollisionEnter(Collision other)
    {
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
        GameObject OGO = other.gameObject;
        if (OGO.tag == "Player")
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage * 0.1f);
        }
        
    }
}
