using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnManager : MonoBehaviour
{
    private LootManager lootManager;
    [SerializeField] private GameObject[] locations;

    void Awake()
    {
        lootManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootManager>();
        lootManager.AddLocations(locations);
    }
}
