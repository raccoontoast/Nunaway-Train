using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchNun : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        FindObjectOfType<ExplosionTrigger>().ExplosionTriggered += enableGravity;
    }

    void enableGravity(object sender, EventArgs e)
    {
        rb.useGravity = true;
    }
}
