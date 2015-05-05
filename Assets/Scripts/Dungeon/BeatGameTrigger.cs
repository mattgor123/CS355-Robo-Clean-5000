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
        if (other.gameObject.tag == "Player" && inventory.hasKey(9))
        {
            Application.LoadLevel("Victory");
        }
    }


}
