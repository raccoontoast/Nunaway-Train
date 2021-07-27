using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicBlackBars : MonoBehaviour
{
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Appear()
    {
        animator.SetTrigger("In");
    }

    public void Disappear()
    {
        animator.SetTrigger("Out");
    }
}
