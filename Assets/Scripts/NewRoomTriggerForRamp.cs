﻿using UnityEngine;
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
                playerController.incrementCurrentFloor();
            }
            else if (upOrDown == "up")
            {
				//if (playerController.getCurrentFloor() == 0) {
				//	return;
				//}
                playerController.decrementCurrentFloor();
            }

            Application.LoadLevel(level);

        }
    }
}
