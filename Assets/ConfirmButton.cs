using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(delegate {
            //Debug.Log("GM knows word is: " + GameManager.Instance.GMChosenLetters);

            // Initiate animation sequence
            GameManager.Instance.ProgressToSemaphoreAnimation();
        });
    }
}
