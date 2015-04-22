using UnityEngine;
using System.Collections;

public class FireCollider : MonoBehaviour {

    [SerializeField]
    private float Damage;

    void OnTriggerEnter(Collider other)
    {
        //Deal damage on initial contact; hits enemies too
        GameObject OGO = other.gameObject;
        if (OGO.tag == "Player")
        {
            HealthController HC = OGO.GetComponent<HealthController>();
            HC.ChangeHealth(Damage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        {
            GameObject OGO = other.gameObject;
            if (OGO.tag == "Player")
            {
                HealthController HC = OGO.GetComponent<HealthController>();
                HC.ChangeHealth(Damage * 0.1f);
            }
        }
    }

}
