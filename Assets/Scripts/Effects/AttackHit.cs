using UnityEngine;
using System.Collections;

public class AttackHit : MonoBehaviour {

    private ParticleSystem PS;
    private Transform Core;

	void Start () {
        PS = GetComponent<ParticleSystem>();
        PS.Stop();
        Core = GetComponentInParent<Transform>();
	}
	
    public void GetHit(Vector3 pos) {
        transform.position = pos;
        PS.Play();
    }

}
