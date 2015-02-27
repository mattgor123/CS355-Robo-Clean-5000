using UnityEngine;
using System.Collections;

// Logic Module: Basic Shot
// fires a shot if cooldown has elapsed
public class LMBasicShot : MonoBehaviour, LMAttack {
    public void AttackLogic(EnemyController enemy, GameObject player)
    {
        // Start firing if timer has elapsed
        if(LMHelper.CheckAttackTimer(enemy)) {
            enemy.GetComponent<WeaponBackpackController>().StartFiring();
        }
        else
        {
            enemy.GetComponent<WeaponBackpackController>().StopFiring();
        }
    }
}
