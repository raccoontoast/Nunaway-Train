using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    int failCounter = 1;
    public AK.Wwise.Event SignalReplyEvent;
    public AK.Wwise.Event StopRadioFXEvent;
    uint CurrentEventID;
    string CurrentEventString = "";
    GameObject player;
    int tooShortCount = 1;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        GameManager.Instance.OnSemaphoreAnimationFinish += StartDialogue;
        player = GameObject.FindGameObjectWithTag("Player");
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


            
        if (letters == "DODGE") // Win condition
            dialogueRunner.StartDialogue("PlayerToEddChat.Victory1");
        else if (dialogueRunner.NodeExists(node))
            dialogueRunner.StartDialogue(node);
        else if (letters.Length <= 2)
        {
            dialogueRunner.StartDialogue("PlayerToEddChat.TooShort" + tooShortCount.ToString());
            tooShortCount++;
            if (tooShortCount > 13)    // Number of 'too short' responses         
                tooShortCount = 1;            
        }
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

    void StopPreviousAndReassign(string s)
    {
        // Stop previous event
        if (CurrentEventString != "")
        {
            AkSoundEngine.ExecuteActionOnEvent(CurrentEventString, AkActionOnEventType.AkActionOnEventType_Stop, player, 500);
        }

        // Reassign CurrentEvent to event being posted
        CurrentEventID = AkSoundEngine.GetIDFromString(s);
        CurrentEventString = s;
    }

    [YarnCommand("playAudio")]
    public void playAudio(string WwiseEventName)
    {
        StopPreviousAndReassign(WwiseEventName);

        // Post the event
        AkSoundEngine.PostEvent(CurrentEventID, player);        
        Debug.Log("Playing " + WwiseEventName);
    }

    [YarnCommand("playSignalReply")]
    public void playSignalReply()
    {
        StopPreviousAndReassign("Play_SignalReply");

        SignalReplyEvent.Post(player, (uint)AkCallbackType.AK_EndOfEvent, SignalReplyStop);
        AkSoundEngine.PostEvent("Play_RadioFX", player);
    }

    public void SignalReplyStop(object in_cookie, AkCallbackType in_type, object in_info)
    {
        //AkSoundEngine.PostEvent("Stop_RadioFX", GameObject.FindGameObjectWithTag("Player"));
        Debug.Log("Callback function.");
        StopRadioFXEvent.Post(player);
    }

    [YarnCommand("setSwitch")]
    public void setSwitch(string WwiseSwitchGroup, string WwiseSwitchState)
    {
        AkSoundEngine.SetSwitch(WwiseSwitchGroup, WwiseSwitchState, player);        
    }
    
    public void StopDialogue()
    {
        AkSoundEngine.ExecuteActionOnEvent(CurrentEventString, AkActionOnEventType.AkActionOnEventType_Stop, player, 250);
    }
}
