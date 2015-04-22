using UnityEngine;
using System.Collections;

public class ShattersWhenKilled : MonoBehaviour
{

    #region Serialized Variables
    [SerializeField]
    private bool dead;

    #endregion

    #region private variables
    private int boneNumber;
    private HealthController health;
    private Animator anim;
    private ArrayList bones;
    private bool changed = false;
    private const string deathAnim = "PA_WarriorDeath_Clip";
    #endregion

    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            anim.SetBool("isDead", true);
        }
        if (!dead)
        {
            anim.SetBool("isDead", false);
        }
	    
	}
}
