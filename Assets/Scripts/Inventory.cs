using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private BackpackCanvas backpackCanvas;
    private CampfireCanvas campfireCanvas;
    private ChestCanvas chestCanvas;
    private CraftingCanvas craftingCanvas;
    private FpsCounter fpsCounter;
    private FurnaceCanvas furnaceCanvas;
    private InventoryChange inventoryChange;
    private ItemList itemList;
    private LockMovement lockMovement;
    private LootCanvas lootCanvas;
    private Minimap minimap;
    private PlayerItems playerItems;
    private SaveManager saveManager;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotParent;
    [SerializeField] private GameObject beltSlotParent;

    [SerializeField] private GameObject inventoryPanel;

    [SerializeField] private GameObject itemDescriptionPanel;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemDescriptionHeaderText;

    [SerializeField] private GameObject craftingPanel;
    [SerializeField] private GameObject craftingDescriptionPanel;
    [SerializeField] private GameObject craftingOrderPanel;


    [HideInInspector] public List<GameObject> slots = new List<GameObject>();
    private int inventorySlotCount = 50;
    public int beltSlotCount { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openInventorySound;
    [SerializeField] private AudioClip closeInventorySound;
    [SerializeField] private AudioClip addItemSound;
    [SerializeField] private AudioClip itemBreakingSound;

    [HideInInspector] public Slot draggingItem;

    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private Transform itemDropPoint;

    void Awake()
    {
        backpackCanvas = GetComponent<BackpackCanvas>();
        campfireCanvas = GetComponent<CampfireCanvas>();
        chestCanvas = GetComponent<ChestCanvas>();
        craftingCanvas = GetComponent<CraftingCanvas>();
        fpsCounter = GetComponent<FpsCounter>();
        furnaceCanvas = GetComponent<FurnaceCanvas>();
        inventoryChange = GetComponent<InventoryChange>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
        lockMovement = GetComponent<LockMovement>();
        lootCanvas = GetComponent<LootCanvas>();
        minimap = GetComponent<Minimap>();
        playerItems = GetComponent<PlayerItems>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();

        beltSlotCount = 7;
        CreateSlots();
    }
    void Start()
    {
        playerItems.FindPlayerItems();
    }
    void Update()
    {
        if (lockMovement.playerInCar) return;
        if (lockMovement.zoomOn)
        {
            ActivateBeltSlot(false);
            return;
        }

        ActivateBeltSlot(true);
        CheckInputs();
    }
    private void CreateSlots()
    {
        for (int i = 0; i < beltSlotCount; i++)
        {
            GameObject myObject = Instantiate(slotPrefab, beltSlotParent.transform);

            Slot slotCode = myObject.GetComponent<Slot>();
            slotCode.SetParameters(this, itemList, null);
            slots.Add(myObject);
        }

        for (int i = 0; i < inventorySlotCount; i++)
        {
            GameObject myObject = Instantiate(slotPrefab, slotParent.transform);

            Slot slotCode = myObject.GetComponent<Slot>();
            slotCode.SetParameters(this, itemList, null);
            slots.Add(myObject);
        }
    }
    public void GiveStartingItems()
    {
        AddItem("axe", 1, false);
        AddItem("pickaxe", 1, false);
        AddItem("spear", 1, false);
        AddItem("sword", 1, false);
        AddItem("baseball_bat", 1, false);
        AddItem("wood_spike", 20, false);
        AddItem("bandage", 10, false);
        AddItem("first_aid_kit", 10, false);
        AddItem("wood_frame", 100, false);
        AddItem("gasoline", 500, false);
        AddItem("empty_bottle", 10, false);
        AddItem("bottled_water", 10, false);
        AddItem("ladder", 10, false);
        AddItem("furnace", 2, false);
        AddItem("campfire", 2, false);
        AddItem("wood_stair", 100, false);
        AddItem("hammer", 1, false);
        AddItem("stone", 5000, false);
        AddItem("wood", 5000, false);
        AddItem("forged_iron", 5000, false);
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!lockMovement.locked)
            {
                OpenInventory(true, true);
            }
            else if (lockMovement.locked && inventoryPanel.activeInHierarchy)
            {
                CloseInventory();
            }

        }
    }
    public void OpenInventory(bool withCraftingPanel, bool withCraftingOrderPanel)
    {
        if (inventoryPanel.activeInHierarchy || lockMovement.locked || lockMovement.playerBusy || lockMovement.inventoryLocked) return;

        PlaySound(openInventorySound);
        ResetSlotsColor();

        inventoryPanel.SetActive(true);
        SetItemDescriptionText(string.Empty);

        campfireCanvas.CraftingPanelParentSetActive(false);
        craftingCanvas.CraftingPanelParentSetActive(true);

        campfireCanvas.ClearSelectedItem();
        craftingCanvas.ClearSelectedItem();
  

        if (withCraftingPanel)
        {
            craftingPanel.SetActive(true);
        } 
        if(withCraftingOrderPanel)
        {
            craftingOrderPanel.SetActive(true);
        }

        craftingCanvas.ChangePage(1);

        minimap.Close();
        fpsCounter.AddReason("inventory");
        inventoryChange.ClosePanel();
        lockMovement.Lock();
    }
    public void CloseInventory()
    {
        if (!inventoryPanel.activeInHierarchy || !lockMovement.locked ||
            lockMovement.playerBusy || lockMovement.inventoryLocked || draggingItem.item != null) return;

        PlaySound(closeInventorySound);
        ResetSlotsColor();

        inventoryPanel.SetActive(false);
        itemDescriptionPanel.SetActive(false);

        craftingOrderPanel.SetActive(false);
        craftingPanel.SetActive(false);
        minimap.Open();
        fpsCounter.RemoveReason("inventory");
        inventoryChange.OpenPanel();
        lockMovement.Unlock();
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void AddItem(int id, int amount, bool sound)
    {
        if (sound) PlaySound(addItemSound);

        int remainedAmount = amount;

        for(int i = 0; i < slots.Count; i++)
        {
            Slot slotCode = slots[i].GetComponent<Slot>();

            if (slotCode.item == null) continue;

            if (slotCode.item.id == id && slotCode.item.amount < slotCode.item.maxAmount)
            {
                if(slotCode.item.amount + remainedAmount > slotCode.item.maxAmount)
                {
                    int addedAmount = slotCode.item.maxAmount - slotCode.item.amount;
                    remainedAmount -= addedAmount;
                    slotCode.item.IncreaseAmount(addedAmount);
                    slotCode.UpdateSlot();
                }
                else
                {
                    slotCode.item.IncreaseAmount(remainedAmount);
                    remainedAmount = 0;
                    slotCode.UpdateSlot();
                    break;
                }
            }
        }

        if(remainedAmount > 0)
        for (int i = 0; i < slots.Count; i++) // boþ olan slota ekle
        {
            Slot slotCode = slots[i].GetComponent<Slot>();

            if (slotCode.item != null) continue;

            if (remainedAmount > itemList.items[id].maxAmount)
            {
                remainedAmount -= itemList.items[id].maxAmount;
                slotCode.SetItem(itemList.CreateNewItem(id, itemList.items[id].maxAmount));
            }
            else
            {
                slotCode.SetItem(itemList.CreateNewItem(id, remainedAmount));
                break;
            }
        }

        if(sound)
        {
            Item item = itemList.CreateNewItem(id, amount);
            inventoryChange.AddChangeItem(item.image, amount, CalculateItemCount(item.nickName));
        }  
    }
    public void AddItem(Item item, bool sound)
    {
        if(item == null) return;

        if(item.maxAmount > 1)
        {
            AddItem(item.id, item.amount, sound);
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            Slot slotCode = slots[i].GetComponent<Slot>();

            if (slotCode.item != null) continue;

            slotCode.SetItem(item);
            inventoryChange.AddChangeItem(item.image, 1, CalculateItemCount(item.nickName));
            PlaySound(addItemSound);
            return;
        }
    }
    public void AddItem(string nickName, int amount, bool sound)
    {
        Item item = itemList.items.Find(x => x.nickName == nickName);
        if (item != null)
        {
            AddItem(item.id, amount, sound);
        }
    }
    public int CheckItemCount(int id)
    {
        int amount = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            Slot slotCode = slots[i].GetComponent<Slot>();

            if (slotCode.item == null) continue;

            if (slotCode.item.id == id)
            {
                amount += slotCode.item.amount;
            }
        }

        return amount;
    }
    public void DropItem(Item item)
    {
        GameObject myObject = Instantiate(droppedItemPrefab, itemDropPoint.position, Quaternion.identity);
        DroppedItem droppedItem = myObject.GetComponent<DroppedItem>();
        droppedItem.item = item;
    }
    public void GetItem(GameObject item)
    {
        DroppedItem droppedItem = item.GetComponent<DroppedItem>();
        AddItem(droppedItem.item, true);
        Destroy(item);
    }
    public void ResetSlotsColor()
    {
        lootCanvas.ResetSlotsColor();
        backpackCanvas.ResetSlotsColor();
        chestCanvas.ResetSlotsColor();
        campfireCanvas.ResetSlotsColor();
        furnaceCanvas.ResetSlotsColor();

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Slot>().ResetSlotColor();
        }
    }
    public void SetItemDescriptionText(string text)
    {
        OpenItemDescriptionPanel();
        itemDescriptionText.text = text;
    }
    public void ShowDraggingItem(bool value)
    {
        if (value && draggingItem.gameObject.activeInHierarchy) return;
        if (!value && !draggingItem.gameObject.activeInHierarchy) return;

        draggingItem.gameObject.SetActive(value);
    }
    public void ChangeDraggingItemPosition(Vector3 newPosition)
    {
        draggingItem.gameObject.transform.position = newPosition;
    }
    public void OpenSelectedSlotImage(int slotNumber)
    {
        CloseAllSelectedSlotImage();
        slots[slotNumber].GetComponent<Slot>().ActivateSelectedSlot();
    }
    private void CloseAllSelectedSlotImage()
    {
        for (int i = 0; i < beltSlotCount; i++)
        {
            slots[i].GetComponent<Slot>().DeactivateSelectedSlot();
        }
    }
    public void UpdateSelectedSlot()
    {
        slots[playerItems.selectedSlotNo].GetComponent<Slot>().UpdateSlot();
    }
    public int CalculateItemCount(string nickName)
    {
        var result = itemList.items.Find(x => x.nickName == nickName);
        if (result == null) return 0;

        int totalCount = 0;

        for(int i = 0; i < slots.Count; i++)
        {
            Item item = slots[i].GetComponent<Slot>().item;
            if (item == null) continue;

            if (item.nickName == nickName)
                totalCount += item.amount;
        }
        return totalCount;
    }
    public void DecreaseItem(string nickName, int amount)
    {
        var result = itemList.items.Find(x => x.nickName == nickName);
        if (result == null) return;

        int remainedAmount = amount;

        for (int i = 0; i < slots.Count; i++)
        {
            if (remainedAmount <= 0) break;

            Slot slot = slots[i].GetComponent<Slot>();
            Item item = slot.item;

            if (item == null) continue;

            if (item.nickName == nickName)
            {
                if(item.amount >= remainedAmount)
                {
                    item.DecreaseAmount(remainedAmount);
                    remainedAmount = 0;
                }
                else
                {
                    remainedAmount = remainedAmount - item.amount;
                    item.DecreaseAmount(item.amount);
                }
                slot.UpdateSlot();
            }
        }

        Item myItem = itemList.CreateNewItem(nickName, amount);
    }
    public void OpenCraftingDescriptionPanel()
    {
        if (craftingDescriptionPanel.activeInHierarchy) return;
        itemDescriptionPanel.SetActive(false);
        craftingDescriptionPanel.SetActive(true);     
    }
    public void OpenItemDescriptionPanel()
    {
        craftingDescriptionPanel.SetActive(false);
        itemDescriptionPanel.SetActive(true);
    }
    public int CalculateCraftingAmount(List<CraftStruct> requiredItems)
    {
        int amount = -1;

        for (int i = 0; i < requiredItems.Count; i++) 
        {
            int currentAmount = CalculateItemCount(requiredItems[i].nickName) / requiredItems[i].amount;

            if (amount == -1 || amount > currentAmount)
            {
                amount = currentAmount;
            }
        }
        return amount;
    }
    private void ActivateBeltSlot(bool value)
    {
        if(value)
        {
            if (beltSlotParent.activeInHierarchy) return;
            beltSlotParent.SetActive(true);
        }
        else
        {
            if (!beltSlotParent.activeInHierarchy) return;
            beltSlotParent.SetActive(false);
        }
    }
    public void PlayItemBreakingSound()
    {
        audioSource.PlayOneShot(itemBreakingSound);
    }
    public void FastItemTransfer()
    {

    }
    public void SaveInventory()
    {
        SaveInventory saveInventory = new SaveInventory();

        for(int i = 0; i < slots.Count; i++)
        {
            Item item = slots[i].GetComponent<Slot>().item;
            if(item == null)
            {
                saveInventory.nickName.Add(null);
                saveInventory.amount.Add(0);
                saveInventory.durability.Add(0);
                saveInventory.ammoInside.Add(0);
                continue;
            }

            saveInventory.nickName.Add(item.nickName);
            saveInventory.amount.Add(item.amount);
            saveInventory.durability.Add(item.durability);
            saveInventory.ammoInside.Add(item.ammoInside);
        }

        StartCoroutine(saveManager.Save(saveInventory, SaveType.Inventory));
    }
    public void LoadInventory()
    {
        SaveInventory loadedInventory = JsonUtility.FromJson<SaveInventory>(saveManager.Load(SaveType.Inventory));
        if(loadedInventory == null)
        {
            GiveStartingItems();
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();
            Item item = itemList.CreateNewItem(loadedInventory.nickName[i], loadedInventory.amount[i],
                loadedInventory.durability[i], loadedInventory.ammoInside[i]);
            if (item == null) continue;

            slot.SetItem(item);
        }
    }
}

[System.Serializable]
public class SaveInventory
{
    public List<string> nickName = new List<string>();
    public List<int> amount = new List<int>();
    public List<int> durability = new List<int>();
    public List<int> ammoInside = new List<int>();
}
