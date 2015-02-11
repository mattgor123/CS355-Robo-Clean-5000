using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {

    public float speed;
    /*
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    public GameObject player;
     * */

	// Use this for initialization
	void Start () {

        /*
        player = GameObject.Find("Player");
        //Vector3 shootDirection;
        //Vector3 shootDirection = Input.mousePosition - player.transform.position;
        //shootDirection.z = 0.0f;
        //shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        //shootDirection = shootDirection-transform.position;
        //...instantiating the rocket
        //Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
        //bulletInstance.velocity = new Vector2(shootDirection.x * speed, shootDirection.y * speed);

        //rigidbody.velocity = shootDirection * speed;

        //rigidbody.velocity = transform.forward * speed;//

        floorMask = LayerMask.GetMask("Floor");
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - player.transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            rigidbody.MoveRotation(newRotation);
        }
         */
        rigidbody.velocity = transform.forward * speed;
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

}
