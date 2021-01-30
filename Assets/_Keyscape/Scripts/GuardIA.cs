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
        Vector3 endRayPoint = new Vector3(transform.position.x + raycastDistance * Mathf.Cos(coneAngle / 2 * Mathf.Deg2Rad), transform.position.y, transform.position.z + raycastDistance * Mathf.Sin(coneAngle / 2 * Mathf.Deg2Rad));
        Debug.DrawLine(transform.position, endRayPoint); 
        
        Vector3 endRayPoint2 = new Vector3(transform.position.x + raycastDistance * Mathf.Cos(-coneAngle / 2 * Mathf.Deg2Rad), transform.position.y, transform.position.z + raycastDistance * Mathf.Sin(-coneAngle / 2 * Mathf.Deg2Rad));
        Debug.DrawLine(transform.position, endRayPoint2); 
        for (int i = 0; i < rayNumber; i++)
        {
            //Physics.Raycast(transform.position, endRayPoint - transform.position, raycastDistance);
        }
    }
}