using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ClearButton : MonoBehaviour
{
    public Text ChosenLettersText;
    public SemaphoreButtonManager SemaphoreButtonManager;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    public void ClearChosenLetters()
    {
        // Reset chosen Letters
        GameManager.Instance.GMChosenLetters.Clear();
        SemaphoreButtonManager.resetChosenLetters();

        // Reset Semaphore Buttons
        SemaphoreButton[] semaphoreButtons = FindObjectsOfType<SemaphoreButton>();
        foreach (var semaphoreButton in semaphoreButtons)
        {
            semaphoreButton.resetButton(this, EventArgs.Empty);
            
            // Reset colour too                        
            semaphoreButton.GetComponent<Animator>().SetTrigger("ClearButtonPressed");
        }
    }
}
