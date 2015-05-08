using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElevatorController : MonoBehaviour {

    [SerializeField]
    private StageBuilder stagebuilder;
    [SerializeField]
    private GameObject blackScreen;
    private CameraController cam;
    private Image blackImage;


	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        blackImage = blackScreen.GetComponent<Image>();
        FadeIn();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextLevel(int level)
    {
        FadeOut();
        stagebuilder.nextLevel(level);
        
    }

    public void LoadBoss(string boss)
    {
        FadeIn();
        Application.LoadLevelAdditive(boss);
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

    private IEnumerator Fade(float value) {
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
    }
    
}
