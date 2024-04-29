using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    SpawnPoint[] spawnPoints;
    private void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }
    public Transform getSpawmPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
