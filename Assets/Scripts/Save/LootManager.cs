using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private SaveManager saveManager;

    private List<GameObject> lootPrefabs = new List<GameObject>();
    private List<Loot> spawnedLoots = new List<Loot>();
    private List<GameObject> locations = new List<GameObject>();

    private float checkCounter;
    private float checkCounterMax;

    private List<GameObject> otherLoots = new List<GameObject>();

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        lootPrefabs.AddRange(Resources.LoadAll<GameObject>("Loots"));

        checkCounter = checkCounterMax = 900;
    }
    void Start()
    {
        for (int i = 0; i < locations.Count; i++)
        {
            spawnedLoots.Add(null);
        }
    }
    void Update()
    {
        CheckCounter();
    }
    private void CheckCounter()
    {
        if (checkCounter > 0) checkCounter -= Time.deltaTime;
        else
        {
            Check();
            checkCounter = checkCounterMax;
        }
    }
    public void AddLocations(GameObject[] locations)
    {
        this.locations.AddRange(locations);
    }
    private void SpawnLoot(int id, GameObject location)
    {
        LootSpawnLocation lootSpawnLocation = location.GetComponent<LootSpawnLocation>();

        Loots[] lootTypes = lootSpawnLocation.spawnableLootTypes;

        int typeToSpawn = Random.Range(0, lootTypes.Length);

        for (int i = 0; i < lootPrefabs.Count; i++)
        {
            Loot loot = lootPrefabs[i].GetComponent<Loot>();

            if (lootTypes[typeToSpawn] == loot.lootType)
            {
                GameObject myLoot = Instantiate(lootPrefabs[i], location.transform.position, Quaternion.identity, location.transform);

                myLoot.transform.localPosition = lootPrefabs[i].transform.localPosition;
                myLoot.transform.localRotation = lootSpawnLocation.lootsRotation[typeToSpawn];

                myLoot.GetComponent<Loot>().FillItems();
                spawnedLoots[id] = myLoot.GetComponent<Loot>();
                return;
            }
        }
    }
    private void Check()
    {
        StartCoroutine(CheckNumerator());
    }
    IEnumerator CheckNumerator()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < locations.Count; i++)
        {
            if (spawnedLoots[i] == null)
            {
                SpawnLoot(i, locations[i]);
            }
        }
    }
    private GameObject FindLootPrefab(Loots type)
    {
        for (int i = 0; i < lootPrefabs.Count; i++)
        {
            if (lootPrefabs[i].GetComponent<Loot>().lootType == type) return lootPrefabs[i];
        }
        return null;
    }
    private void SaveLoot()
    {
        SaveLoot saveLoot = new SaveLoot();

        for (int i = 0; i < spawnedLoots.Count; i++)
        {
            Loot loot = spawnedLoots[i];
            if (loot == null)
            {
                saveLoot.position.Add(Vector3.zero);
                saveLoot.rotation.Add(Quaternion.identity);
                saveLoot.lootType.Add(Loots.MilitaryChest);
                saveLoot.chest.Add(null);
                continue;
            }

            saveLoot.position.Add(loot.transform.position);
            saveLoot.rotation.Add(loot.transform.rotation);
            saveLoot.lootType.Add(loot.lootType);

            SaveInventory saveChest = new SaveInventory();

            for (int j = 0; j < loot.items.Count; j++)
            {
                if (loot.items[j] != null)
                {
                    saveChest.nickName.Add(loot.items[j].nickName);
                    saveChest.amount.Add(loot.items[j].amount);
                    saveChest.durability.Add(loot.items[j].durability);
                    saveChest.ammoInside.Add(loot.items[j].ammoInside);
                }
                else
                {
                    saveChest.nickName.Add(null);
                    saveChest.amount.Add(0);
                    saveChest.durability.Add(0);
                    saveChest.ammoInside.Add(0);
                }
            }

            saveLoot.chest.Add(saveChest);
        }

        saveLoot.checkCounter = checkCounter;
        StartCoroutine(saveManager.Save(saveLoot, SaveType.Loot));
    }
    private void LoadLoot()
    {
        SaveLoot loadedLoot = JsonUtility.FromJson<SaveLoot>(saveManager.Load(SaveType.Loot));
        if (loadedLoot == null)
        {
            checkCounter = 0;
            return;
        }

        for (int i = 0; i < loadedLoot.position.Count; i++)
        {
            if (loadedLoot.position[i] == Vector3.zero) continue;

            GameObject pref = FindLootPrefab(loadedLoot.lootType[i]);
            if (pref == null) continue;

            GameObject lootObject = Instantiate(pref, loadedLoot.position[i], loadedLoot.rotation[i], locations[i].transform);

            Loot loot = lootObject.GetComponent<Loot>();
            loot.LoadItems(loadedLoot.chest[i]);

            spawnedLoots[i] = loot;
        }

        checkCounter = loadedLoot.checkCounter;
    }
    public void AddOtherLoot(GameObject otherLoot)
    {
        otherLoots.Add(otherLoot);
    }
    public void RemoveOtherLoot(GameObject otherLoot)
    {
        otherLoots.Remove(otherLoot);
    }
    private void SaveOtherLoot()
    {
        SaveOtherLoot saveOtherLoot = new SaveOtherLoot();

        for (int i = 0; i < otherLoots.Count; i++)
        {
            Loot loot = otherLoots[i].GetComponent<Loot>();

            saveOtherLoot.position.Add(otherLoots[i].transform.position);
            saveOtherLoot.rotation.Add(otherLoots[i].transform.rotation);
            saveOtherLoot.lootType.Add(loot.lootType);

            OtherLootDestroyCounter destroyCounter = otherLoots[i].GetComponent<OtherLootDestroyCounter>();

            if (destroyCounter != null)
                saveOtherLoot.destroyCounter.Add(destroyCounter.destroyCounter);
            else
                saveOtherLoot.destroyCounter.Add(0);

            SaveInventory saveChest = new SaveInventory();

            for (int j = 0; j < loot.items.Count; j++)
            {
                if (loot.items[j] != null)
                {
                    saveChest.nickName.Add(loot.items[j].nickName);
                    saveChest.amount.Add(loot.items[j].amount);
                    saveChest.durability.Add(loot.items[j].durability);
                    saveChest.ammoInside.Add(loot.items[j].ammoInside);
                }
                else
                {
                    saveChest.nickName.Add(null);
                    saveChest.amount.Add(0);
                    saveChest.durability.Add(0);
                    saveChest.ammoInside.Add(0);
                }
            }

            saveOtherLoot.chest.Add(saveChest);
        }

        StartCoroutine(saveManager.Save(saveOtherLoot, SaveType.OtherLoot));
    }
    private void LoadOtherLoot()
    {
        SaveOtherLoot saveOtherLoot = JsonUtility.FromJson<SaveOtherLoot>(saveManager.Load(SaveType.OtherLoot));

        if (saveOtherLoot == null) return;

        for (int i = 0; i < saveOtherLoot.position.Count; i++)
        {
            GameObject pref = FindLootPrefab(saveOtherLoot.lootType[i]);
            if (pref == null) continue;

            GameObject lootObject = Instantiate(pref, saveOtherLoot.position[i], saveOtherLoot.rotation[i]);
            Loot loot = lootObject.GetComponent<Loot>();

            OtherLootDestroyCounter destroyCounter = lootObject.GetComponent<OtherLootDestroyCounter>();
            if(destroyCounter != null)
            {
                destroyCounter.SetCounter(saveOtherLoot.destroyCounter[i]);
            }

            loot.LoadItems(saveOtherLoot.chest[i]);

            otherLoots.Add(lootObject);
        }
    }
    public void SaveAll()
    {
        SaveLoot();
        SaveOtherLoot();
    }
    public void LoadAll()
    {
        LoadLoot();
        LoadOtherLoot();
    }
}

[System.Serializable]
public class SaveLoot
{
    public List<Vector3> position = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();
    public List<Loots> lootType = new List<Loots>();
    public List<SaveInventory> chest = new List<SaveInventory>();

    public float checkCounter;
}

[System.Serializable]
public class SaveOtherLoot
{
    public List<Vector3> position = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();
    public List<Loots> lootType = new List<Loots>();
    public List<SaveInventory> chest = new List<SaveInventory>();

    public List<float> destroyCounter = new List<float>();
}
