using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    int failCounter = 1;
    public AK.Wwise.Event SignalReplyEvent;
    public AK.Wwise.Event StopRadioFXEvent;
    public AK.Wwise.Event TestEvent;
    uint CurrentEventID;
    string CurrentEventString = "";
    GameObject player;
    int tooShortCount = 1;
    bool failMusicPlaying = false;
    [HideInInspector] public bool winStateAchieved = false;

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



        if (letters == "DODGE") { // Win condition
            dialogueRunner.StartDialogue("PlayerToEddChat.Victory1");
            GameManager.Instance.RailOperatorCamera.SetActive(false);
            GameManager.Instance.mainCamera.SetActive(false);
            Instantiate(Resources.Load("Prefabs/ChurchScene_Victory"));
            FindObjectOfType<CinematicBlackBars>().Appear();
            winStateAchieved = true;
        }
            
        else if (dialogueRunner.NodeExists(node))
        {
            dialogueRunner.StartDialogue(node);           
        }
            
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

        if (WwiseEventName == "Play_NarratorPrebakedEndings")
        {
            GameManager.Instance.InstantiateChurchScene();
        }
    }

    [YarnCommand("playEDO")] // To do: could probably merge this with the playAudio function
    public void playAudioWithCallback()
    {
        StopPreviousAndReassign("Play_FailstateUniquePrebaked13");

        object obj = new object();
        AkSoundEngine.PostEvent("Play_FailMusic", player);
        failMusicPlaying = true;

        AkSoundEngine.PostEvent("Play_FailstateUniquePrebaked13", player, (uint)AkCallbackType.AK_Marker, callbackFunction, obj);
        GameManager.Instance.InstantiateChurchScene();
    }

    [YarnCommand("playGOD")] // To do: could probably merge this with the playAudio function
    public void playGod()
    {
        StopPreviousAndReassign("Play_GOD");
        AkSoundEngine.PostEvent("Play_GOD", player);

        GameManager.Instance.InstantiateChurchScene();
    }

    void callbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        failMusicPlaying = false;
        Debug.Log("Marker reached");
        AkSoundEngine.ExecuteActionOnEvent("Play_FailMusic", AkActionOnEventType.AkActionOnEventType_Stop, player, 500, AkCurveInterpolation.AkCurveInterpolation_Linear);
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

    [YarnCommand("startCredits")]
    public void StartCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    [YarnCommand("instantiateChurchScene")]
    public void InstantiateChurchScene()
    {
        GameManager.Instance.InstantiateChurchScene();
    }

    [YarnCommand("cleanupChurchScene")]
    public void CleanupChurchScene()
    {
        if (GameObject.FindGameObjectWithTag("Nun") != null)
        {
            GameManager.Instance.CleanupChurchScene();
        }        
    }

    public void StopDialogue()
    {
        AkSoundEngine.ExecuteActionOnEvent(CurrentEventString, AkActionOnEventType.AkActionOnEventType_Stop, player, 250);

        // Messy but works for now
        if (failMusicPlaying)
        {
            AkSoundEngine.ExecuteActionOnEvent("Play_FailMusic", AkActionOnEventType.AkActionOnEventType_Stop, player, 250);
        }
    }
}
