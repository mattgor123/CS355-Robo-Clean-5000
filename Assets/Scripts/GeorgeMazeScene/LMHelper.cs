using UnityEngine;
using System.Collections;


// Logic Module: Helper
// Contains assorted helper code for various logic modules to prevent re-implementation of code
public class LMHelper {

    public static Vector3 BaseMoveLogic(EnemyController enemy, GameObject player)
    {
        float x = player.transform.position.x - enemy.transform.position.x;
        float z = player.transform.position.z - enemy.transform.position.z;

        Vector3 mvt = new Vector3(x, 0.0f, z);
        mvt.Normalize();
        return mvt;
    }

}
