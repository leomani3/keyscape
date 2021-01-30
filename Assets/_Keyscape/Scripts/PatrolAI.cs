using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public enum PatroType{Looping, Yoyo}

public class PatrolAI : MonoBehaviour
{
    [Separator("Reference")]
    public GameObject checkPointsParent;

    [Separator("settings")]
    public float speed;
    [SerializeField] private PatroType patrolType;
    [SerializeField] [Range(0f, 1f)] private float rotationSpeed;

    private int _currentCheckPoints;
    private bool sens;
    private Rigidbody _rb;
    private List<Transform> _checkPoints = new List<Transform>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        for (int i = 0; i < checkPointsParent.transform.childCount; i++)
        {
            _checkPoints.Add(checkPointsParent.transform.GetChild(i));
        }
        
        transform.position = new Vector3(_checkPoints[0].position.x, transform.position.y, _checkPoints[0].position.z);
    }

    private void FixedUpdate()
    {
        Vector3 checkPointPos = new Vector3(_checkPoints[_currentCheckPoints].position.x, transform.position.y, _checkPoints[_currentCheckPoints].position.z);
        
        _rb.velocity = (checkPointPos - transform.position).normalized * speed;
        
        Quaternion targetRotation = Quaternion.LookRotation( new Vector3(checkPointPos.x, transform.position.y, checkPointPos.z) - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        
        if (Vector3.Distance(transform.position, checkPointPos) < 0.1f)
        {
            NextCheckPoint();
        }
    }

    private void NextCheckPoint()
    {
        switch (patrolType)
        {
            case PatroType.Looping :
                
                if (_currentCheckPoints < _checkPoints.Count - 1)
                    _currentCheckPoints++;
                else
                _currentCheckPoints = 0;
                
                    break;
            case PatroType.Yoyo :

                if (sens)
                {
                    if (_currentCheckPoints < _checkPoints.Count - 1)
                        _currentCheckPoints++;
                    else
                        sens = !sens;
                }
                else
                {
                    if (_currentCheckPoints > 0)
                        _currentCheckPoints--;
                    else
                        sens = !sens;
                }
                
                break;
        }
    }
}
