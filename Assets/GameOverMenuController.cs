using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverMenuController : MonoBehaviour
{


    [SerializeField]
    private GameObject GameOverMenuScreen;


    [SerializeField]
    private AudioClip beep;


    //AudioSource that plays the beeps
    private AudioSource ouraudio;

    void Awake()
    {
        ouraudio = gameObject.GetComponent<AudioSource>();
        AudioListener.volume = PlayerPrefs.GetFloat("Volume");

        GameOverMenuScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }


    //Restarts the game from beginning of Game scene
    public void RestartGame(string level)
    {
        PlayerPrefs.Save();
        Application.LoadLevel(level);
    }



    public void OpenGameOverMenu()
    {
        //opens main menu screen
        GameOverMenuScreen.SetActive(true);
    }


    public void playBleep()
    {
        ouraudio.PlayOneShot(beep, 1.0f);
    }
}
