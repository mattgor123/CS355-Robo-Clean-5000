﻿using UnityEngine;
using System.Collections;

public class NewRoomTrigger : MonoBehaviour {

    //The scene name that this trigger leads to
    [SerializeField]
    string level;

    private GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        //if this object hits Player
        if (other.gameObject.tag == "Player")
        {
            //move player to (0,0,0) because that's where all scenes start
            //moved code to beginning of each scene
            //other.gameObject.transform.position = Vector3.zero;
            //load next scene
            Application.LoadLevel(level);
        }
    }
}