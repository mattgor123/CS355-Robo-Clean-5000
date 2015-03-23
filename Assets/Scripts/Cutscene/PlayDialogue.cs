using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PlayDialogue : MonoBehaviour {

    [SerializeField]
    private GameObject cutsceneCanvas; //prefab

    private int currentText; //current position in the dialogueArray
    private List<string> textArray; //array of strings to be in dialogue
    private GameObject cc; //cutscene canvas
    private Text text; //text of cutscene canvas

    private Transform playerTransform;
    private GameObject player;

    //is dialogue playing
    private bool going;

    void Start()
    {
        currentText = 0;
        textArray = new List<string>();
        going = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;

        PlayerController pc = player.GetComponent<PlayerController>();

        //first time exiting floor
        if (pc.getCurrentFloor() == pc.getDialogueLevel())
        {
            cc = Instantiate(cutsceneCanvas);
            text = cc.GetComponentInChildren<Text>();
            cc.SetActive(false);
            text.text = "";

            try
            {
                string fileName = Application.dataPath + Path.DirectorySeparatorChar + "Dialogue" + Path.DirectorySeparatorChar + "dialogue" + pc.getDialogueLevel() + ".txt";
                StreamReader fileInput = new StreamReader(fileName);

                string line = fileInput.ReadLine();

                while (line != null)
                {
                    textArray.Add(line);
                    line = fileInput.ReadLine();
                }
            }
            catch (FileNotFoundException e)
            {
                Destroy(cc);
                Destroy(gameObject);
            }
            
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
            cc.SetActive(true);
            text.text = textArray[currentText];
            //Because gravity still pulls player down when timeScale = 0
            other.attachedRigidbody.useGravity = false;
            Time.timeScale = 0;
            going = true;
        }
    }

    void Update()
    {
        
        if (going && currentText < textArray.Count)
        {
            
            //player.transform.position = playerTransform.position;

            //move to next sentence is these keys are pressed
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                currentText += 1;

                //if we've reached the end of the textArray
                if (currentText == textArray.Count)
                {
                    Time.timeScale = 1;
                    player.GetComponent<Collider>().attachedRigidbody.useGravity = true;
                    Destroy(cc);
                    Destroy(gameObject);
                    return;
                }

                text.text = textArray[currentText];
                
            }
        }
    }
}
