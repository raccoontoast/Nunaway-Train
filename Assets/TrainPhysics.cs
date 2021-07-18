using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainPhysics : MonoBehaviour
{
    Rigidbody rb;
    public float ImpulseForce = 1000f;
    public float ExplosionForce = 500f;
    public float ExplosionRadius = 5f;
    public Transform ExplosionPosition;
    public GameObject[] Nuns;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0f, 0f, ImpulseForce), ForceMode.Impulse);
        Time.timeScale = 0.4f;
    }

    public void Explode()
    {
        Debug.Log("Exlpode");
        foreach (var nun in Nuns)
        {
            nun.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, nun.transform.position, ExplosionRadius);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
        //}
    }
}
