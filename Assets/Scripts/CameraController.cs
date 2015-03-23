using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player;

    private bool TrackFace; //Whether to track player facing (and be behind player) or not

	private Vector3 offset;
    private Vector3 offset2;

    private Vector3 facedown;   //angle facing down

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0f, 10f, -5f);//transform.position;
        offset2 = new Vector3(0f, 10f, 0f);
        facedown = new Vector3(45f, 0f, 0f);
        TrackFace = false;
	}

	void LateUpdate () {
        
		transform.position = Player.transform.position + offset;

        if (TrackFace)
        {
            transform.position = Player.transform.position;
            transform.forward = Player.transform.forward;
            transform.position += offset2 - Player.transform.forward * (5f);
            transform.Rotate(facedown);
        }
        else
        {
            //this.transform.forward = Player.transform.position.x;
            transform.LookAt(Player.transform);
            transform.Translate(Vector3.right * Time.deltaTime);
            transform.forward = Player.transform.forward;
            transform.rotation = Quaternion.AngleAxis(60f, Vector3.right);
        }


	}

    public void SetTrackFace(bool set)
    {
        TrackFace = set;
    }
}
