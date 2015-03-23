﻿using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

    [SerializeField]
    private GameObject pauseScreen;

	void Start () {
        pauseScreen.SetActive(false);
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //currently paused
            if (Time.timeScale == 0)
            {
                //unpause and deactivate pause screen
                Time.timeScale = 1;
                pauseScreen.SetActive(false);
            }
            //not currently paused
            else
            {
                //pause and show pause screen
                Time.timeScale = 0;
                pauseScreen.SetActive(true);
                //TODO
                //will have to add code to stop all movement
                //right now, if a key is pressed as game is paused,
                //the game will continue as if that key is still pressed
            }
        }
	}

    public void LoadScene(string level)
    {
        //TODO not hard code "Main"?
        if (level.Equals("Main", System.StringComparison.OrdinalIgnoreCase))
        {
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                Destroy(o);
            }
        }
        Time.timeScale = 1;

        //TODO fix this
        //Must be a better way to do this, but as of now Main Menu settings don't persist.
        AudioListener.volume = 1;
        Application.LoadLevel(level);
    }
}