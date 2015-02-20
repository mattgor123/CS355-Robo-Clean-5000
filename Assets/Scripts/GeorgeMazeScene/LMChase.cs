using UnityEngine;
using System.Collections;

// Logic Module: Chase
// chases target if in aggro radius
public class LMChase : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (dist <= enemy.GetAggroRadius())
        {
            return LMHelper.BaseMoveLogic(enemy, player);
        }
        return new Vector3 (0f, 0f, 0f);
    }
	
}