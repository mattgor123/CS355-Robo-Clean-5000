using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private GameObject Player;

	private float offsetF;    //Forward axis offset mutiplier
    private Vector3 offsetV;    //Vertical axis offset
    private Vector3 facedown;   //angle facing down
    private Vector3 offsetDC;   //position offset for raycasting from core

    private float SC = 0.6f;           //side correction distance variable
    private float FC = 1.0f;           //forward correction modifier
    private float ZC = -0.3f;           //zoom correction modifier
    private float TC = 3.5f;           //tilt correction modifier 


    private bool isShaking;
    private Vector3 positionBeforeShake;
    private int shakeIntensity;
    private float shakeTime;
    private float shakeCountdown;

	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        offsetF = 2.5f;
        offsetV = new Vector3(0f, 4f, 0f); 
        facedown = new Vector3(30f, 0f, 0f);
        
        //Dynamic Camera variables
        offsetDC = offsetV * 0.5f;

        shakeIntensity = 1;
        shakeTime = 2;

	}

	void LateUpdate () {
        if (this.isShaking)
        {
            transform.position = positionBeforeShake + Random.insideUnitSphere * shakeIntensity;
            //this.shakeCountdown -= Time.deltaTime;
            if (Time.realtimeSinceStartup > this.shakeCountdown)
            {
                this.isShaking = false;
                this.shakeCountdown = 0;
                //GameObject.FindObjectOfType<NewRoomTrigger>().nextLevel(); ;
                //GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
                //stagebuilder.GetComponent<StageBuilder>().nextLevel();
            }
            return;
        }

        offsetDC = offsetV * 0.25f;

        //Stay behind the player, facing in same direction
        transform.position = Player.transform.position;
        transform.forward = Player.transform.forward;
        transform.position += offsetV - Player.transform.forward * offsetF;
        transform.Rotate(facedown);

        //Dynamic Camera (to prevent clipping through walls)
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
            //if so, shift forward by sufficient distance to clear (maximum ~5)
            HitDist = hit.distance;
            Vector3 delta = Player.transform.forward * (CamDist - HitDist) / Mathf.Sqrt(3f);
            transform.position += FC*delta;

            //tilt down to keep player in view 
            Vector3 tilt = new Vector3 (TC*delta.magnitude, 0f, 0f);
            transform.Rotate(tilt);

            //zoom in slightly
            Vector3 zoom = new Vector3 (0f, ZC*delta.magnitude, 0f);
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
    
    public void shake()
    {
        this.positionBeforeShake = transform.position;
        this.shakeCountdown = this.shakeTime + Time.realtimeSinceStartup;
        this.isShaking = true;
    }
        
}
