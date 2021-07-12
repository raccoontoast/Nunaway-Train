using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainAnimation : MonoBehaviour
{
    public Transform[] TerrainTransforms;
    public int TerrainOffset = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var terrain in TerrainTransforms)
        {
            if (terrain.position.z <= (TerrainTransforms.Length - TerrainOffset) * -10)
            {
                terrain.position = new Vector3(0f, 0f, TerrainOffset*10f);
            }

            terrain.position += new Vector3(0f, 0f, -0.5f);
        }
    }
}
