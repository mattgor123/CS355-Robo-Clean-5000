using UnityEngine;
using System.Collections;

/* Parent Logic Module: Attack
 * Inherited by all attack Logic Modules
 */
public interface LMAttack {
    //Returns the facing during the attack
    Vector3 AttackLogic(EnemyController enemy, GameObject player);
}
