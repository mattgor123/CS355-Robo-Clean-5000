using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	[SerializeField] private float life;
	[SerializeField] private Light light;

	private float start_time;
	private float max_intensity;

	private void Start () {
		start_time = Time.time;
		max_intensity = light.intensity;
		light.intensity = 0.0f;
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
