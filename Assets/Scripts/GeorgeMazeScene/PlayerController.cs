    using UnityEngine;
using System.Collections;

public class GeorgePlayerController : MonoBehaviour {
	[SerializeField]
	private float speed;

    [SerializeField]
    private GeorgeCameraController cam;

    private float HP;

	void Start() {
        HP = 25;
	}

	//Physics Code
	void FixedUpdate () {
        if (HP > 0)
        {

            float moveVertical = Input.GetAxis("Vertical");    //forward movement; turning linked to camera

            float angle = cam.getAngle() * Mathf.PI / 180;

            float z = Mathf.Cos(angle) * moveVertical;
            float x = Mathf.Sin(angle) * moveVertical;

            Vector3 mvt = new Vector3(x, 0.0f, z);
            rigidbody.AddForce(mvt * speed * Time.deltaTime);
        }
	}

	void OnTriggerEnter(Collider other) {
    
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {
            HP -= 1;
        }   
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Enemy")
        {
            HP -= 0.2f;
        }
    }

    public float getHP()
    {
        return HP;
    }

}