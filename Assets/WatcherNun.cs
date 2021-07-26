using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherNun : MonoBehaviour
{
    public Transform Target;
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {
        TrainPhysics.TrainSpawned += assignTarget;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = Target.position - transform.position;

        float singleStep = Speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void assignTarget(object sender, EventArgs e)
    {
        Target = FindObjectOfType<TrainPhysics>().transform;
    }
}
