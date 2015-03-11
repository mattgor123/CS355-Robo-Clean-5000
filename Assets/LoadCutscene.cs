using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LoadCutscene : MonoBehaviour {

    [SerializeField]
    private GameObject cutsceneCanvas;

    private int currentText;
    private List<string> textArray;
    private Text text;

    private Transform playerTransform;
    private GameObject player;

    void Start()
    {
        currentText = 0;
        textArray = new List<string>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        PlayerController pc = player.GetComponent<PlayerController>();

        //first time exiting first floor
        if (pc.getCurrentFloor() == pc.getDeepestFloorVisited() && pc.getCurrentFloor() == 1)
        {
            GameObject cc = Instantiate(cutsceneCanvas);
            text = cc.GetComponentInChildren<Text>();
            //create array of strings for text
            textArray.Add("Testing");
            text.text = textArray[currentText];
            Time.timeScale = 0;
        }
    }

    void Update()
    {
        
        if (currentText < textArray.Count)
        {
            //Because gravity still pulls player down when timeScale = 0
            player.transform.position = playerTransform.position;

            //TODO change to mouse right click
            if (Input.GetKeyDown(KeyCode.G))
            {
                currentText += 1;
                if (currentText == textArray.Count)
                {
                    Time.timeScale = 1;
                    text.text = "";
                    return;
                }
                text.text = textArray[currentText];
                
            }
        }
    }
}
