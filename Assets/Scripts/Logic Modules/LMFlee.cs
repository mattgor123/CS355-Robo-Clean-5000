using UnityEngine;
using System.Collections;

// Logic Module: Flee
// runs away from target
public class LMFlee : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        return -1 * LMHelper.BaseMoveLogic(enemy, player);
    }
}
