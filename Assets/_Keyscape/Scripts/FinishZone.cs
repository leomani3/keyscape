using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
    public int nbItemNeeded;
    public float validationTime;
    public TextMeshPro ui;

    private int currentValidatedItems = 0;

    private void Awake()
    {
        UpdateUi();
    }

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
        item.transform.DOScale(0.01f, validationTime).OnComplete(() => DisableItem(item));
        currentValidatedItems++;
        if (currentValidatedItems >= nbItemNeeded)
        {
            GameManager.Instance.Win();
        }

        UpdateUi();
    }

    private void DisableItem(PickUpItem item)
    {
        item.StopInteraction();
        item.gameObject.SetActive(false);
    }

    private void UpdateUi()
    {
        ui.text = currentValidatedItems.ToString() + " / " + nbItemNeeded.ToString();
    }
}
