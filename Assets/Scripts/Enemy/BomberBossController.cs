using UnityEngine;
using System.Collections;

//Bomber Boss auxiliary logic
public class BomberBossController : MonoBehaviour {

    Transform Core;
    private float LaunchDelay = 1.0f;
    private Vector3 Delta = new Vector3(0f, 1f, 0f);

    //Start with bomber deactivated at rest
	void Awake () {
        Core = gameObject.GetComponent<Transform>();
        PauseAI();
	}
	

	// Update is called once per frame
	void Update () {
        Debug.Log(LaunchDelay);

        if (Core.position.y > 4)        
            ResumeAI();        
        if (LaunchDelay < 0)        
            Core.position += Delta * Time.deltaTime;        
        else 
            LaunchDelay -= Time.deltaTime;      
	}

    private void PauseAI()
    {
        GetComponent<LMLongPatrol>().enabled = false;
        GetComponent<LMStrafingRun>().enabled = false;
        GetComponent<EnemyController>().enabled = false;
    }

    private void ResumeAI()
    {
        GetComponent<LMLongPatrol>().enabled = true;
        GetComponent<LMStrafingRun>().enabled = true;
        GetComponent<EnemyController>().enabled = true;
        gameObject.GetComponent<BomberBossController>().enabled = false;
    }

    public void SetLaunchDelay(float dl)
    {
        LaunchDelay = dl;
    }

}


