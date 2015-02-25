using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {

    [SerializeField]
    private GameObject menuScreen;

    [SerializeField]
    private GameObject optionsScreen;

    [SerializeField]
    private GameObject playMenuScreen;

    void Start()
    {
        optionsScreen.SetActive(false);
        menuScreen.SetActive(true);
        playMenuScreen.SetActive(false);
    }

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void OpenOptions()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(true);
        playMenuScreen.SetActive(false);
    }

    public void OpenPlayMenu()
    {
        menuScreen.SetActive(false);
        optionsScreen.SetActive(false);
        playMenuScreen.SetActive(true);
    }

    public void OpenMainMenu()
    {
        menuScreen.SetActive(true);
        optionsScreen.SetActive(false);
        playMenuScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
