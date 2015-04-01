using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewRoomTrigger : MonoBehaviour {

    //The scene name that this trigger leads to
    //[SerializeField]
    //string level = "RampDown";

    //private GameObject player;

    private GameObject elevatorCanvas;

    private int countdown;
    private float nextLevelCountdown;

    private bool trigger;

    //private GameObject elevatorCanvas;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        countdown = 2;
        nextLevelCountdown = 0;
        trigger = false;
        elevatorCanvas = GameObject.FindGameObjectWithTag("ElevatorCanvas");
        elevatorCanvas.SetActive(false);
        ElevatorController ec = elevatorCanvas.GetComponent<ElevatorController>();
        ec.makeButtons();
    }

    void Update()
    {
        if (nextLevelCountdown == 0)
        {
            return;
        }
        else if (nextLevelCountdown > 0)
        {
            nextLevelCountdown -= Time.deltaTime;
        }
        else
        {
            //nextLevelCountdown = 0;
            GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
            stagebuilder.GetComponent<StageBuilder>().nextLevel();
        }

    }

    /*
    public void setLevel(string s)
    {
        level = s;
    }
    */
     
    void OnTriggerEnter(Collider other)
    {
        if (!trigger)
        {

            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {
                //PlayerController pc = other.gameObject.GetComponent<PlayerController>();
                //int dfv = pc.getDeepestLevelVisited();

                //GameObject ep = GameObject.FindGameObjectWithTag("ElevatorPanel");


                //TODO pause everything

                Debug.Log("SET PANEL ");
                elevatorCanvas.SetActive(true);

                //GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                //CameraController cc = camera.GetComponent<CameraController>();
                //cc.shake();
                
                //nextLevelCountdown = countdown;

                trigger = true;

                //GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
                //stagebuilder.GetComponent<StageBuilder>().nextLevel();
            }
        }
    }

    public void nextLevel()
    {
        GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
        stagebuilder.GetComponent<StageBuilder>().nextLevel();
    }

    public void cancelElevator()
    {
        //trigger = false;
        elevatorCanvas.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            trigger = false;
            elevatorCanvas.SetActive(false);
        }
    }
}
