using UnityEngine;
using System.Collections;

// Logic Module: Long Patrol
//  patrols around, moving in direction of longest open distance
public class LMLongPatrol : MonoBehaviour, LMAuxMove {

    private Vector3 dir;        //current direction 
    private Vector3[] SearchDirs;   //cardinal directions to search in

    [SerializeField]
    private float ReAcTime;     //reacquisition time (to find new direction)
    
    [SerializeField]
    private float NumDirs;      //total number of directions to search in

    void Start()
    {
        //start with random direction
        float x = (Random.value - 0.5f);
        float z = (Random.value - 0.5f);
        dir = new Vector3(x, 0f, z);
        dir.Normalize();

        //Setup search directions & reacq time
        ReAcTime = 2;
        NumDirs = 6;
        SearchDirs = new Vector3[4];
        SearchDirs[0] = new Vector3(1f, 0f, 0f);
        SearchDirs[1] = new Vector3(-1f, 0f, 0f);
        SearchDirs[2] = new Vector3(0f, 0f, 1f);
        SearchDirs[3] = new Vector3(0f, 0f, -1f);        
    }

    public Vector3 AuxMoveLogic(EnemyController enemy, GameObject player)
    {
        //Do raycast in a few directions and choose the one with the greatest distance
        //Add some randomness to reacquisition timer: *(1 +- 0.25)
        float time = ReAcTime*(1 + 0.5f*(Random.value - 0.5f));
        if (Time.time % time <= 0.02)
        {
            Vector3 ndir;
            Vector3 bestdir = dir;        //largest direction
            RaycastHit hit;
            float best = 0;                 //largest distance
            float dist = 0;

            for (int i = 0; i < NumDirs; i++) {
                //Search in 4 cardinal directions first
                //then in random directions as applicable
                if (i < 4) 
                    ndir = SearchDirs[i];
                else
                {
                    float x = (Random.value - 0.5f);
                    float z = (Random.value - 0.5f);
                    ndir = new Vector3(x, 0f, z);
                }
                
                //Perform raycast to see distance to a thing
                if (Physics.Raycast(transform.position, ndir, out hit))
                {
                    dist = hit.distance;
                }

                //Take the furthest distance
                if (dist > best)
                {
                    best = dist;
                    bestdir = ndir;
                }                    
            }
            dir = bestdir;
        }
        return dir;
        
    }
}
