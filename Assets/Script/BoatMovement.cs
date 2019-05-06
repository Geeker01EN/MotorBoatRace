using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{

    [SerializeField] float motorFoamMultiplier;
    [SerializeField] float motorFoamBase;
    [SerializeField] float frontFoamMultiplier;

    [SerializeField] float trust;
    [SerializeField] float turningSpeed;

    float Volume;
    const float pH2O = 1000;

    Rigidbody rb;
    BoxCollider box;
    Water water;

    ParticleSystem.EmissionModule motor, front;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        water = GameObject.Find("Water").GetComponent<Water>();

        motor = transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        front = transform.GetChild(1).GetComponent<ParticleSystem>().emission;
    }

    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") < -0.2f || Input.GetAxis("Horizontal") > 0.2f)
        {
            transform.rotation = Quaternion.EulerRotation(0, transform.rotation.ToEulerAngles().y + Input.GetAxis("Horizontal") * turningSpeed * Time.fixedDeltaTime, 0);
        }
        if (Input.GetAxis("Throttle") > 0.2f)
        {
            rb.AddRelativeForce(Vector3.right * trust * Time.fixedDeltaTime * Input.GetAxis("Throttle"));
        }

        motor.rate = motorFoamMultiplier * Input.GetAxis("Throttle") + motorFoamBase;
        front.rate = frontFoamMultiplier * rb.velocity.magnitude;

        Volume = box.size.x * box.size.z * (water.WaterLevel(transform.position) - transform.position.y);

        if (Volume > 0)
        {
            rb.AddForce(Vector3.up * pH2O * Physics.gravity.magnitude * Volume);
        }
    }
}
