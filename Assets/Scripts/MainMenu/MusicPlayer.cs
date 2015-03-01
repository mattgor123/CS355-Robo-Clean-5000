using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        DontDestroyOnLoad(gameObject);
        //Don't destroy, so this instance persists throughout entire game play
        //current music file from http://www.newgrounds.com/audio/listen/563100
	
	}

    void LateUpdate()
    {
        //Stay with the camera so it stays loud
        //Need to find tag because some scenes have different cameras
        Transform camera = GameObject.FindWithTag("MainCamera").transform;
        gameObject.transform.position = camera.position;
    }
}
