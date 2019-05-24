using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    float Volume;
    const float pH2O = 1000;

    Rigidbody rb;
    BoxCollider box;
    Water water;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        water = GameObject.Find("Water").GetComponent<Water>();
    }

    void Update()
    {
        Volume = box.size.x * box.size.z * (water.WaterLevel(transform.position) - transform.position.y) / 500;

        if (Volume > 0)
        {
            rb.AddForce(Vector3.up * pH2O * Physics.gravity.magnitude * Volume);
        }
    }
}
