using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDance : MonoBehaviour
{
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Shimmy()
    {
        animator.SetTrigger("Beat");
    }
}
