using UnityEngine;
using System.Collections;


//Parent Logic Module: Move
// inherited by all movement Logic Modules
public interface LMMove {

    //[SerializeField] protected EnemyController Enemy;

    //[SerializeField] protected GameObject Player;

    Vector3 MoveLogic(EnemyController enemy, GameObject player);/* {
        return new Vector3(0f, 0f, 0f);
    }*/
    
}
