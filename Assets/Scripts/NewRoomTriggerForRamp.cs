using UnityEngine;
using System.Collections;

public class NewRoomTriggerForRamp : MonoBehaviour {

    //The scene name that this trigger leads to
    [SerializeField]
    private string level;

    [SerializeField]
    private string upOrDown;


    void OnTriggerEnter(Collider other)
    {
        //if this object hits Player
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (upOrDown == "down")
            {
                playerController.increaseCurrentFloor();
            }
            else if (upOrDown == "up")
            {
                playerController.decreaseCurrentFloor();
            }

            Application.LoadLevel(level);
        }
    }
}
