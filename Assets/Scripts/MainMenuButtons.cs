using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public delegate void Fade();
    public event Fade OnFadeComplete;

    private void Start()
    {
        // The fade has an event on ending, so subscribe the function here
        OnFadeComplete += LoadGame;
    }

    public void PlayGame()
    {
        // Starts fade to black
        Instantiate(Resources.Load("Prefabs/FadeOutCanvas") as GameObject);        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("New Scene");
    }

    public void FadeComplete()
    {
        OnFadeComplete();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
