using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    private float minDistance = 1.5f;
    private float maxDistance = 5f;

    private float offset = 10;

    private float distance;


    Vector3 direction;

    void Start()
    {
        direction = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 camPos = transform.parent.TransformPoint(direction * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, camPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance * 0.85f, minDistance, maxDistance);
        }

        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, direction* distance, offset * Time.deltaTime);
    }
}
