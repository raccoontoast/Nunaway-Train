using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemaphoreButtonManager : MonoBehaviour
{
    public Text ChosenLettersText;
    [SerializeField] string chosenLetters;
    public ColorBlock SelectedColorBlock;

    // Start is called before the first frame update
    void Start()
    {
        ChosenLettersText.text = "";
        GameManager.Instance.OnSemaphoreAnimationFinish += resetChosenLetters;
    }

    public void resetChosenLetters()
    {
        chosenLetters = "";
        ChosenLettersText.text = "";
    }

    public void resetChosenLetters(object sender, EventArgs e)
    {
        chosenLetters = "";
        ChosenLettersText.text = "";        
    }

    public void LogSelection(string chosenLetter, Animator buttonAnimator, SemaphoreButton semaphoreButton)
    {
        // If the button clicked hasn't already been clicked...
        if (!semaphoreButton.hasBeenChosen)
        {
            // So it doesn't do the action if clicked again
            semaphoreButton.hasBeenChosen = true;

            // Manage adding the letter to the chosen letters in the Semaphore Button Manager
            chosenLetters += (chosenLetter + " ");
            ChosenLettersText.text = chosenLetters;
            Debug.Log("Word is currently " + chosenLetters + "...");

            // Make sure the Game Manager knows what letters are chosen too
            GameManager.Instance.GMChosenLetters.Add(chosenLetter);

            // Fade Button / Change colour to show it's been pressed
            //buttonAnimator.SetTrigger("ButtonPressed");
            //semaphoreButton.GetComponent<Button>().colors = SelectedColorBlock;
            semaphoreButton.GetComponent<Button>().interactable = false;

        }
    }
}
