using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCanvas : MonoBehaviour
{
    public void DestroyFadeCanvas()
    {
        Destroy(GetComponentInParent<Canvas>().gameObject);
    }

    public void FadeComplete()
    {
        FindObjectOfType<MainMenuButtons>().FadeComplete();
    }
}
