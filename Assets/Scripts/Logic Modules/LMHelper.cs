using UnityEngine;
using System.Collections;


// Logic Module: Helper
// Contains assorted helper code for various logic modules to prevent re-implementation of code
public class LMHelper {

    //Calculates the normalized direction to target
    public static Vector3 BaseMoveLogic(EnemyController enemy, GameObject player)
    {
        return DistNoY(enemy.gameObject, player);
    }

    //Get distance without any Y
    public static Vector3 DistNoY(GameObject enemy, GameObject player)
    {
        float x = player.transform.position.x - enemy.transform.position.x;
        float z = player.transform.position.z - enemy.transform.position.z;

        Vector3 mvt = new Vector3(x, 0.0f, z);
        mvt.Normalize();
        return mvt;
    }

    //Checks whether target is in range
    public static bool IsInRange(EnemyController enemy, GameObject player)
    {
        float dist = GetDistance(enemy, player);
        if (dist <= enemy.GetAggroRadius())
        {
            return true;
        }
        return false;
    }

    //Checks whether target can be shot
    public static bool CanShoot(EnemyController enemy, GameObject player)
    {
        float dist = GetDistance(enemy, player);
        if (IsInRange(enemy, player))
        {
            //Only shoot if in range and in sight
            RaycastHit hit;
            Vector3 dir = player.transform.position - enemy.transform.position;
            if (Physics.Raycast(enemy.transform.position, dir, out hit, dist))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Gets the distance
    public static float GetDistance(EnemyController enemy, GameObject player)
    {
        //Ignore distance along vertical (y) axis
        Vector3 pp = player.transform.position;
        pp = new Vector3(pp.x, 0f, pp.z);
        Vector3 ep = enemy.transform.position;
        ep = new Vector3(ep.x, 0f, ep.z);

        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        return dist;
    }

    //Calculates whether attack cooldown has elapsed
    // if so, resets attack timer
    // otherwise, increments attack timer
    public static bool CheckAttackTimer(EnemyController enemy)
    {
        float CD = enemy.GetAttackCD();
        float timer = enemy.GetAttackTimer();
        if (timer >= CD)
        {
            enemy.SetAttackTimer(0f);
            return true;
        }
        else
        {
            timer += Time.deltaTime;
            enemy.SetAttackTimer(timer);
            return false;
        }
    }

}
