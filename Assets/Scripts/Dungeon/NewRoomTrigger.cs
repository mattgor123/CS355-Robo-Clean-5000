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
        trigger = false;
        elevatorCanvas = GameObject.FindGameObjectWithTag("ElevatorCanvas");
        elevatorCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {

        if (!trigger)
        {
            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {
                elevatorCanvas.SetActive(true);
                trigger = true;
                Time.timeScale = 0;

            }
            
        }
    }

   



    void OnTriggerExit(Collider other)
    {       
        //remove any buttons that are on the panel
        if (other.gameObject.tag == "Player")
        {
            //Cursor.lockState = CursorLockMode.Locked;
            trigger = false;
            elevatorCanvas.SetActive(false);
        }
    }
}
