using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player;

    private bool TrackFace; //Whether to track player facing (and be behind player) or not

	private Vector3 offset;
    private Vector3 offset2;
    private Vector3 offsetDC;
    private float SC;           //side correction distance variable
    private Vector3 facedown;   //angle facing down
    private bool isShaking;
    private Vector3 positionBeforeShake;
    private int shakeIntensity;
    private float shakeTime;
    private float shakeCountdown;

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        offset = new Vector3(0f, 10f, -5f);//transform.position;
        offset2 = new Vector3(0f, 10f, 0f);  //vertical offset for track face
        facedown = new Vector3(45f, 0f, 0f);
        TrackFace = true;
        
        //Dynamic Camera variables
        SC = 0.6f;
        offsetDC = offset2 * 0.5f;

        shakeIntensity = 1;
        shakeTime = 2;

	}

	void LateUpdate () {
        if (this.isShaking)
        {
            transform.position = positionBeforeShake + Random.insideUnitSphere * shakeIntensity;
            this.shakeCountdown -= Time.deltaTime;
            if (this.shakeCountdown < 0)
            {
                this.isShaking = false;
                //GameObject.FindObjectOfType<NewRoomTrigger>().nextLevel(); ;
                //GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
                //stagebuilder.GetComponent<StageBuilder>().nextLevel();
            }
            return;
        }
	
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

            //raycat from player to camera and swee if something is in the way
            if (Physics.Raycast(core + axis, axis, out hit, CamDist))
            {
                //if so, shift forward by sufficient distance to clear (maximum ~5)
                HitDist = hit.distance;
                Vector3 delta = Player.transform.forward * (CamDist - HitDist) / Mathf.Sqrt(3f);
                transform.position += (1.2f)*delta;

                //tilt down to keep player in view 
                Vector3 tilt = new Vector3 (3f*delta.magnitude, 0f, 0f);
                transform.Rotate(tilt);

                //zoom in slightly
                Vector3 zoom = new Vector3 (0f, -1f*delta.magnitude, 0f);
                transform.position += zoom;                
            }
            //check to the left/right & shift camera slightly to prevent clipping on the side
            Vector3 cross = Vector3.Cross(axis, offsetDC.normalized);
            cross.Normalize();

            if (Physics.Raycast(transform.position - axis, cross, out hit, CamDist)) {                
                if (hit.distance < SC)
                {
                    Vector3 shift = -2.0f * (SC/2 - Mathf.Abs(SC/2 - hit.distance)) * cross;
                    transform.position += shift;
                }
            }
            else if (Physics.Raycast(transform.position - axis, -1f * cross, out hit, CamDist))
            {
                if (hit.distance < SC)
                {
                    Vector3 shift = 2.0f * (SC / 2 - Mathf.Abs(SC / 2 - hit.distance)) * cross;
                    transform.position += shift;
                }
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

    public void shake()
    {
        this.positionBeforeShake = transform.position;
        this.shakeCountdown = this.shakeTime;
        this.isShaking = true;
    }
        
}
