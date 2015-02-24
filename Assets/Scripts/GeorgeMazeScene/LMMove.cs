using UnityEngine;
using System.Collections;


/*Parent Logic Module: Move (Primary)
 * Inherited by all movement Logic Modules
 * These movements take priority over auxiliary movements
 * and are things like chasing the player
 */ 
public interface LMMove {

    Vector3 MoveLogic(EnemyController enemy, GameObject player);
    
}
