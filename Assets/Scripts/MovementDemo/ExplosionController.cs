using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	[SerializeField] 
    private float life;

	[SerializeField] 
    private Light light;

    [SerializeField]
    private int splashDamage;

    //radius of explosion, for splash damage
    [SerializeField]
    private int radius;

	private float start_time;
	private float max_intensity;

	private void Start () {
		start_time = Time.time;
		max_intensity = light.intensity;
		light.intensity = 0.0f;

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
			light.intensity = max_intensity * scale;
		}
	}
}
