using UnityEngine;
using System.Collections;

public class GeorgeCameraController : MonoBehaviour {
	[SerializeField]
	private GeorgePlayerController player;

    [SerializeField]
    private float turnrate;

    [SerializeField]
    private float n_offset;

	private Vector3 offset;
    private float height;
    private float angle; //in degrees

	// Use this for initialization
	void Start () {
		offset = transform.position;
        height = transform.position.y;
        angle = 0;
	}

    void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        angle = transform.eulerAngles.y + turn*turnrate;
        if (angle > 360){
            angle -= 360;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, 0);  //Rotate to right facing

        float z = Mathf.Cos(angle * Mathf.PI/180)*n_offset;
        float x = Mathf.Sin(angle * Mathf.PI/180) * n_offset;
        offset = new Vector3(x, height, z);
    }

	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + offset;
	}

    public float getAngle()
    {
        return angle;
    }
}
