using UnityEngine;
using System.Collections;

/* Parent Logic Module: Attack
 * Inherited by all attack Logic Modules
 */
public interface LMAttack {
    void AttackLogic(EnemyController enemy, GameObject player);
}
