using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public LevelManagerRef levelManagerRef;
    public List<SpawnPosition> spawnPositions;
    public List<PickUpItem> pickupItemsPrefabs;

    private void Awake()
    {
        levelManagerRef.levelManager = this;

        SpawnPickUpItems();
    }

    private void SpawnPickUpItems()
    {
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            Instantiate(pickupItemsPrefabs[Random.Range(0, pickupItemsPrefabs.Count)].gameObject, spawnPosition.transform.position, spawnPosition.transform.rotation);
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
}