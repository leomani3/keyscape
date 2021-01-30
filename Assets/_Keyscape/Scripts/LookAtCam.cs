using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LookAtCam : MonoBehaviour
{
    private GameObject _cam;

    private void Awake()
    {
        _cam = Camera.main.gameObject;
    }

    private void Update()
    {
        transform.LookAt(_cam.transform.position);
    }
}
