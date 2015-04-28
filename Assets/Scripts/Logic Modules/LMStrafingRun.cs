using UnityEngine;
using System.Collections;

//Logic Module: Strafing Run
//Combined primary movement & attack module
//Strafes across player & drops fire bombs while strafing
public class LMStrafingRun : MonoBehaviour, LMMove, LMAttack {

    [SerializeField]
    private Transform FireBomb;           //The attack dropped

    [SerializeField]
    private float EBDamage = -50f;        //Damage 

    private Vector3 StrafeTarget;       //The target end location of a strafing run
    private float distST = 0f;               //distance to strafe target

    private bool Strafing = false;     //whether strafing is active
    private float Side = 2.0f;           //which side to drop on (+ right, - left)

    private Vector3 Zero = new Vector3(0f, 0f, 0f);

    void Update()
    {
        if (Strafing)
        {
            distST = (StrafeTarget - this.transform.position).magnitude;
        }
    }

    public Vector3 MoveLogic(EnemyController enemy, GameObject player)
    {
        float dist = LMHelper.GetDistance(enemy, player);
        Vector3 mvt = new Vector3(0f, 0f, 0f);

        // If in aggro range, set aggro state to true
        if (dist <= enemy.GetAggroRadius())
        {
            enemy.SetAggroState(true);

            //if not already strafing or reached strafe target, setup new one
            if (Strafing == false || distST < 5) {
                Strafing = true;

                //Raycast out in direction of player
                Vector3 dir = LMHelper.BaseMoveLogic(enemy, player);
                this.transform.forward = dir;
                RaycastHit hit;
                float tdist = 0;
                if (Physics.Raycast(this.transform.position, dir, out hit))
                {
                    tdist = hit.distance;
                }
                StrafeTarget = this.transform.position + dir * tdist;
                mvt = dir;
            }
            
            //else, continue along strafing path towards target
            else
            {
                mvt = (StrafeTarget - this.transform.position).normalized;
            }            

        }
        else {
            enemy.SetAggroState(false);
            Strafing = false;
        }

        //Debug.Log(Strafing + "," + StrafeTarget + "," + mvt);

        return mvt;
    }


    public Vector3 AttackLogic(EnemyController enemy, GameObject player)
    {

        // Drop bombs if cooldown elapsed & is strafing
        // flip sides per drop
        if (LMHelper.CheckAttackTimer(enemy) && Strafing)
        {
            Vector3 corepos = enemy.transform.position;
            DropBomb(corepos + enemy.transform.right * Side);
            Side *= -1;
            
        }

        return Zero;
    }

    //Instantiate & setup a bomb
    public void DropBomb(Vector3 pos)
    {        
        Transform bomb = Instantiate(FireBomb);
        bomb.transform.position = new Vector3(pos.x, 0f, pos.z);
        FireBomb fb = bomb.GetComponent<FireBomb>();
        fb.SetDamage(EBDamage);
    }

    public void SetDamage(float dmg)
    {
        EBDamage = dmg;
    }
}
    




