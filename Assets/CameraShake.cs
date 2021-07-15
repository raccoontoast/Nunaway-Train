using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool shakeOn = true;
    int direction = 1;
    [Range(1, 15)] public float Magnitude = 1;
    [Range(0, 0.1f)] public float interpolationTime = 5f;
    float timeElapsed;
    float interpolationPoint = 0f;
    Vector3 CurrentRotation;
    float TargetZRotation = 360f;
    float PreviousTargetZRotation;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (shakeOn)
            animator.enabled = false;

        CurrentRotation = transform.rotation.eulerAngles;
        PreviousTargetZRotation = transform.rotation.eulerAngles.z;

        //TargetZRotation = new Vector3(0f, 0f, 360f);
    }

    public void SetShakeOn()
    {
        shakeOn = true;        
        animator.enabled = false;                     
    }
    public void SetShakeOff()
    {
        shakeOn = false;
        animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        interpolationPoint = timeElapsed / interpolationTime;
        //Debug.Log(interpolationPoint);

        // If reached Target rotation
        if (CurrentRotation.z == TargetZRotation)
        {
            // Get a new target rotation
            if (direction == 1) // So the camera goes back and forth
                direction = -1;
            else if (direction == -1)
                direction = 1;

            PreviousTargetZRotation = TargetZRotation;
            TargetZRotation = Mathf.PerlinNoise(Time.time, 0f) * direction * Magnitude;

            // Reset time elapsed
            timeElapsed = 0f;
        }
        // Otherwise keep going
        else
        {
            CurrentRotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(PreviousTargetZRotation, TargetZRotation, interpolationPoint));
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(CurrentRotation));
        }
    }
}
