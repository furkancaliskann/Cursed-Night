using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireCanvas : MonoBehaviour
{
    private Campfire campfire;
    private CraftingCanvas craftingCanvas;
    private Inventory inventory;
    private ItemList itemList;
    private LockMovement lockMovement;

    private float openCloseCounter = 0.2f;
    private float openCloseCounterMax = 0.2f;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    private List<Slot> slots = new List<Slot>();

    [SerializeField] private GameObject campfirePanel;

    private List<CraftableItem> craftableItems = new List<CraftableItem>();

    [Header("Page (Crafting Panel)")]
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private Text pageNumberText;
    [SerializeField] private InputField searchItemInputfield;
    private List<GameObject> pages = new List<GameObject>();
    private int selectedPageNumber;

    [Header("Craftable Object (Crafting Panel)")]
    [SerializeField] private GameObject craftableObjectPrefab;
    [SerializeField] private Transform craftingPanelParent;
    private CraftableItem selectedItem;

    [Header("Description (Inventory)")]
    [SerializeField] private List<GameObject> requiredMaterialSlots = new List<GameObject>();
    [SerializeField] private Image descriptionImage;
    [SerializeField] private Text descriptionMultiplierText;
    [SerializeField] private Text descriptionTimeText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private InputField craftingAmountInputfield;
    [SerializeField] private GameObject craftButton;
    private int craftingAmount = 0;

    [Header("Crafting Order (Crafting Panel)")]
    [SerializeField] private Transform craftingOrderPanel;
    [SerializeField] private GameObject craftingOrderCampfireCanvasPrefab;
    private List<GameObject> craftingOrderSlots = new List<GameObject>();

    private AudioSource audioSource;
    [SerializeField] private AudioClip craftingSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        craftingCanvas = GetComponent<CraftingCanvas>();
        inventory = GetComponent<Inventory>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
        lockMovement = GetComponent<LockMovement>();
    }
    void Start()
    {
        CreateSlots();

        CreateCraftableItem("mushroom_onion_rings", 40, 1, CreateStruct("mushroom", 1, "onion", 2, "wood", 20));
        CreateCraftableItem("boiled_tomato_salad", 20, 1, CreateStruct("tomato", 2, "cucumber", 1, "onion", 1, "wood", 5));
        CreateCraftableItem("potato_balls", 35, 1, CreateStruct("potato", 2, "wood", 15));
        CreateCraftableItem("pancake", 35, 1, CreateStruct("flour", 1, "egg", 1, "milk", 1, "wood", 20));
        CreateCraftableItem("grilled_meat", 15, 1, CreateStruct("raw_meat", 1, "wood", 20));
        CreateCraftableItem("meat_skewer", 60, 1, CreateStruct("raw_meat", 2, "onion", 1, "pepper", 1, "wood", 20));
        CreateCraftableItem("corn_bread", 25, 1, CreateStruct("corn", 2, "bottled_water", 1, "wood", 20));
        CreateCraftableItem("tomato_soup", 30, 1, CreateStruct("tomato", 2, "pepper", 1, "bottled_water", 2, "wood", 20));
        CreateCraftableItem("pasta_with_sauce", 45, 1, CreateStruct("flour", 1, "tomato", 1, "pepper", 1, "bottled_water", 1, "wood", 20));
        CreateCraftableItem("pizza", 60, 1, CreateStruct("mushroom", 1, "corn", 1, "flour", 1, "wood", 20));
        CreateCraftableItem("coffee", 20, 1, CreateStruct("coffee_bean", 1, "bottled_water", 1, "wood", 10));
        CreateCraftableItem("rosehip_tea", 25, 1, CreateStruct("rosehip", 2, "bottled_water", 1, "wood", 10));
        CreateCraftableItem("potato_chips", 25, 1, CreateStruct("potato", 2, "salt", 1, "wood", 10));
        CreateCraftableItem("corn_chips", 25, 1, CreateStruct("corn", 2, "salt", 1, "wood", 10));
        CreateCraftableItem("meat_stew", 80, 1, CreateStruct("raw_meat", 2, "potato", 2, "mushroom", 2, "salt", 1, "wood", 50));
        CreateCraftableItem("bottled_water", 15, 1, CreateStruct("dirty_water", 1, "wood", 20));

        PlaceItems();
        UpdateCraftingAmountText();
    }
    void Update()
    {
        CheckInputs();
        CheckCounter();
        CheckSlots();
    }
    private void CreateSlots()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject myObject = Instantiate(slotPrefab, slotParent);
            Slot slotCode = myObject.GetComponent<Slot>();

            slotCode.SetParameters(inventory, itemList, null);
            slots.Add(slotCode);
        }
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && !searchItemInputfield.isFocused)
        {
            Close();
        }
    }
    private void CheckCounter()
    {
        if (openCloseCounter > 0) openCloseCounter -= Time.deltaTime;
    }
    private void CheckSlots()
    {
        if (campfire == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (campfire.items[i] != slots[i].item)
            {
                campfire.items[i] = slots[i].item;
            }
        }
    }
    private void ClearSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetItem(null);
            slots[i].UpdateSlot();
        }
    }
    public void PlaceItems(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (slots[i].item != items[i])
            {
                slots[i].SetItem(items[i]);
                slots[i].UpdateSlot();
            }
        }
    }
    public void ResetSlotsColor()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Slot>().ResetSlotColor();
        }
    }
    public void Open(GameObject campfireObject)
    {
        if (openCloseCounter > 0 || lockMovement.locked || lockMovement.playerBusy || campfirePanel.activeInHierarchy) return;

        openCloseCounter = openCloseCounterMax;

        Campfire campfireCode = campfireObject.GetComponent<Campfire>();
        PlaceItems(campfireCode.items);
        campfire = campfireCode;
        campfireCode.SetCampfireCanvas(this);

        RefreshCraftingOrderCampfireCanvas();

        inventory.OpenInventory(true, false);

        craftingCanvas.CraftingPanelParentSetActive(false);
        CraftingPanelParentSetActive(true);
        ClearSelectedItem();
        ChangePage(1);
        craftingCanvas.ClearSelectedItem();

        for (int i = 0; i < campfireCode.craftingOrderSlots.Count; i++)
        {
            Instantiate(campfireCode.craftingOrderSlots[i], craftingOrderPanel);
        }

        campfirePanel.SetActive(true);
        lockMovement.LockInventory();
    }
    private void Close()
    {
        if (openCloseCounter > 0 || !lockMovement.locked || lockMovement.playerBusy || !campfirePanel.activeInHierarchy) return;

        openCloseCounter = openCloseCounterMax;
        campfire.SetCampfireCanvas(null);
        ClearSlots();
        campfire = null;

        for (int i = 0; i < craftingOrderSlots.Count; i++)
        {
            Destroy(craftingOrderSlots[i]);
            craftingOrderSlots[i] = null;
        }

        lockMovement.UnlockInventory();
        inventory.CloseInventory();
        campfirePanel.SetActive(false);
    }

    private List<CraftStruct> CreateStruct(string nickname1, int amount1, string nickname2, int amount2)
    {
        List<CraftStruct> craftStruct = new List<CraftStruct>
        {
            { new CraftStruct { nickName = nickname1, amount = amount1 } },
            { new CraftStruct {nickName = nickname2, amount = amount2 } },
        };

        return craftStruct;
    }
    private List<CraftStruct> CreateStruct(string nickname1, int amount1, string nickname2, int amount2, string nickname3, int amount3)
    {
        List<CraftStruct> craftStruct = new List<CraftStruct>
        {
            { new CraftStruct { nickName = nickname1, amount = amount1 } },
            { new CraftStruct { nickName = nickname2, amount = amount2 } },
            { new CraftStruct { nickName = nickname3, amount = amount3 } },
        };

        return craftStruct;
    }
    private List<CraftStruct> CreateStruct(string nickname1, int amount1, string nickname2, int amount2,
        string nickname3, int amount3, string nickname4, int amount4)
    {
        List<CraftStruct> craftStruct = new List<CraftStruct>
        {
            { new CraftStruct { nickName = nickname1, amount = amount1 } },
            { new CraftStruct { nickName = nickname2, amount = amount2 } },
            { new CraftStruct { nickName = nickname3, amount = amount3 } },
            { new CraftStruct { nickName = nickname4, amount = amount4 } },
        };

        return craftStruct;
    }
    private List<CraftStruct> CreateStruct(string nickname1, int amount1, string nickname2, int amount2,
        string nickname3, int amount3, string nickname4, int amount4, string nickname5, int amount5)
    {
        List<CraftStruct> craftStruct = new List<CraftStruct>
        {
            { new CraftStruct { nickName = nickname1, amount = amount1 } },
            { new CraftStruct { nickName = nickname2, amount = amount2 } },
            { new CraftStruct { nickName = nickname3, amount = amount3 } },
            { new CraftStruct { nickName = nickname4, amount = amount4 } },
            { new CraftStruct { nickName = nickname5, amount = amount5 } },
        };

        return craftStruct;
    }
    private void CreateCraftableItem(string nickName, int craftingTime, int multiplier, List<CraftStruct> requiredItems)
    {
        Item item = itemList.GetItem(nickName);

        craftableItems.Add(new CraftableItem
        {
            nickName = nickName,
            name = item.name,
            description = item.description,
            craftingTime = craftingTime,
            multiplier = multiplier,
            requiredItems = requiredItems,
            image = item.image
        });
    }
    public void CraftingPanelParentSetActive(bool value)
    {
        craftingPanelParent.gameObject.SetActive(value);
    }
    public void ClearSelectedItem()
    {
        ClearSearchInput();
        selectedItem = new CraftableItem();
    }
    private void PlaceItems()
    {
        craftableItems.Sort((a, b) => a.name.CompareTo(b.name));

        int buttonNumber = 0;

        for (int i = 0; i < craftableItems.Count; i++)
        {
            buttonNumber++;

            if (pages.Count == 0 || buttonNumber == 7)
            {
                GameObject page = Instantiate(pagePrefab, craftingPanelParent);
                pages.Add(page);
                buttonNumber = 0;
            }

            GameObject myObject = Instantiate(craftableObjectPrefab, pages[pages.Count - 1].transform);

            myObject.GetComponentInChildren<Text>().text = craftableItems[i].name;
            myObject.GetComponentsInChildren<Image>()[1].sprite = craftableItems[i].image;

            string referanceID = craftableItems[i].nickName;
            myObject.GetComponent<Button>().onClick.AddListener(delegate { UpdateDescription(referanceID); });

        }

        ChangePage(1);
    }
    public void SearchItem()
    {
        if (searchItemInputfield.text == "" && campfire == null) return;

        var result = craftableItems.FindIndex(x => x.name.StartsWith(searchItemInputfield.text, StringComparison.OrdinalIgnoreCase));
        int page = result / 7;
        if (page * 7 <= result) page++;

        ChangePage(page);
    }
    public void ClearSearchInput()
    {
        searchItemInputfield.text = string.Empty;
    }
    public void PreviousPage()
    {
        ClearSearchInput();
        ChangePage(selectedPageNumber - 1);
    }
    public void NextPage()
    {
        ClearSearchInput();
        ChangePage(selectedPageNumber + 1);
    }
    private void ChangePage(int pageToOpen)
    {
        if (campfire == null) return;
        if (pages.Count <= pageToOpen - 1 || pageToOpen <= 0) return;

        selectedPageNumber = pageToOpen;
        pageToOpen = pageToOpen - 1;


        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(false);
        }

        pages[pageToOpen].SetActive(true);
        pageNumberText.text = selectedPageNumber.ToString();

    }
    public void UpdateDescription(string nickName)
    {
        inventory.OpenCraftingDescriptionPanel();

        for (int i = 0; i < requiredMaterialSlots.Count; i++)
        {
            requiredMaterialSlots[i].GetComponent<RequiredMaterialSlot>().ClearSlot();
        }

        CraftableItem item = new CraftableItem();
        item = craftableItems.Find(x => x.nickName == nickName);

        selectedItem = item;

        for (int i = 0; i < item.requiredItems.Count; i++)
        {
            RequiredMaterialSlot requiredMaterialSlot = requiredMaterialSlots[i].GetComponent<RequiredMaterialSlot>();

            var requiredItem = itemList.GetItem(item.requiredItems[i].nickName);

            requiredMaterialSlot.SetSlot(requiredItem.image, requiredItem.name, item.requiredItems[i].amount, inventory.CalculateItemCount(requiredItem.nickName));
        }

        descriptionText.text = "-  " + item.name.ToUpper() + "  -" + "\n\n" + item.description;
        descriptionImage.sprite = item.image;
        descriptionMultiplierText.text = "x" + item.multiplier.ToString();

        int minute = item.craftingTime / 60;
        int second = item.craftingTime % 60;

        descriptionTimeText.text = string.Format("{0:00}:{1:00}", minute, second);

        int amount = inventory.CalculateCraftingAmount(item.requiredItems);
        if (amount > 0) craftingAmount = 1;
        else craftingAmount = 0;
        UpdateCraftingAmountText();
    }
    public void DecreaseCraftingAmount()
    {
        if (selectedItem.nickName == null) return;

        if (craftingAmount <= 1) return;
        else
            craftingAmount--;

        UpdateCraftingAmountText();
    }
    public void IncreaseCraftingAmount()
    {
        if (selectedItem.nickName == null) return;

        if (craftingAmount + 1 > inventory.CalculateCraftingAmount(selectedItem.requiredItems) || craftingAmount + 1 >= 999) return;
        else
            craftingAmount++;

        UpdateCraftingAmountText();
    }
    public void MaximizeCraftingAmount()
    {
        if (selectedItem.nickName == null) return;

        craftingAmount = inventory.CalculateCraftingAmount(selectedItem.requiredItems);
        if (craftingAmount >= 999)
            craftingAmount = 999;

        UpdateCraftingAmountText();
    }
    public void CraftingAmountInputfieldValueChanged()
    {
        if (selectedItem.nickName == null) return;

        int amount = int.Parse(craftingAmountInputfield.text);

        if (amount > 999) amount = 999;

        if (amount > inventory.CalculateCraftingAmount(selectedItem.requiredItems)) return;

        craftingAmount = amount;
        UpdateCraftingAmountText();
    }
    public void UpdateCraftingAmountText()
    {
        craftingAmountInputfield.text = craftingAmount.ToString();

        if (craftingAmount <= 0)
        {
            craftButton.GetComponent<Button>().interactable = false;
        }
        else
            craftButton.GetComponent<Button>().interactable = true;
    }
    public void CraftItem()
    {
        if (selectedItem.nickName == null || !campfire.CheckCraftingOrderSpace()) return;

        if (inventory.CalculateCraftingAmount(selectedItem.requiredItems) < craftingAmount) return;

        List<CraftStruct> itemsInside = new List<CraftStruct>();

        for (int i = 0; i < selectedItem.requiredItems.Count; i++)
        {
            itemsInside.Add(new CraftStruct
            {
                nickName = selectedItem.requiredItems[i].nickName,
                amount = selectedItem.requiredItems[i].amount * craftingAmount,
                image = selectedItem.requiredItems[i].image
            });

            inventory.DecreaseItem(selectedItem.requiredItems[i].nickName, selectedItem.requiredItems[i].amount * craftingAmount);
        }

        campfire.CreateCraftingOrder(itemsInside, selectedItem, craftingAmount);

        audioSource.PlayOneShot(craftingSound);
        RefreshCraftingOrderCampfireCanvas();

        UpdateDescription(selectedItem.nickName);
    }
    public void RefreshCraftingOrderCampfireCanvas()
    {
        if (campfire == null) return;

        CheckCraftingOrderSlots();

        if(craftingOrderSlots.Count == campfire.craftingOrderSlots.Count)
        {
            for (int i = 0; i < campfire.craftingOrderSlots.Count; i++)
            {
                CraftingOrderCampfire craftingOrderCampfire = campfire.craftingOrderSlots[i].GetComponent<CraftingOrderCampfire>();
                CraftingOrderCampfireCanvas craftingOrderCampfireCanvas = craftingOrderSlots[i].GetComponent<CraftingOrderCampfireCanvas>();

                craftingOrderCampfireCanvas.RefreshText(craftingOrderCampfire.amount, craftingOrderCampfire.remainedTime);
            }
            return;
        }

        for (int i = 0; i < craftingOrderSlots.Count; i++)
        {
            Destroy(craftingOrderSlots[i]);
        }

        craftingOrderSlots.Clear();

        for (int i = 0; i < campfire.craftingOrderSlots.Count; i++)
        {
            GameObject orderCampfireCanvas = Instantiate(craftingOrderCampfireCanvasPrefab, craftingOrderPanel);

            CraftingOrderCampfire craftingOrderCampfire = campfire.craftingOrderSlots[i].GetComponent<CraftingOrderCampfire>();
            CraftingOrderCampfireCanvas craftingOrderCampfireCanvas = orderCampfireCanvas.GetComponent<CraftingOrderCampfireCanvas>();

            Item itemImage = itemList.GetItem(craftingOrderCampfire.nickName);
            craftingOrderCampfireCanvas.SetVariable(inventory,
                craftingOrderCampfire, itemImage.image);

            craftingOrderCampfireCanvas.RefreshText(craftingOrderCampfire.amount, craftingOrderCampfire.remainedTime);

            craftingOrderSlots.Add(orderCampfireCanvas);
            orderCampfireCanvas.GetComponent<Button>().onClick.AddListener(delegate { craftingOrderCampfireCanvas.CancelCraft(); });
        }
    }
    private void CheckCraftingOrderSlots()
    {
        for (int i = 0; i < craftingOrderSlots.Count; i++)
        {
            if (craftingOrderSlots[i] == null)
            {
                craftingOrderSlots.RemoveAt(i);
            }
        }
    }
}
