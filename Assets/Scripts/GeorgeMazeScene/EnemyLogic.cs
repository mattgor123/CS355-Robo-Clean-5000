/*  Enemy Logic File
 *      -Contains all logic governing enemy NPC movement and actions.   
 *      -In general, EnemyController enemies have a "Type" parameter. 
 *          -This is a string that holds all of an enemy's logic flags.
 *          -flags are of the form "<type>-<subtype>"
 *           where <type> denotes the type of action (such as movement)
 *           and <subtype> denotes the specific form of that action (such as basic chase)
 *      -To use, call a high-level command with relevant input (such as Move() )
 *          -specific low-level commands will be used based on the input "Type"
 * 
 * 
 *  ==Movement==
 *  Move(EnemyController enemy, PlayerController player, string type)
 *      enemy: generally "this", the object calling the method
 *      player: the player 
 *      type: the type; see below for flags
 *      "m- " format
 *  
 *  "m-c"     Basic Chase:  Moves directly towards the target and attempts to go as close as possible
 *  "m-d"     Detour Chase: If direct path is unsuccessful, attempts to take a detour. Otherwise acts as Basic Chase
 *  "m-r"     Basic Maintain Range: Moves such that a specific distance is maintained between target and self.   
 *  "m-f"     Basic Flee: Runs away from the target. 
 *  
 *  
 *  
 *  
 *  
 *  
 */




using UnityEngine;
using System.Collections;

public class EnemyLogic {

    private static Vector3 ZERO;    //Zero vector
    private static Vector3 VERT;    //Unit vertical vector
    private static float CLOSE;     //Closeness cutoff (beyond which player is not "close")
    private static int init;

    static void Initialize()
    {
        ZERO = new Vector3(0f, 0f, 0f);
        VERT = new Vector3(0f, 1f, 0f);
        CLOSE = 1;
        init = 1;
    }

    #region Movement
    // Movement
    // returns normalized direction of movement
    public static Vector3 Move(EnemyController enemy, PlayerController player, string type)
    {
        if (type.Contains("m-c"))
            return BasicChase(enemy, player);
        else if (type.Contains("m-d"))
            return DetourChase(enemy, player);
        else if (type.Contains("m-r"))
            return BasicRange(enemy, player);
        else if (type.Contains("m-f"))
            return BasicFlee(enemy, player);
        else
            return ZERO;
    }

    // Low-level Chase functionality
    private static Vector3 Chase(EnemyController enemy, PlayerController player)
    {
        // Initialize constants if not initialized
        if (init == 0)
        {
            Initialize();
        }

        float x = player.transform.position.x - enemy.transform.position.x;
        float z = player.transform.position.z - enemy.transform.position.z;

        Vector3 mvt = new Vector3(x, 0.0f, z);
        mvt.Normalize();
        return mvt;
    }

    // Basic Chase
    public static Vector3 BasicChase(EnemyController enemy, PlayerController player)
    {
        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        if (dist <= enemy.GetAggroRadius())
        {
            return Chase(enemy, player);
        }
        return ZERO;
    }

    // Detour Chase
    public static Vector3 DetourChase(EnemyController enemy, PlayerController player)
    {
        Vector3 mvt = ZERO;
        float pm = enemy.GetPrevMvt().magnitude;

        // If hitting a wall and moving slowly, take a detour sideways
        // (prevents behavior if out in the open or if it happens to graze a wall while moving rapidly)
        if (enemy.GetWallHit() && pm <= .05)
        {
            Vector3 direct = Chase(enemy, player);              //first get direct approach
            mvt = Vector3.Cross(direct, VERT);                  //cross with vertical to get left perpendicular

            if (Time.time % 4 == 0)
            {
                mvt *= -1;                                      //flip direction every 5 seconds
            }


            mvt -= direct * (Random.value - 0.5f);              //take random angle from straight perpendicular
            mvt.Normalize();                                 //re-normalize

        }
        else // otherwise chase as normal
        {
            mvt = Chase(enemy, player);
        }

        return mvt;
    }

    // Basic Maintain Range
    public static Vector3 BasicRange(EnemyController enemy, PlayerController player)
    {
        float dist = Vector3.Distance(player.transform.position, enemy.transform.position);
        float optrng = enemy.GetOptimalRange();
        float delta = dist - optrng;

        // If outside of optimal range band (optimal += 25%), move towards optimal range
        if (Mathf.Abs(delta) / optrng >= 0.20)
        {
            Vector3 mvt = Chase(enemy, player);
            mvt *= delta / (1.5f*optrng);
            return mvt;
        }
        return ZERO;
    }
    
    // Basic Flee
    private static Vector3 BasicFlee(EnemyController enemy, PlayerController player)
    {
        return -1 * BasicChase(enemy, player);
    }
    
    
    #endregion

}
