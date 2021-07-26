using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticleSystem : MonoBehaviour
{
    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        FindObjectOfType<ExplosionTrigger>().ExplosionTriggered += triggerParticleSystem;
    }

    void triggerParticleSystem(object sender, EventArgs e)
    {
        ps.Play();
    }
}
