using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryMenuController : MonoBehaviour
{


    [SerializeField]
    private GameObject VictoryMenuScreen;

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

        VictoryMenuScreen.SetActive(true);
        ContinueMenuScreen.SetActive(false);

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
        VictoryMenuScreen.SetActive(false);
        ContinueMenuScreen.SetActive(true);
    }

    public void OpenVictoryMenu()
    {
        //opens main menu screen
        VictoryMenuScreen.SetActive(true);
        ContinueMenuScreen.SetActive(false);
    }


    public void playBleep()
    {
        ouraudio.PlayOneShot(beep, 1.0f);
    }
}
