// Smooth Follow from Standard Assets
// Converted to C# because I fucking hate UnityScript and it's inexistant C# interoperability
// If you have C# code and you want to edit SmoothFollow's vars ingame, use this instead.
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{

    // The target we are following
    private Transform target;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]


    void LateUpdate()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Early out if we don't have a target
        if (!target) return;

        // Calculate the current rotation angles
        this.transform.rotation = Quaternion.Euler(90, 0, 0);


        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

    }
}