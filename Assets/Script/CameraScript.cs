using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 velocityOffset;
    [SerializeField] float velocityMultiplier;

    [SerializeField] Vector3 restingAngle;
    [SerializeField] Vector3 movingAngle;
    [SerializeField] float velocityLimit;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }


    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + velocityOffset * player.GetComponent<Rigidbody>().velocity.magnitude / velocityMultiplier, Time.deltaTime);

        if (player.GetComponent<Rigidbody>().velocity.magnitude < velocityLimit) transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(restingAngle), Time.deltaTime);
        if (player.GetComponent<Rigidbody>().velocity.magnitude >= velocityLimit) transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(movingAngle), Time.deltaTime);
    }
}
