using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player;

    private bool TrackFace; //Whether to track player facing (and be behind player) or not

	private Vector3 offset;

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0f, 10f, -5f);//transform.position;
        TrackFace = false;
	}

	void LateUpdate () {
        
		transform.position = Player.transform.position + offset;

        if (TrackFace)
        {
            transform.position = Player.transform.position;
            transform.forward = Player.transform.forward;
            transform.position += new Vector3(0f, 3f, 0f) + (0.5f)*transform.forward;
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
