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
    private Canvas blackCanvas;


    // Use this for initialization
    void Start()
    {
        var blackCanvas = GameObject.Find("BlackScreenCanvas");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        blackImage = blackScreen.GetComponent<Image>();
        //currentFloor.text = "B0";
        FadeIn();
        //blackImage.CrossFadeAlpha(0, 2f, true);
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
        //FadeOut();
        //blackImage.CrossFadeAlpha(1, 2f, true);
        //stagebuilder.nextLevel(level);
        //currentFloor.text = "B" + level;
        StartCoroutine(nextLevelHelper(level));

    }

    private IEnumerator nextLevelHelper(int level)
    {
        FadeOut();
        Time.timeScale = 0.000001f; //hack to make WaitForSeconds run while have practically 0 timeScale
        yield return new WaitForSeconds(1 * Time.timeScale);
        Time.timeScale = 0;
        stagebuilder.nextLevel(level);
        currentFloor.text = "B" + level;
        FadeIn();
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
        //StartCoroutine(Fade(1f));
        blackImage.CrossFadeAlpha(1, 1f, true);
    }

    //Fade from Black to invisible
    public void FadeIn()
    {
        //StartCoroutine(Fade(0f));
        blackImage.CrossFadeAlpha(0, 1f, true);

    }

    /*
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
*/

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        //coming from Tutorial
        //FadeOut();
        //blackImage.CrossFadeAlpha(255f, 2f, true);
        Color c = blackImage.color;
        c.a = 1;
        blackImage.color = c;
        Application.LoadLevel("Game");
    }

}
