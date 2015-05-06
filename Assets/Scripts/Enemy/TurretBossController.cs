using UnityEngine;
using System.Collections;

//Additional functionality for turret boss
//works in tandem with generic Enemy Controller
public class TurretBossController : MonoBehaviour {

    private EnemyController EC;
    private ShieldController Shield;
    private HealthController HC;
    private LMEnergyBallShot EBS;

    [SerializeField]
    private float ShieldActiveCD = 15f;     //Cooldown before shields restart
    
    private float SATimer = 0;              //timer for ^
    private bool Enrage = false;            //enrage status

	void Start () {
        EC = GetComponent<EnemyController>();
        Shield = GetComponentInChildren<ShieldController>();
        HC = GetComponent<HealthController>();
        EBS = GetComponent<LMEnergyBallShot>();
	}
	
	void Update () {

        //If shields have been taken down, deactivate for a brief period of time
        if (Shield.GetShield() == 0 && SATimer == 0)
        {
            Shield.Deactivate();
            SATimer = ShieldActiveCD;
        }

        //decrement timer; on reaching zero, reactivate shields at 25%
        if (SATimer > 0)
        {
            SATimer -= Time.deltaTime;
            if (SATimer <= 0)
            {
                SATimer = 0;
                Shield.Activate();
                Shield.SetShieldFrac(0.25f);
            }
        }

        //On reaching 20% hp, shoot faster and deal more damage
        if (!Enrage && HC.GetCurrentHealth() < HC.GetMaxHealth()*0.2f) {
            Enrage = true;
            EC.SetAttackCooldown(0.75f);
            EBS.SetDamage(-75f);
        }
	}
}
