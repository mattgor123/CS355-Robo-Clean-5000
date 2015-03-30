using UnityEngine;
using System.Collections;

public class NewRoomTrigger : MonoBehaviour {

    //The scene name that this trigger leads to
    [SerializeField]
    string level = "RampDown";

    private GameObject player;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    public void setLevel(string s)
    {
        level = s;
    }

    void OnTriggerEnter(Collider other)
    {
        //if this object hits Player
        if (other.gameObject.tag == "Player")
        {
            //load next scene
            //Application.LoadLevel(level);
            GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
            stagebuilder.GetComponent<StageBuilder>().nextLevel();
        }
    }
}
