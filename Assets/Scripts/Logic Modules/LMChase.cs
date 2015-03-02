using UnityEngine;
using System.Collections;

// Logic Module: Chase
// chases target if in aggro radius
public class LMChase : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        if (LMHelper.IsInRange(enemy, player))
        {
            enemy.SetAggroState(true);  //aggroed if within aggro range
            return LMHelper.BaseMoveLogic(enemy, player);
        }
        enemy.SetAggroState(false);
        return new Vector3 (0f, 0f, 0f);
    }
	
}