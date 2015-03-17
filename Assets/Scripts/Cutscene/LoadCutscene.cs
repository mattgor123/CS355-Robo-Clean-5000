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
    private bool going;

    void Start()
    {
        currentText = 0;
        textArray = new List<string>();
        going = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        PlayerController pc = player.GetComponent<PlayerController>();

        //first time exiting first floor
        if (pc.getCurrentFloor() == pc.getDialogueLevel() && pc.getCurrentFloor() == 0)
        {
            GameObject cc = Instantiate(cutsceneCanvas);
            text = cc.GetComponentInChildren<Text>();
            //create array of strings for text
            textArray.Add("Woah");
            textArray.Add("What were those creatures?");
            textArray.Add("And where are all the researchers?");
            textArray.Add("Why is the building so run down?");
            textArray.Add("Is it possible that the creatures took over the labs?");
            textArray.Add("Or maybe an apocalypse occurred...");
            textArray.Add("I was sent to a post-apocalytic world???");
            text.text = "";
            //Time.timeScale = 0;

            pc.incrementDialogueLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            text.text = textArray[currentText];
            other.attachedRigidbody.useGravity = false;
            Time.timeScale = 0;
            going = true;


        }
    }

    void Update()
    {
        
        if (going && currentText < textArray.Count)
        {
            //Because gravity still pulls player down when timeScale = 0
            player.transform.position = playerTransform.position;

            //TODO change to mouse right click
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentText += 1;
                if (currentText == textArray.Count)
                {
                    Time.timeScale = 1;
                    player.GetComponent<Collider>().attachedRigidbody.useGravity = true;
                    text.text = "";
                    Destroy(gameObject);
                    return;
                }
                text.text = textArray[currentText];
                
            }
        }
    }
}
