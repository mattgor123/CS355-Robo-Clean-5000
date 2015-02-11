using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathMessage : MonoBehaviour {

    [SerializeField]
    private PlayerController player;

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (player.getHP() <= 0)
        {
            text.text = "YOU HAVE DIED";
        }
	}
}
