using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] [Range(2, 200)]private int rayNumber;
    [SerializeField] private float raycastDistance;
    [SerializeField] private float coneAngle;
    [SerializeField] private Material material;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    
    [SerializeField] private float _angleStep;
    [SerializeField] private Vector3[] _points;
    [SerializeField] private int[] _tris;
    [SerializeField] private int _indexPoints;
    [SerializeField] private int _indexTris;

    private RaycastHit _hit;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer.material = material;
    }

    private void Update()
    {
        _mesh = new Mesh();
        
        _points = new Vector3[rayNumber + 1];
        _tris = new int[(rayNumber - 1) * 3];
        _indexPoints = 0;
        _indexTris = 0;
        
        _points[0] = transform.parent.InverseTransformPoint(new Vector3(transform.parent.position.x, transform.parent.position.y - (transform.parent.lossyScale.y) + 0.1f, transform.parent.position.z));
        _indexPoints++;

        if (rayNumber - 1 != 0)
            _angleStep = coneAngle / (rayNumber - 1);
        else
            _angleStep = coneAngle;
        

        for (int i = 0; i < rayNumber; i++)
        {
            Vector3 rayPoint = new Vector3(transform.position.x + raycastDistance * Mathf.Cos((-coneAngle / 2 + (_angleStep * i) + 90 - transform.eulerAngles.y) * Mathf.Deg2Rad), transform.position.y , transform.position.z + raycastDistance * Mathf.Sin((-coneAngle / 2 + (_angleStep * i) + 90 - transform.eulerAngles.y) * Mathf.Deg2Rad));
            if (Physics.Raycast(transform.position, rayPoint - transform.position, out _hit, raycastDistance))
            {
                PickUpItem item = _hit.collider.GetComponent<PickUpItem>();
                if (item != null)
                {
                    item.SeenByGuard();
                }
                rayPoint = _hit.point;
            }
            _points[_indexPoints] = transform.parent.InverseTransformPoint(new Vector3(rayPoint.x, transform.parent.position.y - (transform.parent.lossyScale.y) + 0.1f, rayPoint.z));
            _indexPoints++;

            if (i > 0)
            {
                _tris[_indexTris] = 0;
                _tris[_indexTris + 1] = i + 1;
                _tris[_indexTris + 2] = i;
                _indexTris += 3;
            }
        }

        _mesh.vertices = _points;
        _mesh.triangles= _tris;
        _meshFilter.transform.position = transform.parent.position;
        _meshFilter.mesh = _mesh;
    }
}