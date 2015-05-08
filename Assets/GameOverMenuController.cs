using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenuController : MonoBehaviour
{


    [SerializeField]
    private GameObject GameOverMenuScreen;

    [SerializeField]
    private GameObject ContinueMenuScreen;

    [SerializeField]
    private AudioClip beep;


    //AudioSource that plays the beeps
    private AudioSource ouraudio;

    void Awake()
    {
        ouraudio = gameObject.GetComponent<AudioSource>();
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");

        GameOverMenuScreen.SetActive(true);
        ContinueMenuScreen.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }


    //Restarts the game from beginning of Game scene
    public void RestartGame(string level)
    {
        PlayerPrefs.Save();
        Application.LoadLevel(level);
    }

    public void ContinueGame()
    {
        //TODO: Load up Player and his gear, Stage, etc
    }



    public void OpenContinueMenu()
    {
        //opens the play screen
        GameOverMenuScreen.SetActive(false);
        ContinueMenuScreen.SetActive(true);
    }

    public void OpenGameOverMenu()
    {
        //opens main menu screen
        GameOverMenuScreen.SetActive(true);
        ContinueMenuScreen.SetActive(false);
    }


    public void playBleep()
    {
        ouraudio.PlayOneShot(beep, 1.0f);
    }
}
