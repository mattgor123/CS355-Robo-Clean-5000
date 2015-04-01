using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour {

    [SerializeField]
    private GameObject eButton;

    [SerializeField]
    private GameObject ePanel;

    [SerializeField]
    private int maxLevels;

    [SerializeField]
    private int buttonGap;

    private int countdown;
    private float nextLevelCountdown;

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
        countdown = 2;
        nextLevelCountdown = 0;
	}
	
	// Update is called once per frame
	void Update () {
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
            nextLevelCountdown = 0;
        }
	
	}

    public void makeButtons()
    {
        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        int dfv = pc.getDeepestLevelVisited();


        //GameObject button; // = Instantiate(eButton);
        //button.transform.SetParent(ePanel.transform);
        //button.transform.position = ePanel.transform.position;

        for (int i = 0; i <= dfv + 1; i++)
        {
            if (pc.getCurrentFloor() != i)
            {

                GameObject button = Instantiate(eButton);
                button.GetComponentInChildren<Text>().text = "B" + i;
                button.transform.SetParent(ePanel.transform);

                Vector3 change = new Vector3(0, -(i * buttonGap) + (maxLevels / 2) * buttonGap, 0);

                Vector3 pos = ePanel.transform.position + change;

                button.transform.position = pos;

                button.GetComponent<Button>().onClick.AddListener(
                    () => shake(i)
                    );
            }
        }

        GameObject cancelButton = Instantiate(eButton);
        cancelButton.GetComponentInChildren<Text>().text = "Cancel";
        cancelButton.transform.SetParent(ePanel.transform);

        Vector3 cChange = new Vector3(0, -(maxLevels / 2) * buttonGap, 0);

        Vector3 cPos = ePanel.transform.position + cChange;

        cancelButton.transform.position = cPos;
        cancelButton.GetComponent<Button>().onClick.AddListener(
                    () => cancel()
                    );
    }

    private void shake(int level)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraController cc = camera.GetComponent<CameraController>();
        cc.shake();
        nextLevelCountdown = countdown;

        //foreach (Transform child in ePanel) {
        //    Destroy(child.gameObject);
        //}

        int children = ePanel.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject.Destroy(ePanel.transform.GetChild(i).gameObject);
        }
    }

    private void cancel()
    {

    }
}
