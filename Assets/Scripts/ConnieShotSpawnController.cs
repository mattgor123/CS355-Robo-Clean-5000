using UnityEngine;
using System.Collections;

public class ShotSpawnController : MonoBehaviour {

    Rigidbody shotSpawnBody;
    float camRayLength = 100f;          // The length of the ray from the camera into the scene.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    public GameObject player;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        //shotSpawnBody = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {
        Turning();
	
	}

    void Turning()
    {
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
    }
}
