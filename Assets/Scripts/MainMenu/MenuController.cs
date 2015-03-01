using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    //musicplayer prefab
    public GameObject musicSource;

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
    private AudioSource audio;

    void Awake()
    {
        audio = gameObject.GetComponent<AudioSource>();
        isSound = true;
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
        playMenuScreen.SetActive(false);

        if (GameObject.FindWithTag("Music") == null)
        {
            Instantiate(musicSource);
        }
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
        audio.PlayOneShot(beep, 1.0f);
    }
}
