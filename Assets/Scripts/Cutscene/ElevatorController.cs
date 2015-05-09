using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour
{

    [SerializeField]
    private StageBuilder stagebuilder;
    [SerializeField]
    private GameObject blackScreen;
    private CameraController cam;
    private Image blackImage;
    [SerializeField]
    private Text currentFloor;


    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        blackImage = blackScreen.GetComponent<Image>();
        //currentFloor.text = "B0";
        FadeIn();

    }

    // Update is called once per frame
    /*
    void Update()
    {
        currentFloor.text = "B" + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getCurrentFloor().ToString();
    }
     * */

    public void NextLevel(int level)
    {
        FadeOut();
        stagebuilder.nextLevel(level);
        currentFloor.text = "B" + level;

    }

    public void LoadBoss(string boss)
    {
        FadeIn();
        stagebuilder.destroycurrentlevel();
        stagebuilder.emptyEnemies();
        Application.LoadLevelAdditive(boss);
        int floor = 3;
        switch (boss)
        {
            case "TurretBoss":
                floor = 3;
                break;
            case "BomberBoss":
                floor = 6;
                break;
            case "Broodmother":
                floor = 9;
                break;

        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().setCurrentFloor(floor);
    }

    //Fade from invisible to Black
    public void FadeOut()
    {
        StartCoroutine(Fade(255f));
    }

    //Fade from Black to invisible
    public void FadeIn()
    {
        StartCoroutine(Fade(0f));


    }

    private IEnumerator Fade(float value)
    {
        bool done = false;
        while (!done)
        {
            blackImage.CrossFadeAlpha(value, 2f, true);
            done = true;
        }
        yield return null;


    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

}
