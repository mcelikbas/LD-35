using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;
    public Transform platform;

    private Vector3 direction;
    private Transform destination;

    public float speed = 5.0f;

    void Start ()
    {
        SetDestination(pointB);
    }

    void Update ()
    {
        platform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(platform.position, destination.position) < speed * Time.fixedDeltaTime)
        {
            SetDestination(destination == pointA ? pointB : pointA);
        }
    }

    void SetDestination (Transform dest)
    {
        destination = dest;
        direction = (dest.position - platform.position).normalized;
    }
}
