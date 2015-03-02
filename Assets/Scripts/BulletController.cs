using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	private float damage;
	private float cleanup_delay;
	private float creation_time;

    private bool source_player;   //whether this is fired by the player (and hits enemies)

    //Following is from http://wiki.unity3d.com/index.php?title=DontGoThroughThings
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 

    private float minimumExtent;
    private float partialExtent;
    private float sqrMinimumExtent;
    private Vector3 previousPosition;
    private Rigidbody myRigidbody;

	private void Start () {
		creation_time = Time.time;

        //Following is from http://wiki.unity3d.com/index.php?title=DontGoThroughThings
        myRigidbody = rigidbody;
        previousPosition = myRigidbody.position;
        minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
        partialExtent = minimumExtent * (1.0f - skinWidth);
        sqrMinimumExtent = minimumExtent * minimumExtent; 
	}

	private void Update () {
		if(Time.time - creation_time > cleanup_delay) {
			Destroy(gameObject);
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

    /*
	void OnCollisionEnter(Collision collision) {
        //Player-fired bullets do not hit the player
        if (source_player && collision.gameObject.tag == "Player")        
            return;        

        //Enemy-fired bullets do not hit enemies
        if (!source_player && collision.gameObject.tag != "Player")
            return;

		var victim_health = collision.gameObject.GetComponent<HealthController>();
		if(victim_health != null) {
			victim_health.ChangeHealth(-damage);
		}
        Destroy(gameObject);
	}
     * */

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
                   victim_health.ChangeHealth(-damage);
               }
               Destroy(gameObject);
           }
	   } 
	   previousPosition = myRigidbody.position; 
	}
}
