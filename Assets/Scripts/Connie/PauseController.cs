using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {

    [SerializeField]
    private GameObject pauseScreen;

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
        //gameObject.renderer.enabled = false;
        //DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(pauseScreen);
        pauseScreen.SetActive(false);
        //pauseScreen.transform.position = transform.position + new Vector3(0, 0, -50); //to make it lower than other canvas
        //transform.position = transform.position + new Vector3(0, 0, -50);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                //gameObject.SetActive(false);
                //gameObject.renderer.enabled = false;
                pauseScreen.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                //gameObject.SetActive(true);
                //gameObject.renderer.enabled = true;
                pauseScreen.SetActive(true);
            }
        }
	}

    public void LoadScene(string level)
    {
        //TODO not hard code "Main"?
        if (level.Equals("Main", System.StringComparison.OrdinalIgnoreCase))
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
            Destroy(GameObject.FindGameObjectWithTag("WeaponCanvas"));
        }
        Time.timeScale = 1;
        //pauseScreen.SetActive(false);
        
        Application.LoadLevel(level);
        //Time.timeScale = 1;
    }
}
