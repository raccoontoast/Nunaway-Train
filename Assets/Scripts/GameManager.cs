using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject mainCamera;
    public event EventHandler OnSemaphoreAnimationFinish;

    void Start()
    {
        GMChosenLetters = new List<string>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        mainCamera = Camera.main.gameObject; // grab a reference for later

        // Events
        OnSemaphoreAnimationFinish += BeginRailControllerResponse;
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

        OnSemaphoreAnimationFinish?.Invoke(this, EventArgs.Empty);
    }    

    void BeginRailControllerResponse(object sender, EventArgs e)
    {
        Debug.Log("Here the yarn stuff will happen with extras.");
        // Look at GMChosenLetters and choose the corresponding yarn script

        RailOperatorCamera.SetActive(false);
        mainCamera.SetActive(true);

        GMChosenLetters.Clear(); // Clear the GMs version of the chosen letters (the UI is cleared in the SemaphoreButtonManager)        
        SetUIVisible(true);
    }
}




