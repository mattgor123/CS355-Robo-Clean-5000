using UnityEngine;
using System.Collections;

// Logic Module: Hold Range 
// tries to stay within some optimal range band with respect to target
public class LMHoldRange : MonoBehaviour, LMMove {


    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {       
        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        float optrng = enemy.GetOptimalRange();
        float delta = dist - optrng;

        // If outside of optimal range band (optimal += 25%), move towards optimal range
        if (Mathf.Abs(delta) / optrng >= 0.20)
        {
            Vector3 mvt = MoveLogic(enemy, player);
            mvt *= delta / (1.5f * optrng);
            return mvt;
        }
        return new Vector3(0f, 0f, 0f);
    }
}
