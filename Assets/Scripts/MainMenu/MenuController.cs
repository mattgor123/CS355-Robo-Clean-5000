using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {


    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject optionsScreen;

    [SerializeField]
    private GameObject playMenuScreen;

    [SerializeField]
    private AudioClip beep;

    private bool isSound;

    //AudioSource that plays the beeps
    private AudioSource ouraudio;

    void Awake()
    {
        ouraudio = gameObject.GetComponent<AudioSource>();
        isSound = true;
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
        playMenuScreen.SetActive(false);

    }

    public void LoadScene(string level)
    {
        Application.LoadLevel(level);
    }

    public void OpenOptions()
    {
        //opens the options screen
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        playMenuScreen.SetActive(false);
    }

    public void OpenPlayMenu()
    {
        //opens the play screen
        menuScreen.SetActive(false);
        optionsScreen.SetActive(false);
        playMenuScreen.SetActive(true);
    }

    public void OpenMainMenu()
    {
        //opens main menu screen
        menuScreen.SetActive(true);
        optionsScreen.SetActive(false);
        playMenuScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void playBleep()
    {
        ouraudio.PlayOneShot(beep, 1.0f);
    }
}
