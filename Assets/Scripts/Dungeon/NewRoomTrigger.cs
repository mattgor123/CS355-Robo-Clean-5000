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
        elevatorPanel = GameObject.FindGameObjectWithTag("ElevatorPanel");



        //blackScreen = GameObject.Find("BlackScreen");

        elevatorCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {

        if (!trigger)
        {

            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {


                //TODO pause everything maybe?
                Time.timeScale = 0;
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                elevatorCanvas.SetActive(true);
                ElevatorController ec = elevatorCanvas.GetComponent<ElevatorController>();
                ec.makeButtons();

                trigger = true;
                Time.timeScale = 0;

            }
        }
    }

    /*
    public void cancelElevator()
    {
        elevatorCanvas.SetActive(false);
        Time.timeScale = 1;
    }
     * */

    void OnTriggerExit(Collider other)
    {       
        //remove any buttons that are on the panel
        int children = elevatorPanel.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject.Destroy(elevatorPanel.transform.GetChild(i).gameObject);
        }


        if (other.gameObject.tag == "Player")
        {
            //Cursor.lockState = CursorLockMode.Locked;
            trigger = false;
            elevatorCanvas.SetActive(false);
            //Time.timeScale = 1;

        }
    }
}
