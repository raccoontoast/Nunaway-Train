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
    public GameObject Church;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0f, 0f, ImpulseForce), ForceMode.Impulse);

        if (GameManager.Instance.ChurchSceneCameraCount == 0 || GameManager.Instance.ChurchSceneCameraCount == 2) // Normal camera view
        {
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        else if (GameManager.Instance.ChurchSceneCameraCount == 1) // First person camera view
        {
            Time.timeScale = 0.15f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        Church = FindObjectOfType<Church>().gameObject;
        Nuns = GameObject.FindGameObjectsWithTag("Nun");
    }

    public void Explode()
    {
        Debug.Log("Explode");
        foreach (var nun in Nuns)
        {
            // Explode in a random direction byt varying the point of the explosion slightly
            nun.GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, nun.transform.position + new Vector3(Random.Range(-2f, 2f), -0.5f, Random.Range(-2f, 2f)), ExplosionRadius);
        }

        MeshCollider[] churchMeshColliders = Church.GetComponentsInChildren<MeshCollider>();

        foreach(var collider in churchMeshColliders)
        {
            collider.enabled = true;
            collider.GetComponent<Rigidbody>().useGravity = true;
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
