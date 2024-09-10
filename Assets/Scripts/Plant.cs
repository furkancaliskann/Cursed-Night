using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private ItemList itemList;
    private TimeController timeController;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material wateredMaterial;
    [SerializeField] private Transform modelParent;

    [SerializeField] private AudioClip plantSound;
    [SerializeField] private AudioClip wateringPlantSound;
    private AudioSource audioSource;

    private List<PlantType> plants = new List<PlantType>();

    public bool isPlanted { get; private set; }
    public bool isWatered { get; private set; }
    public bool readyForHarvest { get; private set; }
    public PlantType plantedType {  get; private set; }
    private GameObject spawnedPlant;
    public DateTime? harvestTime { get; private set; }
    

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        timeController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeController>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();

        AddPlant("potato_seed", 23, "potato", 3);
        AddPlant("tomato_seed", 23, "tomato", 3);
        AddPlant("pepper_seed", 23, "pepper", 3);
        AddPlant("mushroom_seed", 27, "mushroom", 3);
        AddPlant("onion_seed", 23, "onion", 3);
        AddPlant("cucumber_seed", 23, "cucumber", 3);
        AddPlant("corn_seed", 23, "corn", 3);
        AddPlant("rosehip_seed", 25, "rosehip", 2);
        AddPlant("cotton_seed", 30, "cotton", 3);
    }
    void Update()
    {
        CheckHarvestTime();
    }
    private void AddPlant(string plantName, int growTime, string harvestNickName, int harvestAmount)
    {
        plants.Add(new PlantType
        {
            plantName = plantName,
            growTime = growTime,
            growingPrefab = Resources.Load<GameObject>("Plants/Growing/" + plantName),
            growedPrefab = Resources.Load<GameObject>("Plants/Growed/" + plantName),
            harvestItem = itemList.CreateNewItem(harvestNickName, harvestAmount)
        });
    }
    private void CheckHarvestTime()
    {
        if (!isPlanted || !isWatered || harvestTime == null || readyForHarvest) return;

        DateTime time = timeController.GetTime();

        if (time >= harvestTime)
        {
            readyForHarvest = true;
            SpawnPrefab(plantedType.growedPrefab);
        }
    }
    public void PlantSeed(string seedName)
    {
        if (isPlanted) return;

        PlantType type = GetSeed(seedName);
        if (type == null) return;

        PlaySound(plantSound);
        plantedType = type;
        isPlanted = true;
        SpawnPrefab(plantedType.growingPrefab);
    }
    public void WaterSeed()
    {
        PlaySound(wateringPlantSound);
        isWatered = true;
        meshRenderer.material = wateredMaterial;
        harvestTime = timeController.GetTime() + TimeSpan.FromHours(plantedType.growTime);
    }
    private void SpawnPrefab(GameObject prefab)
    {
        if(spawnedPlant != null) Destroy(spawnedPlant);

        spawnedPlant = Instantiate(prefab, modelParent);
        spawnedPlant.transform.localPosition = Vector3.zero;
    }
    private PlantType GetSeed(string seedName)
    {
        var item = plants.Find(x => x.plantName == seedName);
        if (item == null) return null;
        return item;
    }
    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void ResetVariables()
    {
        isPlanted = false;
        isWatered = false;
        plantedType = null;
        Destroy(spawnedPlant);
        spawnedPlant = null;
        harvestTime = null;
        readyForHarvest = false;

        meshRenderer.material = originalMaterial;
    }
    public void LoadPlant(SavePlant loaded)
    {
        isPlanted = loaded.isPlanted;
        isWatered = loaded.isWatered;
        readyForHarvest = loaded.readyForHarvest;
        plantedType = GetSeed(loaded.plantName);

        if(loaded.harvestTime != "")
        harvestTime = DateTime.Parse(loaded.harvestTime);

        if (isPlanted && !readyForHarvest) SpawnPrefab(plantedType.growingPrefab);
        else if (isPlanted && readyForHarvest) SpawnPrefab(plantedType.growedPrefab);
        if (isWatered) meshRenderer.material = wateredMaterial;
    }
}

[System.Serializable]
public class PlantType
{
    public string plantName;
    public int growTime; // Hour
    public GameObject growingPrefab;
    public GameObject growedPrefab;
    public Item harvestItem;
}


