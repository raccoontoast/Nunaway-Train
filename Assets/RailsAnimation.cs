using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailsAnimation : MonoBehaviour
{
    public Transform TeleportLocationTransform;
    public Transform[] Rails;

    private void FixedUpdate()
    {
        foreach (var rail in Rails)
        {
            // Move the rails at the same rate as the terrain
            rail.transform.position += new Vector3(0f, 0f, -0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Move the rail to the start once it reaches the trigger
        other.transform.position = TeleportLocationTransform.position;
    }
}
