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
    private float fadeIn;
    private GameObject blackScreen;
    private Image black;

    private int levelToLoad;

	// Use this for initialization
	void Start () {
        //gameObject.SetActive(false);
        countdown = 2;
        nextLevelCountdown = 0;
        fadeIn = 0;

        blackScreen = GameObject.Find("BlackScreenCanvas");
        black = blackScreen.GetComponentInChildren<Image>();
        blackScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (nextLevelCountdown != 0 && Time.realtimeSinceStartup > nextLevelCountdown)
        {
            //TODO
            //during this time make player invincible? so he doesn't get hurt during shaking
            GameObject stagebuilder = GameObject.FindGameObjectWithTag("StageBuilder");
            stagebuilder.GetComponent<StageBuilder>().nextLevel(levelToLoad);
            nextLevelCountdown = 0;
            fadeIn = Time.realtimeSinceStartup + countdown;
            black.CrossFadeAlpha(0.0f, 2, true);
            //blackScreen.SetActive(false);
        }
        else if (fadeIn != 0 && Time.realtimeSinceStartup > fadeIn)
        {
            blackScreen.SetActive(false);
            fadeIn = 0;
        }
	
	}

    public void makeButtons()
    {
        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        int dfv = pc.getDeepestLevelVisited();
		int currentLevel = pc.getCurrentFloor ();

        //GameObject button; // = Instantiate(eButton);
        //button.transform.SetParent(ePanel.transform);
        //button.transform.position = ePanel.transform.position;

        for (int i = 0; i <= dfv + 1; i++)
        {


            GameObject button = Instantiate(eButton);
            button.GetComponentInChildren<Text>().text = "B" + i;
            button.transform.SetParent(ePanel.transform);
			Vector3 change = new Vector3(0, -(i * buttonGap) + (maxLevels / 2) * buttonGap, 0);
			Vector3 pos = ePanel.transform.position + change;
			button.transform.position = pos;
			if (currentLevel == i) {
				button.GetComponent<Button>().interactable = false;
			}
			else {
                int icopy = i;
                button.GetComponent<Button>().onClick.AddListener(
                    () => shake(icopy)
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

        Time.timeScale = 0;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraController cc = camera.GetComponent<CameraController>();
        cc.shake();
        nextLevelCountdown = countdown + Time.realtimeSinceStartup;
        levelToLoad = level;

        //start fading out
        black.canvasRenderer.SetAlpha(0f);
        blackScreen.SetActive(true);

        //start fading to black
        black.CrossFadeAlpha(1.0f, 2, true);

    }

    private void cancel()
    {
        gameObject.SetActive(false);
    }
}
