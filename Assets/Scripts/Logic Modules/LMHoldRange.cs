using UnityEngine;
using System.Collections;

// Logic Module: Hold Range 
// tries to stay within some optimal range band with respect to target
public class LMHoldRange : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        float dist = LMHelper.GetDistance(enemy, player);
        float optrng = enemy.GetOptimalRange();
        float delta = dist - optrng;    //difference from optimal range
        Vector3 mvt = new Vector3(0f, 0f, 0f);

        // If out of range, move into range
        if (dist <= enemy.GetAggroRadius())
        {
            enemy.SetAggroState(true);
            if (delta > 0) { 
                mvt = LMHelper.BaseMoveLogic(enemy, player);
                mvt *= delta / (1.5f * optrng);
            }
        }
        else {
            enemy.SetAggroState(false);
        }
        return mvt;
    }
}
