using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    int failCounter = 1;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        GameManager.Instance.OnSemaphoreAnimationFinish += StartDialogue;
    }

    void StartDialogue(string node)
    {
        dialogueRunner.StartDialogue(node);
    }

    void StartDialogue(object sender, OnSemaphoreAnimationFinishEventArgs e)
    {
        // Make a string of the List of chosen letters
        string letters = "";

        for (int i = 0; i < e.ChosenLetters.Count; i++)
        {
            if (i + 1 == e.ChosenLetters.Count && e.ChosenLetters[i] == "!")
            {
                letters += ""; // If the exclamation mark is at the end, read it as if without
            }
            else if (e.ChosenLetters[i] == "!")
                letters += "Exclamation";
            else
                letters += e.ChosenLetters[i];
        }

        //foreach (var letter in e.ChosenLetters)
        //{
        //    if (letter == "!")            
        //        letters += "Exclamation";            
        //    else
        //        letters += letter;
        //}

        string node = "PlayerToEddChat." + letters;

        if (dialogueRunner.NodeExists(node))        
            dialogueRunner.StartDialogue(node);
        else
        {
            dialogueRunner.StartDialogue("PlayerToEddChat.Fail" + failCounter.ToString());
            failCounter++;
            if (failCounter > 5) // as only 5 fail states
            {
                failCounter = 1;
            }
        }            
    }

    [YarnCommand("playAudio")]
    public void playAudio(string WwiseEventName)
    {
        AkSoundEngine.PostEvent(WwiseEventName, GameObject.FindGameObjectWithTag("Player"));
        Debug.Log("Playing " + WwiseEventName);
    }
}
