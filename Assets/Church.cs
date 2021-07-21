using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Church : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshCollider[] mcs = GetComponentsInChildren<MeshCollider>();
        foreach (var mc in mcs)
        {
            mc.enabled = false;
            mc.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
