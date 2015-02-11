using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private float AggroRadius;

    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= AggroRadius)
        {
            float x = player.transform.position.x - transform.position.x;
            float z = player.transform.position.z - transform.position.z;

            Vector3 mvt = new Vector3(x, 0.0f, z);
            mvt.Normalize();
            rigidbody.AddForce(mvt * speed * Time.deltaTime);
        }

	}
}
