﻿using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	[SerializeField] 
    private float life;

	//[SerializeField] 
    //private Light ourlight;

    [SerializeField]
    private int splashDamage;

    //radius of explosion, for splash damage
    [SerializeField]
    private int radius;

    [SerializeField]
    private AudioClip sound_effect;

	private float start_time;
	private float max_intensity;

	private void Start () {
        if (sound_effect != null) {
            AudioSource.PlayClipAtPoint(sound_effect, transform.position);
        }
		start_time = Time.time;
		//max_intensity = this.ourlight.intensity;

		//this.ourlight.intensity = 0.0f;

        //explosion splash damage
        Collider[] intersect = Physics.OverlapSphere(gameObject.transform.position, radius);
        foreach (Collider c in intersect) {
            if (c.gameObject.tag == "Enemy")
            {
                HealthController hc = c.gameObject.GetComponent<HealthController>();
                hc.ChangeHealth(-splashDamage);
                //Debug.Log("SPLASH");
            }
        }
	}
	
	private void Update () {
		if(Time.time - start_time > life) {
			Destroy(gameObject);
		}
		else {
			var time_elapsed = Time.time - start_time;
			var scale = life / time_elapsed;
			if(time_elapsed > life - 0.02) {
				scale = 1.0f;
			}
			gameObject.transform.localScale = new Vector3(scale, scale, scale);
			//this.ourlight.intensity = max_intensity * scale;
		}
	}
}
