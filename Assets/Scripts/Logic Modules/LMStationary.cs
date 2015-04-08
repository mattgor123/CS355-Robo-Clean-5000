using UnityEngine;
using System.Collections;

// Logic Module: Hold Range 
// tries to stay within some optimal range band with respect to target
public class LMStationary : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        float dist = LMHelper.GetDistance(enemy, player);
        Vector3 mvt = new Vector3(0f, 0f, 0f);
        enemy.SetStationary(true);

        // If out of range, move into range
        if (dist <= enemy.GetAggroRadius())
        {
            enemy.SetAggroState(true);
        }
        else {
            enemy.SetAggroState(false);
        }
        return mvt;
    }
}
