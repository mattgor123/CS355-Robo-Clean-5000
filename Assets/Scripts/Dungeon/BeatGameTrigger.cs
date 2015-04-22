using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeatGameTrigger : MonoBehaviour {

    private InventoryController inventory;
    private float fadeIn;
    private GameObject blackScreen;
    private Image black;

	// Use this for initialization
	void Start () {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        blackScreen = GameObject.Find("BlackScreenCanvas");
        black = blackScreen.GetComponentInChildren<Image>();
        blackScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        //remove any buttons that are on the panel
        if (other.gameObject.tag == "Player" && inventory.hasThorax())
        {

        }
    }

    private void shake(int level)
    {

        Time.timeScale = 0;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraController cc = camera.GetComponent<CameraController>();
        cc.shake();

        //start fading out
        black.canvasRenderer.SetAlpha(0f);
        blackScreen.SetActive(true);

        //start fading to black
        black.CrossFadeAlpha(1.0f, 2, true);

    }
}
