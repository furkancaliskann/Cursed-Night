using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private EventManager eventManager;
    private SaveManager saveManager;
    private SaveSpawnObjects saveSpawnObjects;
    private Terrain terrain;
    private TimeController timeController;

    private int maxForestTreeCount;
    private int maxSnowyForestTreeCount;
    private int maxDesertTreeCount;
    private int maxRockCount;
    private int maxCollectableCount;

    [SerializeField] private GameObject forestTreePrefab1, forestTreePrefab2;
    [SerializeField] private GameObject snowyForestTreePrefab1, snowyForestTreePrefab2, snowyForestTreePrefab3, snowyForestTreePrefab4;
    [SerializeField] private GameObject desertTreePrefab1, desertTreePrefab2;

    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject coalRockPrefab;
    [SerializeField] private GameObject sulfurRockPrefab;
    [SerializeField] private GameObject ironRockPrefab;

    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject woodPilePrefab;
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject birdNestPrefab;

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject crawlZombiePrefab;
    [SerializeField] private GameObject deerPrefab;

    [SerializeField] private GameObject planeObject;
    [SerializeField] private Plane plane;

    private Vector2 forestStartPos = new Vector2(0, 0);
    private Vector2 forestFinishPos = new Vector2(2048, 950);
    private Vector2 snowyForestStartPos = new Vector2(1024, 1000);
    private Vector2 snowyForestFinishPos = new Vector2(2048, 2048);
    private Vector2 desertStartPos = new Vector2(0, 1000);
    private Vector2 desertFinishPos = new Vector2(975, 2048);

    public List<GameObject> treeList = new List<GameObject>();
    public List<GameObject> rockList = new List<GameObject>();
    public List<GameObject> collectableList = new List<GameObject>();

    public List<GameObject> zombieList = new List<GameObject>();
    public int zombieCount;
    public List<GameObject> animalList = new List<GameObject>();
    public int animalCount;

    public DateTime? nextZombieSpawnTime = null;
    public DateTime? nextAnimalSpawnTime = null;

    private GameObject player;
    public GameObject spawnTesterPrefab;

    private bool checkStarted;
    public bool spawnCompleted {  get; private set; }
    public bool spawnNaturelResources;


    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        saveManager = GetComponent<SaveManager>();
        saveSpawnObjects = GetComponent<SaveSpawnObjects>();
        terrain = Terrain.activeTerrain;
        timeController = GetComponent<TimeController>();
        plane = planeObject.GetComponent<Plane>();
    }
    void Start()
    {
        DestroyZombiesStart();
        DestroyAnimalsStart();

        if (!spawnNaturelResources) return;

        maxForestTreeCount = 2000;
        maxSnowyForestTreeCount = 2000;
        maxDesertTreeCount = 2000;
        maxCollectableCount = 7500;
        maxRockCount = 500;
    }
    void Update()
    {
        if(saveSpawnObjects.allObjectsLoaded && !checkStarted && !spawnCompleted)
        {
            checkStarted = true;
            Invoke(nameof(CheckTrees), 0.5f);
            Invoke(nameof(CheckRocks), 0.5f);
            Invoke(nameof(CheckCollectables), 0.5f);
            Invoke(nameof(SetSpawnCompletedBool), 1f);
        }
        CheckTimeForPlane();
        CheckZombieCount();
        CheckAnimalCount();
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    private void SetSpawnCompletedBool()
    {
        spawnCompleted = true;
    }
    private void CheckTimeForPlane()
    {
        DateTime currentTime = timeController.GetTime();

        if (currentTime.Hour == 12 && currentTime.Minute == 5)
        {
            SpawnPlane();
        }
    }
    private void CheckTrees()
    {
        List<GameObject> trees = new List<GameObject>();
        trees.AddRange(GameObject.FindGameObjectsWithTag("Tree"));

        int forestTreeCount = 0;
        int snowyForestTreeCount = 0;
        int desertTreeCount = 0;

        for (int i = 0; i < trees.Count; i++)
        {
            if (trees[i].GetComponent<TreeCode>().type == TreeType.Forest) forestTreeCount++;
            else if (trees[i].GetComponent<TreeCode>().type == TreeType.SnowyForest) snowyForestTreeCount++;
            else if (trees[i].GetComponent<TreeCode>().type == TreeType.Desert) desertTreeCount++;
        }


        int addingForestTreeCount = maxForestTreeCount - forestTreeCount;
        int addingSnowyForestTreeCount = maxSnowyForestTreeCount - snowyForestTreeCount;
        int addingDesertTreeCount = maxDesertTreeCount - desertTreeCount;

        if (addingForestTreeCount > 0)
            for (int i = 0; i < addingForestTreeCount; i++)
            {
                int prefabModel = UnityEngine.Random.Range(1, 3);
                SpawnForestTree(CreateRandomPosition(forestStartPos, forestFinishPos), prefabModel, 500);
            }

        if (addingSnowyForestTreeCount > 0)
            for (int i = 0; i < addingSnowyForestTreeCount; i++)
            {
                int prefabModel = UnityEngine.Random.Range(1, 5);
                SpawnSnowyForestTree(CreateRandomPosition(snowyForestStartPos, snowyForestFinishPos), prefabModel, 500);
            }

        if (addingDesertTreeCount > 0)
            for (int i = 0; i < addingDesertTreeCount; i++)
            {
                int prefabModel = UnityEngine.Random.Range(1, 3);
                SpawnDesertTree(CreateRandomPosition(desertStartPos, desertFinishPos), prefabModel, 500);
            }
    }
    private void CheckRocks()
    {
        List<GameObject> rocks = new List<GameObject>();

        rocks.AddRange(GameObject.FindGameObjectsWithTag("Rock"));

        int addingRockCount = maxRockCount - rocks.Count;

        if (addingRockCount > 0)
            for (int i = 0; i < addingRockCount; i++)
            {
                int sayi = UnityEngine.Random.Range(0, 4);
                SpawnRock((RockType)sayi, CreateRandomPosition(), sayi, 1000);
            }
    }
    private void CheckCollectables()
    {
        for (int i = 0; i < collectableList.Count; i++)
        {
            if (collectableList[i] == null) collectableList.RemoveAt(i);
        }

        int addingCollectableCount = maxCollectableCount - collectableList.Count;

        for (int i = 0; i < addingCollectableCount; i++)
        {
            int randomCollectable = UnityEngine.Random.Range(0, 4);
            if (randomCollectable == 0) SpawnStone(CreateRandomPosition());
            else if (randomCollectable == 1) SpawnMushroom(CreateRandomPosition());
            else if (randomCollectable == 2) SpawnWoodPiles(CreateRandomPosition());
            else SpawnBirdNest(CreateRandomPosition());
        }
    }
    public void SpawnTreeWithType(Vector3 position, TreeType type, int prefabModel, int health)
    {
        switch (type)
        {
            case TreeType.Forest: SpawnForestTree(position, prefabModel, health); break;
            case TreeType.SnowyForest: SpawnSnowyForestTree(position, prefabModel, health); break;
            case TreeType.Desert: SpawnDesertTree(position, prefabModel, health); break;
        }
    }
    public void AddTree(GameObject tree)
    {
        treeList.Add(tree);
    }
    public void RemoveTree(GameObject tree)
    {
        treeList.Remove(tree);
    }
    public void SpawnRockWithType(Vector3 position, RockType type, int prefabModel, int health)
    {
        switch (type)
        {
            case RockType.Stone: SpawnRock(RockType.Stone, position, prefabModel, health); break;
            case RockType.Coal: SpawnRock(RockType.Coal, position, prefabModel, health); break;
            case RockType.Sulfur: SpawnRock(RockType.Sulfur, position, prefabModel, health); break;
            case RockType.Iron: SpawnRock(RockType.Iron, position, prefabModel, health); break;
        }
    }
    public void AddRock(GameObject rock)
    {
        rockList.Add(rock);
    }
    public void RemoveRock(GameObject rock)
    {
        rockList.Remove(rock);
    }
    public void SpawnCollectableWithType(Vector3 position, CollectableType type)
    {
        switch (type)
        {
            case CollectableType.Stone: SpawnStone(position); break;
            case CollectableType.WoodPile: SpawnWoodPiles(position); break;
            case CollectableType.Mushroom: SpawnMushroom(position); break;
            case CollectableType.BirdNest: SpawnBirdNest(position); break;
        }
    }
    public void AddCollectable(GameObject collectable)
    {
        collectableList.Add(collectable);
    }
    public void RemoveCollectable(GameObject collectable)
    {
        collectableList.Remove(collectable);
    }
    public void SpawnForestTree(Vector3 position, int prefabModel, int health)
    {
        GameObject a;

        if (prefabModel == 1)
        {
            a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, forestTreePrefab1, prefabModel, health);
        }
        else if (prefabModel == 2)
        {
            a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, forestTreePrefab2, prefabModel, health);
        }
    }
    public void SpawnSnowyForestTree(Vector3 position, int prefabModel, int health)
    {
        GameObject a;

        switch (prefabModel)
        {
            case 1:
                a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
                a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, snowyForestTreePrefab1, prefabModel, health);
                break;
            case 2:
                a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
                a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, snowyForestTreePrefab2, prefabModel, health);
                break;
            case 3:
                a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
                a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, snowyForestTreePrefab3, prefabModel, health);
                break;
            case 4:
                a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
                a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, snowyForestTreePrefab4, prefabModel, health);
                break;
        }
    }
    public void SpawnDesertTree(Vector3 position, int prefabModel, int health)
    {
        GameObject a;
        if (prefabModel == 1)
        {
            a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, desertTreePrefab1, prefabModel, health);
        }
        else if (prefabModel == 2)
        {
            a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Tree, desertTreePrefab2, prefabModel, health);
        }
    }
    private void SpawnRock(RockType rockType, Vector3 position, int prefabModel, int health)
    {
        if (rockType == RockType.Stone)
        {
            GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Rock, rockPrefab, prefabModel, health);
        }

        else if (rockType == RockType.Coal)
        {
            GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Rock, coalRockPrefab, prefabModel, health);
        }
        else if (rockType == RockType.Sulfur)
        {
            GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Rock, sulfurRockPrefab, prefabModel, health);
        }
        else if (rockType == RockType.Iron)
        {
            GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
            a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Rock, ironRockPrefab, prefabModel, health);
        }
    }
    private void SpawnStone(Vector3 position)
    {
        GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
        a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Collectable, stonePrefab, 0, 100);
    }
    private void SpawnWoodPiles(Vector3 position)
    {
        GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
        a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Collectable, woodPilePrefab, 0, 100);
    }
    private void SpawnMushroom(Vector3 position)
    {
        GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
        a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Collectable, mushroomPrefab, 0, 100);
    }
    private void SpawnBirdNest(Vector3 position)
    {
        GameObject a = Instantiate(spawnTesterPrefab, position, Quaternion.identity);
        a.GetComponent<SpawnTester>().SetPrefab(this, SpawnType.Collectable, birdNestPrefab, 0, 100);
    }
    private void SpawnZombie()
    {
        if (player == null) return;

        int randomNumber = UnityEngine.Random.Range(0, 2);

        Vector3 spawnPos;
        Vector2 startPos = new Vector2(player.transform.position.x +  UnityEngine.Random.Range(-45, -80), player.transform.position.z + UnityEngine.Random.Range(-45, -80));
        Vector2 finishPos = new Vector2(player.transform.position.x +  UnityEngine.Random.Range(45, 80), player.transform.position.z + UnityEngine.Random.Range(45, 80));

        spawnPos = CreateRandomPosition(startPos, finishPos) + new Vector3(0, 1f, 0);

        if (!NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;

        GameObject zombieObject;

        if(randomNumber == 0)
        {
            zombieObject = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        }    
        else
        {
            zombieObject = Instantiate(crawlZombiePrefab, spawnPos, Quaternion.identity);
        }
        
        zombieList.Add(zombieObject);
    }
    private void CheckZombieList()
    {
        if (zombieList.Count == 0) return;

        for (int i = 0; i < zombieList.Count; i++)
        {
            if (zombieList[i] == null || zombieList[i].transform.tag != "Zombie")
            {
                zombieList.RemoveAt(i);
                zombieCount = zombieList.Count;
            }
            else if (player == null || Vector3.Distance(zombieList[i].transform.position, player.transform.position) > 150)
            {
                Destroy(zombieList[i]);
                zombieList.RemoveAt(i);
                zombieCount = zombieList.Count;
            }
        }
    }
    private void CheckZombieCount()
    {
        if (player == null) return;

        CheckZombieList();

        if (eventManager.eventStarted) zombieCount = 20;
        else
        {
            DateTime currentTime = timeController.GetTime();
            if (currentTime.Day == 1 && currentTime.Hour == 0) return;

            if (nextZombieSpawnTime == null || currentTime >= nextZombieSpawnTime)
            {
                if (currentTime >= nextZombieSpawnTime)
                    zombieCount = UnityEngine.Random.Range(3, 10);

                nextZombieSpawnTime = timeController.GetTime() + TimeSpan.FromMinutes(UnityEngine.Random.Range(10, 40));
            }
        }

        if(zombieCount > zombieList.Count)
        {

            SpawnZombie();
        }
    }
    private void SpawnAnimal()
    {
        if (player == null) return;

        Vector3 spawnPos;
        Vector2 startPos = new Vector2(player.transform.position.x + UnityEngine.Random.Range(-45, -80), player.transform.position.z + UnityEngine.Random.Range(-45, -80));
        Vector2 finishPos = new Vector2(player.transform.position.x + UnityEngine.Random.Range(45, 80), player.transform.position.z + UnityEngine.Random.Range(45, 80));

        spawnPos = CreateRandomPosition(startPos, finishPos) + new Vector3(0, 1f, 0);

        if (!NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;

        GameObject animalObject = Instantiate(deerPrefab, spawnPos, Quaternion.identity);

        animalList.Add(animalObject);
    }
    private void CheckAnimalList()
    {
        if (animalList.Count == 0) return;

        for (int i = 0; i < animalList.Count; i++)
        {
            if (animalList[i] == null || animalList[i].transform.tag != "Animal")
            {
                animalList.RemoveAt(i);
                animalCount = animalList.Count;
            }
            else if (player == null || Vector3.Distance(animalList[i].transform.position, player.transform.position) > 150)
            {
                Destroy(animalList[i]);
                animalList.RemoveAt(i);
                animalCount = animalList.Count;
            }
        }
    }
    private void CheckAnimalCount()
    {
        if (player == null) return;

        CheckAnimalList();

        DateTime currentTime = timeController.GetTime();
        if (currentTime.Day == 1 && currentTime.Hour == 0) return;

        if (nextAnimalSpawnTime == null || currentTime >= nextAnimalSpawnTime)
        {
            if (currentTime >= nextAnimalSpawnTime)
                animalCount = UnityEngine.Random.Range(1, 3);

            nextAnimalSpawnTime = timeController.GetTime() + TimeSpan.FromMinutes(UnityEngine.Random.Range(30, 100));
        }

        if (animalCount > animalList.Count)
        {
            SpawnAnimal();
        }
    }
    public bool SpawnPlane()
    {
        if (plane.flyStarted) return false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float spawnX;
        float spawnZ;
        float targetX;
        float targetZ;

        if (player == null)
        {
            spawnX = UnityEngine.Random.Range(20, terrain.terrainData.size.x - 20);
            spawnZ = UnityEngine.Random.Range(20, terrain.terrainData.size.x - 20);
            targetX = UnityEngine.Random.Range(20, terrain.terrainData.size.x - 20);
            targetZ = UnityEngine.Random.Range(20, terrain.terrainData.size.x - 20);
        }
        else
        {
            spawnX = player.transform.position.x;
            spawnZ = player.transform.position.z;
            targetX = player.transform.position.x;
            targetZ = player.transform.position.z;

            int vertical = UnityEngine.Random.Range(0, 2);
            int horizontal = UnityEngine.Random.Range(0, 2);

            if (vertical == 1)
            {
                spawnZ += UnityEngine.Random.Range(250, 400);
                targetZ -= UnityEngine.Random.Range(350, 700);
            }
            else
            {
                spawnZ -= UnityEngine.Random.Range(250, 400);
                targetZ += UnityEngine.Random.Range(350, 700);
            }

            if (horizontal == 1)
            {
                spawnX += UnityEngine.Random.Range(250, 400);
                targetX -= UnityEngine.Random.Range(350, 700);
            }
            else
            {
                spawnX -= UnityEngine.Random.Range(250, 400);
                targetX += UnityEngine.Random.Range(350, 700);
            }
        }

        planeObject.transform.position = new Vector3(spawnX, 300, spawnZ);
        planeObject.SetActive(true);
        plane.SetTarget(new Vector3(targetX, 300, targetZ));
        return true;
    }
    private Vector3 CreateRandomPosition(Vector2 startPos, Vector2 finishPos)
    {
        float x = UnityEngine.Random.Range(startPos.x, finishPos.x);
        float z = UnityEngine.Random.Range(startPos.y, finishPos.y);

        Vector3 scale = terrain.terrainData.heightmapScale;

        float firstY = terrain.terrainData.GetHeight(Mathf.RoundToInt(x / scale.x), Mathf.RoundToInt(z / scale.z));
        float y = terrain.SampleHeight(new Vector3(x, firstY, z));

        return new Vector3(x, y, z);
    }
    private Vector3 CreateRandomPosition()
    {
        float x = UnityEngine.Random.Range(25, terrain.terrainData.size.x - 25);
        float z = UnityEngine.Random.Range(25, terrain.terrainData.size.z - 25);

        Vector3 scale = terrain.terrainData.heightmapScale;

        float firstY = terrain.terrainData.GetHeight(Mathf.RoundToInt(x / scale.x), Mathf.RoundToInt(z / scale.z));
        float y = terrain.SampleHeight(new Vector3(x, firstY, z));

        return new Vector3(x, y, z);
    }
    private void DestroyZombiesStart()
    {
        if (saveManager.gameName == null) return;

        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

        for (int i = 0; i < zombies.Length; i++)
        {
            Destroy(zombies[i]);
        }
    }
    private void DestroyAnimalsStart()
    {
        if (saveManager.gameName == null) return;
        GameObject[] animals = GameObject.FindGameObjectsWithTag("Animal");

        for (int i = 0; i < animals.Length; i++)
        {
            Destroy(animals[i]);
        }
    }
}
