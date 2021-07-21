using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchSceneCamera : MonoBehaviour
{
    public Transform CameraLocation3;

    // Start is called before the first frame update
    void Start()
    {
        switch (GameManager.Instance.ChurchSceneCameraCount)
        {
            case 0: // Normal
                GameManager.Instance.ChurchSceneCameraCount++;
                break;
            case 1: // First Person
                FirstPersonView();
                GameManager.Instance.ChurchSceneCameraCount++;
                break;
            case 2: // Other side
                GetComponent<Transform>().localPosition = CameraLocation3.localPosition;
                GetComponent<Transform>().localRotation = CameraLocation3.localRotation;
                GameManager.Instance.ChurchSceneCameraCount = 0;
                break;
            default:
                break;
        }            
    }

    public void FirstPersonView()
    {
        // Move the camera to a 1st person view and tweak the transform slightly for a better view
        GameObject player = GameObject.FindGameObjectWithTag("ChurchScenePlayer");
        this.transform.position = player.transform.position;
        this.transform.SetParent(player.transform);
        this.transform.localRotation = Quaternion.Euler(14.358f, 0f, 0f);
        this.transform.localPosition = new Vector3(0.056f, 0.352f, 0.002f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
