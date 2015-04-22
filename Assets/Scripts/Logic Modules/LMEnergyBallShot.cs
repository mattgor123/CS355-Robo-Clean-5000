using UnityEngine;
using System.Collections;

// Logic Module: Energy Ball shot
// fires an energy ball attack if cooldown has elapsed
public class LMEnergyBallShot : MonoBehaviour, LMAttack
{
    #region SF
    [SerializeField]
    private Transform EnergyBall;   //Energy Ball prefab

    [SerializeField]
    private float X = 0.21f;                //X offset from transform center
    [SerializeField]
    private float Y = 1.65f;                //Y offset
    [SerializeField]
    private float Z = 3.5f;                //Z offset

    [SerializeField]
    private float BurstDuration = 0.5f;     //duration of finishing burst

    #endregion

    private Vector3 OffsetY;                //vertical offset


    void Start()
    {
        OffsetY = new Vector3(0f, Y, 0f);   
    }

    public Vector3 AttackLogic(EnemyController enemy, GameObject player)
    {
        Vector3 target = LMHelper.BaseMoveLogic(enemy, player);

        // Fire a shot if timer elapsed and in range
        if (LMHelper.CheckAttackTimer(enemy) && LMHelper.CanShoot(enemy, player))
        {
            //instantiate the shot & position at the end of barrel
            Transform shot = Instantiate(EnergyBall);
            shot.position = enemy.transform.position + enemy.transform.forward * Z + OffsetY + enemy.transform.right * X;
            Vector3 dir = LMHelper.DistNoY(shot.gameObject, player);
            dir.Normalize();

            EnergyBall eb = shot.GetComponent<EnergyBall>();
            eb.SetInitDirection(dir);
            eb.SetBurstDuration(BurstDuration);
        }

        return target;
    }
}
