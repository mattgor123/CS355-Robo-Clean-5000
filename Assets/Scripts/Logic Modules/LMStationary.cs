using UnityEngine;
using System.Collections;

// Logic Module: Stationary
// do not move
public class LMStationary : MonoBehaviour, LMMove {

    private Vector3 Position;

    void Start()
    {
        Position = gameObject.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = Position;
    }

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {      
        float dist = LMHelper.GetDistance(enemy, player);
        Vector3 mvt = new Vector3(0f, 0f, 0f);
        enemy.SetStationary(true);

        // Aggro if in range
        if (dist <= enemy.GetAggroRadius())
        {
            enemy.SetAggroState(true);
        }
        else {
            enemy.SetAggroState(false);
        }
        return mvt;
    }
}
