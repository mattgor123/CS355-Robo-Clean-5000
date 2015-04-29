using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	private float damage;
	private float cleanup_delay;
	private float creation_time;
	private bool is_laser;

    private bool source_player;   //whether this is fired by the player (and hits enemies)

    //Following is from http://wiki.unity3d.com/index.php?title=DontGoThroughThings
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;

	private GameObject player;
	private StatisticsRecorderController stats;
  private PlayerController PC;

	private void Start () {
		creation_time = Time.time;
        layerMask = 1 << LayerMask.NameToLayer("Wall");
        //Following is from http://wiki.unity3d.com/index.php?title=DontGoThroughThings
        myRigidbody = GetComponent<Rigidbody>();
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(GetComponent<Collider>().bounds.extents.x, GetComponent<Collider>().bounds.extents.y), GetComponent<Collider>().bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent; 
        player = GameObject.FindGameObjectWithTag("Player");
        PC = player.GetComponent<PlayerController>();
	}

	private void Update () {
		if(Time.time - creation_time > cleanup_delay) {
			//Destroy(gameObject);
            gameObject.SetActive(false);
		}
	}

    public void SetSource(bool is_player)
    {
        source_player = is_player;
    }

	public void SetDamage (float new_damage) {
		damage = new_damage;
	}

	public void SetCleanupDelay (float delay) {
		cleanup_delay = delay;
	}
    
	public void SetLaser (bool laser) {
		is_laser = laser;
	}

	void OnCollisionEnter(Collision collision) {
        //Player-fired bullets do not hit the player
        if (source_player && collision.gameObject.tag == "Player")        
            return;        

        //Enemy-fired bullets do not hit enemies
        if (!source_player && collision.gameObject.tag != "Player")
            return;

		var victim_health = collision.gameObject.GetComponent<HealthController>();
		if(victim_health != null) {
			//this means you're hitting an enemy, not a wall ... if it's a laser then keep going
      if (gameObject.tag == "Player") {
          victim_health.ChangeHealth(PC.GetDefense() * -damage, transform.position);  
      } else {
          victim_health.ChangeHealth(-damage, transform.position);
      }
			//don't let it get destroyed if it collides with an enemy and is a laser, only a wall

			//start code for stat tracking
			if (player == null) {
				player = GameObject.FindGameObjectWithTag("Player");
			}
			if (player != null && stats == null) {
				stats = player.GetComponent<StatisticsRecorderController>();
			}
			if (stats != null) {
				if (source_player) {
					stats.dealDamage(damage);
					stats.hitShot();
					//for hit canvas
					player.GetComponent<HitCanvasController>().scoreHit();
					//end hit canvas
				} else {
					stats.takeDamage(damage);
				}
			}
			//end code for stat tracking

			if (is_laser) {
				return;
			}
		}
        //Destroy(gameObject);
        gameObject.SetActive(false);
	}    

    void FixedUpdate()
    {
        //logic from http://wiki.unity3d.com/index.php?title=DontGoThroughThings

        //have we moved more than our minimum extent? 
	   Vector3 movementThisStep = myRigidbody.position - previousPosition; 
	   float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
	   if (movementSqrMagnitude > sqrMinimumExtent)
       {
           float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
           RaycastHit hitInfo; 
 
           //check for obstructions we might have missed 
           if (Physics.Raycast(myRigidbody.position, movementThisStep, out hitInfo, movementMagnitude))
           {

               myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent;

               GameObject other = hitInfo.collider.gameObject;

               //Player-fired bullets do not hit the player
               if (source_player && other.tag == "Player")
                   return;

               //Enemy-fired bullets do not hit enemies
               //if (!source_player && other.tag != "Player")
               if (!source_player && other.tag == "Enemy")
               {
                   return;
               }
               
               var victim_health = other.GetComponent<HealthController>();
               if (victim_health != null)
               {
                   if (other.tag == "Player") {
                       victim_health.ChangeHealth(PC.GetDefense() * -damage);
                   } else {
                       victim_health.ChangeHealth(-damage); 
                   } 
  
				   if (is_laser) {
						return;
				   }

               }
               if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
                   myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; 
               //Destroy(gameObject);
               gameObject.SetActive(false);
           }
	   } 
	   previousPosition = myRigidbody.position; 
	}
}
