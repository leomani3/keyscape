using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI countDownText;
    public LevelManagerRef levelManagerRef;
    public List<SpawnPosition> spawnPositions;
    public List<PickUpItem> pickupItemsPrefabs;
    public float countDown;

    private float cleanStuff;

    private void Awake()
    {
        levelManagerRef.levelManager = this;
        cleanStuff = countDown;
        SpawnPickUpItems();
    }

    private void Start()
    {
        GameManager.Instance.ResetCanvas();
    }

    private void SpawnPickUpItems()
    {
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            GameObject item = Instantiate(pickupItemsPrefabs[Random.Range(0, pickupItemsPrefabs.Count)].gameObject, spawnPosition.transform.position, spawnPosition.transform.rotation);
            
            item.GetComponent<PickUpItem>().spawnPosition = spawnPosition;
            spawnPosition.IsTaken = true;
        }
    }

    public SpawnPosition GetNearestAvailablePosition(PickUpItem item)
    {
        SpawnPosition nearestPos = null;
        Vector3 itemPos = item.transform.position;

        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            if (!spawnPosition.IsTaken && (nearestPos == null || Vector3.Distance(itemPos, spawnPosition.transform.position) < Vector3.Distance(itemPos, nearestPos.transform.position)))
            {
                nearestPos = spawnPosition;
            }
        }

        return nearestPos;
    }

    public void SetPositionIsTaken(SpawnPosition spawnPos, bool b)
    {
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            if (spawnPosition == spawnPos)
            {
                spawnPosition.IsTaken = b;
            }
        }
    }

    private void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            countDown = cleanStuff;
            GameManager.Instance.Lose();
        }

        int min = Mathf.FloorToInt(countDown / 60);
        int seconds = Mathf.FloorToInt(countDown % 60);
        countDownText.text = min + " : " + seconds;
    }
}