using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
        DontDestroyOnLoad(gameObject);
        //current music file from http://www.newgrounds.com/audio/listen/563100
	
	}

    void LateUpdate()
    {
        Transform camera = GameObject.FindWithTag("MainCamera").transform;
        gameObject.transform.position = camera.position;
    }
}
