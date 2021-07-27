using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CutsceneManager : MonoBehaviour
{
    Animator animator;
    Camera cutsceneCamera;

    void Start()
    {
        animator = GetComponent<Animator>();
        cutsceneCamera = GetComponent<Camera>();
    }

    public void AdvanceCutscene()
    {
        animator.SetTrigger("AdvanceCutscene");
    }

    [YarnCommand("endCutscene")]
    public void EndCutscene()
    {
        var gm = GameManager.Instance;

        gm.DialogueRunner.Stop();
        gm.mainCamera.SetActive(true);
        gm.UI.SetActive(true);

        cutsceneCamera.gameObject.SetActive(false);
        //GameObject.Find("BlackBarsUI").SetActive(false);
        FindObjectOfType<CinematicBlackBars>().Disappear();
        AkSoundEngine.PostEvent("Play_InGameMusic", GameObject.FindGameObjectWithTag("Player"));
    }
}
