using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    private BackpackCanvas backpackCanvas;
    private Building building;
    private Camera cam;
    private CampfireCanvas campfireCanvas;
    private ChestCanvas chestCanvas;
    private FurnaceCanvas furnaceCanvas;
    private Inventory inventory;
    private LockMovement lockMovement;
    private LootCanvas lootCanvas;
    private Player player;
    private PlayerItems playerItems;
    private SpawnManager spawnManager;
    private Translations translations;

    private RaycastHit hit;
    [SerializeField] private GameObject raycastPanel;
    [SerializeField] private Text raycastText;

    private string lastTag;

    void Awake()
    {
        backpackCanvas = GetComponent<BackpackCanvas>();
        building = GetComponent<Building>();
        cam = GetComponentInChildren<Camera>();
        campfireCanvas = GetComponent<CampfireCanvas>();
        chestCanvas = GetComponent<ChestCanvas>();
        furnaceCanvas = GetComponent<FurnaceCanvas>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        lootCanvas = GetComponent<LootCanvas>();
        player = GetComponent<Player>();
        playerItems = GetComponent<PlayerItems>();
        spawnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>();
        translations = GetComponent<Translations>();
    }
    void Update()
    {
        if (lockMovement.playerInCar || lockMovement.locked || lockMovement.zoomOn)
        {
            CloseRaycastPanel();
            return;
        }

        CheckDroppedItems();
        CheckCollectableItems();
        CheckLoots();
        CheckDoors();
        CheckBuildingDurabilities();
        CheckBackpacks();
        CheckChests();
        CheckCampfires();
        CheckFurnaces();
        CheckCars();
        CheckFarmingPlots();
        CheckFrames();
        CheckLake();
    }
    public RaycastHit? CreateRaycast(float distance)
    {
        if (lockMovement.locked) return null;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            return hit;
        }
        else CloseRaycastPanel();

        return null;
    }
    public RaycastHit? CreateRaycast(float distance, Vector3 difference)
    {
        if (lockMovement.locked) return null;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + difference, out hit, distance))
        {
            return hit;
        }
        else CloseRaycastPanel();

        return null;
    }
    private void WriteToRaycastPanel(string text)
    {
        if (raycastText.text == text) return;

        raycastText.text = text;
        raycastPanel.SetActive(true);
    }
    private void CloseRaycastPanel()
    {
        if (!raycastPanel.activeInHierarchy) return;

        raycastText.text = string.Empty;
        raycastPanel.SetActive(false);
    }
    public bool CheckTag(RaycastHit? raycastHit, string wantedTag, bool rootObject)
    {
        if (raycastHit == null ||
            (rootObject && !raycastHit.Value.transform.root.CompareTag(wantedTag)) ||
            (!rootObject && !raycastHit.Value.transform.CompareTag(wantedTag)))
        {
            if (lastTag == wantedTag)
            {
                CloseRaycastPanel();
                lastTag = string.Empty;
            }
            return false;
        }

        if (rootObject)
            lastTag = raycastHit.Value.transform.root.tag;
        else
            lastTag = raycastHit.Value.transform.tag;
        return true;
    }
    private void CheckDroppedItems()
    {
        if (!CheckTag(CreateRaycast(4f), "DroppedItem", false)) return;

        DroppedItem droppedItem = hit.transform.GetComponent<DroppedItem>();
        WriteToRaycastPanel(droppedItem.item.name + " X" + droppedItem.item.amount);

        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.GetItem(hit.transform.gameObject);
        }
    }
    private void CheckCollectableItems()
    {
        if (!CheckTag(CreateRaycast(4f), "CollectableItem", true)) return;

        CollectableItem collectableItem = hit.transform.root.GetComponent<CollectableItem>();
        WriteToRaycastPanel(translations.GetItemName(collectableItem.nickName) + " X" + collectableItem.amount);

        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(collectableItem.nickName, collectableItem.amount, true);
            spawnManager.RemoveCollectable(hit.transform.root.gameObject);
            Destroy(hit.transform.root.gameObject);
        }
    }
    private void CheckLoots()
    {
        if (!CheckTag(CreateRaycast(4f), "Loot", false)) return;

        WriteToRaycastPanel(translations.Get("OpenLootDialog"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            lootCanvas.OpenPanel(hit.transform.gameObject);
        }
    }
    private void CheckDoors()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", false)) return;

        Door door = hit.transform.GetComponent<Door>();
        if (door == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            door.OpenOrCloseDoor();
        }
    }
    private void CheckBuildingDurabilities()
    {
        if (playerItems.selectedItem == null || playerItems.selectedItem.nickName != "hammer") return;
        if (!CheckTag(CreateRaycast(4f), "Building", true)) return;

        Block block = hit.transform.root.GetComponent<Block>();
        if (block == null) return;

        WriteToRaycastPanel(translations.Get("Durability") + block.durability + "HP");
    }
    private void CheckBackpacks()
    {
        if (!CheckTag(CreateRaycast(4f), "Backpack", false)) return;

        Backpack backpack = hit.transform.GetComponent<Backpack>();
        if (backpack == null) return;

        WriteToRaycastPanel(translations.Get("OpenBackpackDialog"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            backpackCanvas.Open(hit.transform.gameObject);
        }
    }
    private void CheckChests()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", false)) return;

        Chest chest = hit.transform.GetComponent<Chest>();
        if (chest == null) return;

        WriteToRaycastPanel(translations.Get("OpenChestDialog"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            chestCanvas.Open(hit.transform.gameObject);
        }
    }
    private void CheckCampfires()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", true)) return;

        Campfire campfire = hit.transform.root.GetComponent<Campfire>();
        if (campfire == null) return;

        WriteToRaycastPanel(translations.Get("OpenCampfireDialog"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            campfireCanvas.Open(hit.transform.root.gameObject);
        }
    }
    private void CheckFurnaces()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", true)) return;

        Furnace furnace = hit.transform.root.GetComponent<Furnace>();
        if (furnace == null) return;

        WriteToRaycastPanel(translations.Get("OpenFurnaceDialog"));

        if (Input.GetKeyDown(KeyCode.E))
        {
            furnaceCanvas.Open(hit.transform.root.gameObject);
        }
    }
    private void CheckCars()
    {
        if (!CheckTag(CreateRaycast(4f), "Car", true)) return;

        CarController carController = hit.transform.root.GetComponent<CarController>();
        if (carController == null) return;

        if (playerItems.selectedItem != null && playerItems.selectedItem.nickName == "gasoline")
        {
            int selectedGasolineAmount = playerItems.selectedItem.amount;
            int requiredAmount = carController.CalculateRequiredFuel();
            int addAmount;

            if (selectedGasolineAmount >= 100)
            {
                if (requiredAmount >= 100) addAmount = 100;
                else addAmount = requiredAmount;
            }
            else
            {
                if (requiredAmount <= selectedGasolineAmount) addAmount = requiredAmount;
                else addAmount = selectedGasolineAmount;
            }

            if (addAmount > 0)
                WriteToRaycastPanel(translations.Get("Refill") + " " + (float)addAmount / 10 + translations.Get("FuelAndCost") + 
                    addAmount + " " + translations.Get("Gasoline"));
            else
                WriteToRaycastPanel(translations.Get("GasTankFull"));

            if (Input.GetKeyDown(KeyCode.E) && addAmount > 0)
            {
                playerItems.selectedItem.DecreaseAmount(addAmount);
                carController.FillFuel(addAmount / 10);
                inventory.UpdateSelectedSlot();
            }
        }
        else
        {
            WriteToRaycastPanel(translations.Get("Car"));

            if (Input.GetKeyDown(KeyCode.E) && !lockMovement.locked)
            {
                player.DriveCar(hit.transform.root.gameObject);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                chestCanvas.Open(hit.transform.root.gameObject);
            }
        }

    }
    private void CheckFarmingPlots()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", false)) return;

        Plant plant = hit.transform.GetComponent<Plant>();
        if (plant == null) return;

        if(plant.isPlanted)
        {
            if(!plant.readyForHarvest)
            {
                if(!plant.isWatered)
                {
                    WriteToRaycastPanel(translations.Get("PlantNeedWater"));

                    if (playerItems.selectedItem != null && playerItems.selectedItem.nickName == "bottled_water")
                    {
                        WriteToRaycastPanel(translations.Get("PlantWaterDialog"));

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            CloseRaycastPanel();
                            plant.WaterSeed();
                            playerItems.selectedItem.DecreaseAmount(1);
                            inventory.UpdateSelectedSlot();
                        }
                    }
                }
                else
                {
                    WriteToRaycastPanel(translations.Get("PlantGrowingDialog"));
                }
            }
            else
            {
                WriteToRaycastPanel(translations.Get("HarvestDialog"));

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CloseRaycastPanel();
                    inventory.AddItem(plant.plantedType.harvestItem.nickName, plant.plantedType.harvestItem.amount, true);
                    plant.ResetVariables();
                }
            }
        }
        else
        {
            if(playerItems.selectedItem != null && playerItems.selectedItem.category == Categories.Seed)
            {
                WriteToRaycastPanel(translations.Get("PlantDialog"));

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CloseRaycastPanel();
                    plant.PlantSeed(playerItems.selectedItem.nickName);
                    playerItems.selectedItem.DecreaseAmount(1);
                    inventory.UpdateSelectedSlot();
                }
            }
        }
    }
    private void CheckFrames()
    {
        if (!CheckTag(CreateRaycast(4f), "Building", true)) return;

        Block block = hit.transform.root.GetComponent<Block>();
        if (block == null) return;

        if(block.nickName == "wood_frame" && block.materialType == MaterialType.Frame)
        {
            WriteToRaycastPanel(translations.Get("GetWoodFrame"));

            if(Input.GetKeyDown(KeyCode.E))
            {
                building.RemoveBuilding(hit.transform.root.gameObject);
            }
        }
    }
    private void CheckLake()
    {
        if (!CheckTag(CreateRaycast(4f), "Water", false)) return;

        if (playerItems.selectedItem != null && playerItems.selectedItem.nickName == "empty_bottle")
        {
            WriteToRaycastPanel(translations.Get("FillWater"));

            if (Input.GetKeyDown(KeyCode.E))
            {
                int amount = playerItems.selectedItem.amount;
                playerItems.selectedItem.DecreaseAmount(amount);
                inventory.UpdateSelectedSlot();
                inventory.AddItem("dirty_water", amount, true);
            }
        }
    }
}
