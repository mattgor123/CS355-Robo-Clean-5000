using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonListController : MonoBehaviour {

    [SerializeField]
    private Button[] buttons;


    private InventoryController inv;
    private PlayerController player;

	// Use this for initialization
	void Start () {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

	}
	
	// Update is called once per frame
	void Update () {
        int currentLevel = player.getCurrentFloor();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == currentLevel) //Can't go to current floor
            {
                buttons[i].interactable = false;
            }
            else if (!inv.hasKey(i)) //Can't go to floor without having the key
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true; //Else the button is pressable
            }
        }
	
	}
}
