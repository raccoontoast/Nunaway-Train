using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasyModeButton : MonoBehaviour
{
    bool status = false;
    public Text PreviousAttemptsTextPrefab;
    public GameObject PreviousAttemptsTextContainer;
    public Text ChosenLettersText;
    public Font SemaphoreFont;
    public Font NormalFont;

    // Start is called before the first frame update
    void Start()
    {
        //Reset font for prefab
        PreviousAttemptsTextPrefab.font = SemaphoreFont;
    }

    public void ToggleEasyMode()
    {
        if (GameManager.Instance.EasyModeIsAvailable)
        {
            status = !status;

            if (status == true)
            {
                Color color = Color.red;

                //previous attempts font
                // entry box font
                // Button text
                GetComponentInChildren<Text>().text = "Easy Mode ON";

                Text[] textArray = PreviousAttemptsTextContainer.GetComponentsInChildren<Text>();
                foreach (var text in textArray)
                {
                    text.font = NormalFont;
                }

                ChosenLettersText.font = NormalFont;
                PreviousAttemptsTextPrefab.font = NormalFont;
            }

            else if (status == false)
            {
                GetComponentInChildren<Text>().text = "Easy Mode OFF";

                Text[] textArray = PreviousAttemptsTextContainer.GetComponentsInChildren<Text>();
                foreach (var text in textArray)
                {
                    text.font = SemaphoreFont;
                }

                ChosenLettersText.font = SemaphoreFont;
                PreviousAttemptsTextPrefab.font = SemaphoreFont;
            }
        }        
    }
}
