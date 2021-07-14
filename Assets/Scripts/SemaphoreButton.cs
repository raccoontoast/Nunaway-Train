using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemaphoreButton : MonoBehaviour
{
    Button button;
    Animator animator;
    ColorBlock origColors;
    public bool hasBeenChosen = false;

    public void resetButton(object sender, EventArgs e)
    {
        //if (hasBeenChosen & sender.GetType().Name == "ClearButton")
        //{
            button.colors = origColors;
        //}

        hasBeenChosen = false;
    }

    void Start()
    {
        GameManager.Instance.OnSemaphoreAnimationFinish += resetButton; // Subscribe to the event for resetting everything UI related        

        button = GetComponent<Button>();
        origColors = button.colors;
        animator = GetComponent<Animator>();

        button.onClick.AddListener(delegate {
            FindObjectOfType<SemaphoreButtonManager>().LogSelection(GetComponentInChildren<Text>().text,
            animator,
            this.GetComponent<SemaphoreButton>());            
        });        
    }    
}
