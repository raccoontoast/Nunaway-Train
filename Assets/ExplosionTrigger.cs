using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExplosionTrigger : MonoBehaviour
{
    public event EventHandler<EventArgs> ExplosionTriggered;

    // Start is called before the first frame update
    void Start()
    {
        ExplosionTriggered += testFunction;
    }

    void testFunction(object sender, EventArgs e)
    {
        Debug.Log("Howdy y'all");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TrainPhysics>() != null)
        {
            ExplosionTriggered(this, EventArgs.Empty);
            other.GetComponent<TrainPhysics>().Explode();
        }
    }
}
