using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SemaphoreButton : MonoBehaviour
{
    Button button;
    Animator animator;
    public bool hasBeenChosen = false;

    public void resetButton(object sender, EventArgs e)
    {
        hasBeenChosen = false;
        button.interactable = true;
    }

    void Start()
    {
        GameManager.Instance.OnSemaphoreAnimationFinish += resetButton; // Subscribe to the event for resetting everything UI related        

        button = GetComponent<Button>();
        animator = GetComponent<Animator>();

        button.onClick.AddListener(delegate
        {
            FindObjectOfType<SemaphoreButtonManager>().LogSelection(GetComponentInChildren<Text>().text,
            animator,
            this.GetComponent<SemaphoreButton>());
        });
    }
}
