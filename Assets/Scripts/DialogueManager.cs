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
        AkSoundEngine.StopPlayingID(CurrentEventID);

        CurrentEventID = AkSoundEngine.GetIDFromString(WwiseEventName);

        AkSoundEngine.PostEvent(WwiseEventName, GameObject.FindGameObjectWithTag("Player"));
        Debug.Log("Playing " + WwiseEventName);
    }

    [YarnCommand("playSignalReply")]
    public void playSignalReply()
    {
        CurrentEventID = SignalReplyEvent.Id;

        SignalReplyEvent.Post(GameObject.FindGameObjectWithTag("Player"), (uint)AkCallbackType.AK_EndOfEvent, SignalReplyStop);
        AkSoundEngine.PostEvent("Play_RadioFX", GameObject.FindGameObjectWithTag("Player"));
    }

    public void SignalReplyStop(object in_cookie, AkCallbackType in_type, object in_info)
    {
        //AkSoundEngine.PostEvent("Stop_RadioFX", GameObject.FindGameObjectWithTag("Player"));
        Debug.Log("Callback function.");
        StopRadioFXEvent.Post(GameObject.FindGameObjectWithTag("Player"));
    }

    [YarnCommand("setSwitch")]
    public void setSwitch(string WwiseSwitchGroup, string WwiseSwitchState)
    {
        AkSoundEngine.SetSwitch(WwiseSwitchGroup, WwiseSwitchState, GameObject.FindGameObjectWithTag("Player"));        
    }
}
