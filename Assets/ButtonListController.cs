using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonListController : MonoBehaviour {

    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private bool[] testKeys;
    [SerializeField]
    private int testFloor;

    private InventoryController inv;
    private PlayerController player;

	// Use this for initialization
	void Start () {
	    

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == testFloor) //Can't go to current floor
            {
                buttons[i].interactable = false;
            }
            else if (!testKeys[i]) //Can't go to floor without having the key
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
