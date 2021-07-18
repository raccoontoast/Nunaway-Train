using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Yarn.Unity;

public class OnSemaphoreAnimationFinishEventArgs : EventArgs
{
    public List<string> ChosenLetters;
}

public class GameManager : MonoBehaviour
{
    // Singleton stuff
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        _Instance = this;
    }

    public List<string> GMChosenLetters;
    Animator playerAnimator;
    public GameObject UI;
    public GameObject RailOperatorCamera;
    public GameObject mainCamera;
    public event EventHandler<OnSemaphoreAnimationFinishEventArgs> OnSemaphoreAnimationFinish;
    public GameObject PreviousAttemptsScrollView;
    public GameObject PreviousAttemptTextPrefab;
    public GameObject EasyModeButton;
    [SerializeField] private int numberOfAttempts;
    public int EasyModeShowAttemptNo = 2;
    public int EasyModeEnableAttemptNo = 7;
    public bool EasyModeIsAvailable = false;

    public DialogueRunner DialogueRunner;
    public string DebugLettersForDialogue = "";    
    // To do: backspace button for adding semaphore signs

    void Start()
    {
        GMChosenLetters = new List<string>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        if (Camera.main != null) // grab a reference for later
        {
            mainCamera = Camera.main.gameObject;
        }

        // Clear the previous attempts scrollview
        var attempts = PreviousAttemptsScrollView.GetComponentsInChildren<Transform>();
        for (int i = 1; i < attempts.Length; i++)
        {
            Destroy(attempts[i].gameObject);
        }

        // Subscribe listeners
        OnSemaphoreAnimationFinish += addPreviousAttemptToScrollView;
        OnSemaphoreAnimationFinish += incrementNumberOfAttempts;
        OnSemaphoreAnimationFinish += decrementEasyModeButtonAttemptsRemaining;

        // Set Easymode button to inactive
        EasyModeButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ConfirmTest()
    {
        Debug.Log("Confirm Button pressed");
    }

    private void SetUIVisible(bool status)
    {
        if (status == true)        
            UI.SetActive(true);
        else        
            UI.SetActive(false);
    }

    public void ProgressToSemaphoreAnimation()
    {
        // remove UI
        SetUIVisible(false); // To do: animate this transition

        // Switch to viewer's perspective
        RailOperatorCamera.SetActive(true);        
        Camera.main.gameObject.SetActive(false); // To do: transition nicely

        // do the animation
        StartCoroutine(SemaphoreAnimationSteps(0.85f));
    }

    void incrementNumberOfAttempts(object sender, EventArgs e)
    {
        numberOfAttempts++;

        // Easy Mode Button stuff
        if (numberOfAttempts >= EasyModeShowAttemptNo)
        {
            EasyModeButton.SetActive(true);
        }
    }

    void decrementEasyModeButtonAttemptsRemaining(object sender, EventArgs e)
    {
        if (EasyModeButton.activeSelf)
        {
            if (numberOfAttempts >= EasyModeEnableAttemptNo)
            {
                EasyModeButton.transform.GetChild(1).GetComponent<Text>().text = "EASY MODE UNLOCKED";
                EasyModeIsAvailable = true;
            }
            else
                EasyModeButton.transform.GetChild(1).GetComponent<Text>().text = (EasyModeEnableAttemptNo - numberOfAttempts).ToString() + " more attempts to unlock EASY MODE";
        }
    }

    IEnumerator SemaphoreAnimationSteps(float time)
    {
        playerAnimator.SetBool("NotTransmitting", true);

        yield return new WaitForSeconds(time);

        playerAnimator.SetBool("NotTransmitting", false);

        string _item = "";
        foreach (var item in GMChosenLetters)
        {
            if (item == "!")
                playerAnimator.SetTrigger("ExclamationMark");
            else if (item == _item)
                Debug.Log("two Ds so not triggering animation"); //Could smooth over or emphasise two Ds
            else
                playerAnimator.SetTrigger(item);

            Debug.Log("Playing animation for semaphore letter '" + item + "'.");

            _item = item;
            yield return new WaitForSeconds(time);
        }

        playerAnimator.SetBool("NotTransmitting", true);

        yield return new WaitForSeconds(time);

        OnSemaphoreAnimationFinish?.Invoke(this, new OnSemaphoreAnimationFinishEventArgs { ChosenLetters = GMChosenLetters });
    }
    
    void addPreviousAttemptToScrollView(object sender, EventArgs e)
    {
        GameObject attemptGO = Instantiate(PreviousAttemptTextPrefab, PreviousAttemptsScrollView.transform);

        string attemptString = "";

        foreach (var letter in GMChosenLetters)
        {
            attemptString += (letter + " ");
        }

        attemptGO.GetComponent<Text>().text = attemptString;
    }

    public void EndRailControllerResponse()
    {
        RailOperatorCamera.SetActive(false);
        if (mainCamera != null) mainCamera.SetActive(true);

        GMChosenLetters.Clear(); // Clear the GMs version of the chosen letters (the UI is cleared in the SemaphoreButtonManager)        
        SetUIVisible(true);

        AkSoundEngine.PostEvent("Resume_InGameSound", GameObject.FindGameObjectWithTag("Player"));
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(GameManager))]
    public class CustomYarnCommandsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager YC = (GameManager)target;

            if (GUILayout.Button("Start Dialogue"))
            {
                if (Instance.DebugLettersForDialogue == "")                
                    Instance.DialogueRunner.StartDialogue("PlayerToEddChat");
                else
                    Instance.DialogueRunner.StartDialogue("PlayerToEddChat." + Instance.DebugLettersForDialogue);
            }
        }
    }

#endif
}




