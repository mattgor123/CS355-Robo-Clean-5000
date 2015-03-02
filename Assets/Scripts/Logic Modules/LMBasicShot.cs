using UnityEngine;
using System.Collections;

// Logic Module: Basic Shot
// fires a shot if cooldown has elapsed
public class LMBasicShot : MonoBehaviour, LMAttack {
    public Vector3 AttackLogic(EnemyController enemy, GameObject player)
    {
        // Start firing if timer has elapsed & player is in aggro range
        if (LMHelper.CheckAttackTimer(enemy) && LMHelper.IsInRange(enemy, player))
        {
            enemy.GetComponent<WeaponBackpackController>().StartFiring();
        }
        else
        {
            enemy.GetComponent<WeaponBackpackController>().StopFiring();
        }

        //Face the player
        Vector3 facing = LMHelper.BaseMoveLogic(enemy, player);
        //Offset slightly since gun is off-center
        Vector3 perp = Vector3.Cross(facing, new Vector3(0f, 1f, 0f)); //left perpendicular
        facing = (facing + perp * 0.05f);
        facing.Normalize();
        return facing;
    }
}
