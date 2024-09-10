using System.Collections.Generic;
using UnityEngine;

public class SaveBuildings : MonoBehaviour
{
    private ItemList itemList;
    private SaveManager saveManager;

    public List<GameObject> blocks = new List<GameObject>();

    void Awake()
    {
        itemList = GetComponent<ItemList>();
        saveManager = GetComponent<SaveManager>();
    }
    public void AddBlock(GameObject block)
    {
        blocks.Add(block);
    }
    public void RemoveBlock(GameObject block)
    {
        blocks.Remove(block);
    }
    public void SaveBlock()
    {
        SaveBlock saveBlock = new SaveBlock();

        for (int i = 0; i < blocks.Count; i++)
        {
            Block block = blocks[i].GetComponent<Block>();

            saveBlock.chests.Add(TrySaveChest(block.GetComponent<Chest>()));
            saveBlock.furnaces.Add(TrySaveFurnace(block.GetComponent<Furnace>()));
            saveBlock.plants.Add(TrySavePlant(block.GetComponent<Plant>()));
            saveBlock.doors.Add(TrySaveDoor(block.GetComponent<Door>()));
            saveBlock.campfires.Add(TrySaveCampfire(block.GetComponent<Campfire>()));

            saveBlock.position.Add(blocks[i].transform.position);
            saveBlock.rotation.Add(blocks[i].transform.rotation);
            saveBlock.nickName.Add(block.nickName);
            saveBlock.durability.Add(block.durability);
        }

        StartCoroutine(saveManager.Save(saveBlock, SaveType.Block));
    }
    public void LoadBlock()
    {
        SaveBlock loadedBlock = JsonUtility.FromJson<SaveBlock>(saveManager.Load(SaveType.Block));
        if (loadedBlock == null) return;

        for (int i = 0; i < loadedBlock.position.Count; i++)
        {
            Item item = itemList.GetItem(loadedBlock.nickName[i]);
            if (item == null) continue;

            GameObject placedObject = Instantiate(item.blockPrefab, loadedBlock.position[i], loadedBlock.rotation[i]);
            placedObject.GetComponent<Block>().SetValues(item.nickName, item.durability);

            AddBlock(placedObject);

            TryLoadChest(item, placedObject, loadedBlock.chests[i]);
            TryLoadFurnace(item, placedObject, loadedBlock.furnaces[i]);
            TryLoadPlant(item, placedObject, loadedBlock.plants[i]);
            TryLoadDoor(item, placedObject, loadedBlock.doors[i]);
            TryLoadCampfire(item, placedObject, loadedBlock.campfires[i]);
        }
    }
    private SaveInventory TrySaveChest(Chest chest)
    {
        if (chest == null) return null;

        SaveInventory saveInventory = new SaveInventory();

        for (int j = 0; j < chest.items.Count; j++)
        {
            if (chest.items[j] != null)
            {
                saveInventory.nickName.Add(chest.items[j].nickName);
                saveInventory.amount.Add(chest.items[j].amount);
                saveInventory.durability.Add(chest.items[j].durability);
                saveInventory.ammoInside.Add(chest.items[j].ammoInside);
            }
            else
            {
                saveInventory.nickName.Add(null);
                saveInventory.amount.Add(0);
                saveInventory.durability.Add(0);
                saveInventory.ammoInside.Add(0);
            }
        }
        return saveInventory;
    }
    private void TryLoadChest(Item item, GameObject placedObject, SaveInventory items)
    {
        if (item.blockType == BlockTypes.Chest)
        {
            Chest chest = placedObject.GetComponent<Chest>();

            for (int j = 0; j < items.nickName.Count; j++)
            {
                chest.items[j] = itemList.CreateNewItem(items.nickName[j],
                    items.amount[j], items.durability[j], items.ammoInside[j]);
            }
        }
    }
    private SaveFurnace TrySaveFurnace(Furnace furnace)
    {
        if (furnace == null) return null;

        SaveFurnace saveFurnace = new SaveFurnace();

        SaveInventory smeltItem = new SaveInventory();
        SaveInventory smeltedItem = new SaveInventory();
        SaveInventory fuelItem = new SaveInventory();

        if (furnace.smeltItem != null)
        {
            smeltItem.nickName.Add(furnace.smeltItem.nickName);
            smeltItem.amount.Add(furnace.smeltItem.amount);
            smeltItem.durability.Add(furnace.smeltItem.durability);
            smeltItem.ammoInside.Add(furnace.smeltItem.ammoInside);
        } 

        if(furnace.smeltedItem != null)
        {
            smeltedItem.nickName.Add(furnace.smeltedItem.nickName);
            smeltedItem.amount.Add(furnace.smeltedItem.amount);
            smeltedItem.durability.Add(furnace.smeltedItem.durability);
            smeltedItem.ammoInside.Add(furnace.smeltedItem.ammoInside);
        }

        if(furnace.fuelItem != null)
        {
            fuelItem.nickName.Add(furnace.fuelItem.nickName);
            fuelItem.amount.Add(furnace.fuelItem.amount);
            fuelItem.durability.Add(furnace.fuelItem.durability);
            fuelItem.ammoInside.Add(furnace.fuelItem.ammoInside);
        }
        
        saveFurnace.smeltItem = smeltItem;
        saveFurnace.smeltedItem = smeltedItem;
        saveFurnace.fuelItem = fuelItem;
        saveFurnace.furnaceStarted = furnace.furnaceStarted;

        return saveFurnace;
    }
    private void TryLoadFurnace(Item item, GameObject placedObject, SaveFurnace loaded)
    {
        if (item.blockType == BlockTypes.Furnace)
        {
            Furnace furnace = placedObject.GetComponent<Furnace>();

            if(loaded.smeltItem.nickName.Count > 0)
            furnace.smeltItem = itemList.CreateNewItem(loaded.smeltItem.nickName[0], loaded.smeltItem.amount[0],
            loaded.smeltItem.durability[0], loaded.smeltItem.ammoInside[0]);

            if (loaded.smeltedItem.nickName.Count > 0)
                furnace.smeltedItem = itemList.CreateNewItem(loaded.smeltedItem.nickName[0], loaded.smeltedItem.amount[0],
            loaded.smeltedItem.durability[0], loaded.smeltedItem.ammoInside[0]);

            if (loaded.fuelItem.nickName.Count > 0)
                furnace.fuelItem = itemList.CreateNewItem(loaded.fuelItem.nickName[0], loaded.fuelItem.amount[0],
            loaded.fuelItem.durability[0], loaded.fuelItem.ammoInside[0]);

            if (loaded.furnaceStarted) furnace.StartStopFurnace();
        }
    }
    private SavePlant TrySavePlant(Plant plant)
    {
        if (plant == null) return null;

        SavePlant savePlant = new SavePlant();

        savePlant.isPlanted = plant.isPlanted;
        savePlant.isWatered = plant.isWatered;
        savePlant.readyForHarvest = plant.readyForHarvest;

        if(plant.plantedType != null)
        savePlant.plantName = plant.plantedType.plantName;

        savePlant.harvestTime = plant.harvestTime.ToString();
        
        return savePlant;
    }
    private void TryLoadPlant(Item item, GameObject placedObject, SavePlant loaded)
    {
        if (item.blockType == BlockTypes.FarmingPlot)
        {
            Plant plant = placedObject.GetComponent<Plant>();

            plant.LoadPlant(loaded);
        }
    }
    private SaveDoor TrySaveDoor(Door door)
    {
        if (door == null) return null;

        SaveDoor saveDoor = new SaveDoor();

        saveDoor.isOpen = door.isOpen;

        return saveDoor;
    }
    private void TryLoadDoor(Item item, GameObject placedObject, SaveDoor loaded)
    {
        if (item.blockType == BlockTypes.Door)
        {
            Door door = placedObject.GetComponent<Door>();

            door.LoadIsOpen(loaded.isOpen);
        }
    }
    private SaveCampfireCraftingOrder TrySaveCampfire(Campfire campfire)
    {
        if (campfire == null) return null;

        SaveCampfireCraftingOrder saveCraftingOrder = new SaveCampfireCraftingOrder();

        for (int i = 0; i < campfire.craftingOrderSlots.Count; i++)
        {
            CraftingOrderCampfire craftingOrderCampfire = campfire.craftingOrderSlots[i].GetComponent<CraftingOrderCampfire>();

            saveCraftingOrder.nickName.Add(craftingOrderCampfire.nickName);
            saveCraftingOrder.amount.Add(craftingOrderCampfire.amount);
            saveCraftingOrder.multiplier.Add(craftingOrderCampfire.multiplier);
            saveCraftingOrder.remainedTime.Add(craftingOrderCampfire.remainedTime);
            saveCraftingOrder.remainedTimeMax.Add(craftingOrderCampfire.remainedTimeMax);

            ListCraftStruct craftStruct = new ListCraftStruct();
            craftStruct.craftStruct.AddRange(craftingOrderCampfire.itemsInside);
            saveCraftingOrder.itemsInside.Add(craftStruct);

            ListCraftStruct craftStruct2 = new ListCraftStruct();
            craftStruct2.craftStruct.AddRange(craftingOrderCampfire.requiredMaterialForSingleItem);
            saveCraftingOrder.requiredMaterialForSingleItem.Add(craftStruct2);
        }

        SaveInventory saveInventory = new SaveInventory();

        for (int i = 0; i < campfire.items.Count; i++)
        {
            if (campfire.items[i] == null)
            {
                saveInventory.nickName.Add(null);
                saveInventory.amount.Add(0);
                saveInventory.durability.Add(0);
                saveInventory.ammoInside.Add(0);
                continue;
            }
            saveInventory.nickName.Add(campfire.items[i].nickName);
            saveInventory.amount.Add(campfire.items[i].amount);
            saveInventory.durability.Add(campfire.items[i].durability);
            saveInventory.ammoInside.Add(campfire.items[i].ammoInside);
        }

        saveCraftingOrder.items.Add(saveInventory);

        return saveCraftingOrder;
    }
    public void TryLoadCampfire(Item item, GameObject placedObject, SaveCampfireCraftingOrder loaded)
    {
        if (item.blockType != BlockTypes.Campfire) return;

        for (int i = 0; i < loaded.nickName.Count; i++)
        {
            Campfire campfire = placedObject.GetComponent<Campfire>();

            campfire.LoadCraftingOrder(loaded.itemsInside[i].craftStruct, loaded.requiredMaterialForSingleItem[i].craftStruct,
                loaded.nickName[i], loaded.amount[i], loaded.multiplier[i], loaded.remainedTime[i], loaded.remainedTimeMax[i]);

            campfire.LoadItems(loaded.items[i]);
        }
    }
}

