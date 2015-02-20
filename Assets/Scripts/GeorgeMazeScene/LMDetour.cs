using UnityEngine;
using System.Collections;

// Logic Module: Detour
// chases target if in aggro range, and tries to go around walls
public class LMDetour : MonoBehaviour, LMMove {

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (dist <= enemy.GetAggroRadius())
        {
            float pm = enemy.GetPrevMvt().magnitude;
            Vector3 mvt;

            // If hitting a wall and moving slowly, take a detour sideways
            // (prevents behavior if out in the open or if it happens to graze a wall while moving rapidly)
            if (enemy.GetWallHit() && pm <= .05)
            {
                Vector3 direct = LMHelper.BaseMoveLogic(enemy, player);              //first get direct approach
                mvt = Vector3.Cross(direct, new Vector3(0f, 1f, 0f));                  //cross with vertical to get left perpendicular

                if (Time.time % 4 == 0)
                {
                    mvt *= -1;                                      //flip direction every 5 seconds
                }

                mvt -= direct * (Random.value - 0.5f);              //take random angle from straight perpendicular
                mvt.Normalize();                                 //re-normalize

            }
            else // otherwise chase as normal
            {
                mvt = LMHelper.BaseMoveLogic(enemy, player);
            }
            return mvt;
        }
        return new Vector3(0f, 0f, 0f);
    }
}
