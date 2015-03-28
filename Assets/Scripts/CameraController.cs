using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player;

    private bool TrackFace; //Whether to track player facing (and be behind player) or not

	private Vector3 offset;
    private Vector3 offset2;
    private Vector3 offsetDC;

    private Vector3 facedown;   //angle facing down

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0f, 10f, -5f);//transform.position;
        offset2 = new Vector3(0f, 10f, 0f);  //vertical offset for track face
        offsetDC = offset2 * 0.5f;
        facedown = new Vector3(45f, 0f, 0f);
        TrackFace = true;
	}

	void LateUpdate () {        		
        if (TrackFace)
        {
            //Stay behind the player, facing in same direction
            transform.position = Player.transform.position;
            transform.forward = Player.transform.forward;
            transform.position += offset2 - Player.transform.forward * (5f);
            transform.Rotate(facedown);

            //Dynamic Camera zoom (to prevent clipping through walls)
            //setup connecting axis
            Vector3 core = Player.transform.position + offsetDC;
            Vector3 axis = transform.position - core;
            axis.Normalize();
            float CamDist = Vector3.Distance(transform.position, core);
            float HitDist;
            RaycastHit hit;

            //raycat from player to camera and see if something is in the way
            if (Physics.Raycast(core + axis, axis, out hit, CamDist))
            {
                //if so, shift forward by sufficient distance to clear      
                HitDist = hit.distance;
                Vector3 delta = Player.transform.forward * (CamDist - HitDist) / Mathf.Sqrt(3f);
                transform.position += delta;

                //tilt down to keep player in view 
                Vector3 tilt = new Vector3 (3f*delta.magnitude, 0f, 0f);
                transform.Rotate(tilt);
                Debug.Log(tilt);
            }
        }
        else
        {
            transform.position = Player.transform.position + offset;
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
