using UnityEngine;
using System.Collections;


/* Parent Logic Module: Auxiliary Move
 * inherited by all auxiliary movement Logic Modules
 * These only run when the primary movement module returns zero movement
 *  and are low-priority patterns like patrolling.
 */
public interface LMAuxMove
{
    Vector3 AuxMoveLogic(EnemyController enemy, GameObject player);

}
