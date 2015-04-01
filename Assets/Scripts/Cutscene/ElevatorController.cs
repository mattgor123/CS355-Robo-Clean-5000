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

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
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
            GameObject button = Instantiate(eButton);
            button.GetComponentInChildren<Text>().text = "B"+i;
            button.transform.SetParent(ePanel.transform);

            Vector3 change = new Vector3(0, i * 45 + (maxLevels/2) * 45, 0);

            Vector3 pos = ePanel.transform.position + change;

            button.transform.position = pos;
        }

    }

    private void shake()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraController cc = camera.GetComponent<CameraController>();
        cc.shake();
    }
}
