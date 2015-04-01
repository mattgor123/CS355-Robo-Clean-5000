using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewRoomTrigger : MonoBehaviour {

    //The scene name that this trigger leads to
    //[SerializeField]
    //string level = "RampDown";

    //private GameObject player;

    private GameObject elevatorCanvas;
    private GameObject elevatorPanel;
    private GameObject blackScreen;

    private int countdown;
    private float nextLevelCountdown;

    private bool trigger;

    //private GameObject elevatorCanvas;

    void Start()
    {
        Time.timeScale = 1;

        countdown = 2;
        nextLevelCountdown = 0;
        trigger = false;
        elevatorCanvas = GameObject.FindGameObjectWithTag("ElevatorCanvas");
        ElevatorController ec = elevatorCanvas.GetComponent<ElevatorController>();

        //remove any buttons that are on the panel
        elevatorPanel = GameObject.FindGameObjectWithTag("ElevatorPanel");
        int children = elevatorPanel.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject.Destroy(elevatorPanel.transform.GetChild(i).gameObject);
        }

        ec.makeButtons();

        //blackScreen = GameObject.Find("BlackScreen");

        elevatorCanvas.SetActive(false);
    }

    /*
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
     * */

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


                //TODO pause everything maybe?

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

    /*
    public void nextLevel()
    {
        GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
        stagebuilder.GetComponent<StageBuilder>().nextLevel();
    }
     * */

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
