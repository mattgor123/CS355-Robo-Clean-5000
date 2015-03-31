using UnityEngine;
using System.Collections;

public class NewRoomTrigger : MonoBehaviour {

    //The scene name that this trigger leads to
    [SerializeField]
    string level = "RampDown";

    //private GameObject player;

    private int countdown;
    private float nextLevelCountdown;

    private bool shake;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        countdown = 2;
        nextLevelCountdown = 0;
        shake = false;
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

    public void setLevel(string s)
    {
        level = s;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!shake)
        {

            //if this object hits Player
            if (other.gameObject.tag == "Player")
            {
                //load next scene
                //Application.LoadLevel(level);

                GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
                CameraController cc = camera.GetComponent<CameraController>();
                cc.shake();

                nextLevelCountdown = countdown;

                shake = true;

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
}
