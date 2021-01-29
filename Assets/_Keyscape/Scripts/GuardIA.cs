using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class GuardIA : MonoBehaviour
{
    [SerializeField] [Range(2, 100)]private int rayNumber;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float coneAngle;

    private void Update()
    {
        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 endRayPoint = new Vector3(transform.rotation.x, transform.rotation.y - coneAngle, transform.rotation.z) * raycastDistance;
            Debug.DrawLine(transform.position, endRayPoint);
            //Physics.Raycast(transform.position, endRayPoint - transform.position, raycastDistance);
        }
    }
}