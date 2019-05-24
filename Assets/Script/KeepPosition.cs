using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPosition : MonoBehaviour
{

    Vector3 pos;
    Rigidbody rb;
    [SerializeField] [Range(0f, 1f)] float strenght;
    [SerializeField] float radiusCheck;

    public Vector3 PositionToMaintain
    {
        set
        {
            pos = value;
        }
    }

    void Start()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(radiusCheck, radiusCheck, radiusCheck));

        if (colliders.Length > 1)
        {
            Destroy(transform.gameObject);
        }

        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce((pos - transform.position) * strenght);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), Time.fixedDeltaTime);
    }
}