[System.Serializable]
public class SaveBlock
{
    public List<Vector3> position = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();
    public List<string> nickName = new List<string>();
    public List<float> durability = new List<float>();

    public List<SaveInventory> chests = new List<SaveInventory>();
    public List<SaveFurnace> furnaces = new List<SaveFurnace>();
    public List<SavePlant> plants = new List<SavePlant>();
    public List<SaveDoor> doors = new List<SaveDoor>();
    public List<SaveCampfireCraftingOrder> campfires = new List<SaveCampfireCraftingOrder>();
}

[System.Serializable]
public class SaveFurnace
{
    public SaveInventory smeltItem;
    public SaveInventory smeltedItem;
    public SaveInventory fuelItem;
    public bool furnaceStarted;
}

[System.Serializable]
public class SavePlant
{
    public bool isPlanted;
    public bool isWatered;
    public bool readyForHarvest;
    public string plantName;
    public string harvestTime;
}

[System.Serializable]
public class SaveDoor
{
    public bool isOpen;
}

[System.Serializable]
public class SaveCampfireCraftingOrder
{
    public List<string> nickName = new List<string>();
    public List<int> amount = new List<int>();
    public List<int> multiplier = new List<int>();
    public List<int> remainedTime = new List<int>();
    public List<int> remainedTimeMax = new List<int>();
    public List<ListCraftStruct> itemsInside = new List<ListCraftStruct>();
    public List<ListCraftStruct> requiredMaterialForSingleItem = new List<ListCraftStruct>();

    public List<SaveInventory> items = new List<SaveInventory>();
}