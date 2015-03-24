using UnityEngine;
using System.Collections;

// Logic Module: Patrol
//  patrols around randomly
public class LMPatrol : MonoBehaviour, LMAuxMove {

    private Vector3 dir; 

    void Start()
    {
        float x = (Random.value - 0.5f);
        float z = (Random.value - 0.5f);
        dir = new Vector3(x, 0f, z);
        dir.Normalize();
    }

    public Vector3 AuxMoveLogic(EnemyController enemy, GameObject player)
    {
        if (Time.time % 2 <= 0.02)
        {
            float x = (Random.value - 0.5f);
            float z = (Random.value - 0.5f);
            dir = new Vector3(x, 0f, z);
        }
        return dir;
        
    }
}
