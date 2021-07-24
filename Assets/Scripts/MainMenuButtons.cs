using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler
{
    public delegate void Fade();
    public event Fade OnFadeComplete;

    GameManager gm;

    private void Start()
    {
        // The fade has an event on ending, so subscribe the function here
        OnFadeComplete += LoadGame;

        AkSoundEngine.PostEvent("Play_MainMenu", gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            // Play the PEEP PEEP
            AkSoundEngine.PostEvent("Play_UIButtonHover", gameObject);
    }

    public void PlayGame()
    {
        // Starts fade to black
        Instantiate(Resources.Load("Prefabs/FadeOutCanvas") as GameObject);

        // Audio
        AkSoundEngine.PostEvent("Play_UIButtonPress", gameObject);

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            AkSoundEngine.ExecuteActionOnEvent("Play_MainMenu", AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 750);
        }        
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
