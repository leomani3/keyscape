using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public float validationTime;
    private void OnTriggerEnter(Collider other)
    {
        PickUpItem item = other.GetComponent<PickUpItem>();
        if (item != null)
        {
            Validate(item);
        }
    }

    private void Validate(PickUpItem item)
    {
        item.transform.DOMove(transform.position, validationTime);
        item.transform.DOScale(0.01f, validationTime);
    }
}
