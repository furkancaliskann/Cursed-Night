using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCanvas : MonoBehaviour
{
    private Inventory inventory;
    private ItemList itemList; 
    private SaveManager saveManager;

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
    [SerializeField] private GameObject craftingOrderPrefab;
    private List<GameObject> craftingOrderSlots = new List<GameObject>();

    private AudioSource audioSource;
    [SerializeField] private AudioClip craftingSound;

    private float counter = 1f;
    private float counterMax = 1f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inventory = GetComponent<Inventory>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
    }
    void Start()
    {
        CreateCraftableItem("cotton_seed", 5, 2, CreateStruct("cotton", 1));
        CreateCraftableItem("potato_seed", 5, 3, CreateStruct("potato", 1));
        CreateCraftableItem("corn_seed", 5, 3, CreateStruct("corn", 1));
        CreateCraftableItem("cucumber_seed", 5, 3, CreateStruct("cucumber", 1));
        CreateCraftableItem("mushroom_seed", 5, 3, CreateStruct("mushroom", 1));
        CreateCraftableItem("pepper_seed", 5, 3, CreateStruct("pepper", 1));
        CreateCraftableItem("tomato_seed", 5, 3, CreateStruct("tomato", 1));
        CreateCraftableItem("onion_seed", 5, 3, CreateStruct("onion", 1));
        CreateCraftableItem("9mm_ammo", 5, 1, CreateStruct("bullet_tip", 1, "bullet_mold", 1, "powder", 1));
        CreateCraftableItem("shotgun_shell", 11, 1, CreateStruct("paper", 2, "plastic", 1, "powder", 3));
        CreateCraftableItem("7.62mm_ammo", 8, 1, CreateStruct("bullet_tip", 1, "bullet_mold", 1, "powder", 2));
        CreateCraftableItem("landmine", 30, 1, CreateStruct("forged_iron", 100, "powder", 20));
        CreateCraftableItem("gasoline", 45, 100, CreateStruct("animal_fat", 10, "chemical", 2));
        CreateCraftableItem("cloth", 4, 1, CreateStruct("cotton", 2));
        CreateCraftableItem("bandage", 5, 1, CreateStruct("cloth", 2));
        CreateCraftableItem("first_aid_kit", 30, 1, CreateStruct("bandage", 1, "cream", 1, "tape", 1));
        CreateCraftableItem("flour", 20, 1, CreateStruct("corn", 2));
        CreateCraftableItem("cucumber_extract", 30, 1, CreateStruct("cucumber", 2));
        CreateCraftableItem("cream", 15, 1, CreateStruct("cucumber_extract", 3));
        CreateCraftableItem("screw", 45, 1, CreateStruct("forged_iron", 100));
        CreateCraftableItem("pan", 40, 1, CreateStruct("forged_iron", 100));
        CreateCraftableItem("campfire", 60, 1, CreateStruct("pan", 1, "wood", 200, "stone", 100));
        CreateCraftableItem("tape", 20, 1, CreateStruct("glue", 1, "chemical", 1));
        CreateCraftableItem("bullet_mold", 5, 1, CreateStruct("forged_iron", 5));
        CreateCraftableItem("stick", 3, 20, CreateStruct("wood", 10));
        CreateCraftableItem("powder", 5, 1, CreateStruct("forged_sulfur", 1, "coal", 1));
        CreateCraftableItem("pistol", 60, 1, CreateStruct("forged_iron", 300, "screw", 1, "plastic", 3));
        CreateCraftableItem("assault_rifle", 100, 1, CreateStruct("forged_iron", 500, "screw", 10, "plastic", 10, "chemical", 5));
        CreateCraftableItem("shotgun", 80, 1, CreateStruct("forged_iron", 350, "screw", 5, "plastic", 5, "chemical", 3));
        CreateCraftableItem("hunting_rifle", 120, 1, CreateStruct("forged_iron", 750, "screw", 10, "plastic", 10, "chemical", 5, "scope", 1));
        CreateCraftableItem("bow", 30, 1, CreateStruct("stick", 200));
        CreateCraftableItem("crossbow", 45, 1, CreateStruct("wood", 100, "stick", 300, "forged_iron", 100, "cloth", 10, "rope", 2));
        CreateCraftableItem("sword", 30, 1, CreateStruct("forged_iron", 150, "wood", 100, "screw", 5));
        CreateCraftableItem("baseball_bat", 40, 1, CreateStruct("wood", 500, "screw", 10));
        CreateCraftableItem("spear", 35, 1, CreateStruct("forged_iron", 200, "wood", 200, "screw", 6));
        CreateCraftableItem("arrow", 5, 2, CreateStruct("feather", 1, "stone", 5));
        CreateCraftableItem("dynamite", 50, 1, CreateStruct("forged_iron", 300, "powder", 50));
        CreateCraftableItem("axe", 30, 1, CreateStruct("forged_iron", 100, "wood", 300));
        CreateCraftableItem("hammer", 25, 1, CreateStruct("forged_iron", 100, "wood", 200));
        CreateCraftableItem("wood_frame", 3, 1, CreateStruct("wood", 20));
        CreateCraftableItem("wood_stair", 5, 1, CreateStruct("wood", 100));
        CreateCraftableItem("stone_stair", 5, 1, CreateStruct("stone", 100));
        CreateCraftableItem("iron_stair", 5, 1, CreateStruct("forged_iron", 100));
        CreateCraftableItem("wood_door", 15, 1, CreateStruct("wood", 200, "screw", 5));
        CreateCraftableItem("iron_door", 25, 1, CreateStruct("forged_iron", 500, "screw", 10));
        CreateCraftableItem("glass", 10, 1, CreateStruct("broken_bottle", 2, "tape", 1));
        CreateCraftableItem("furnace", 60, 1, CreateStruct("pan", 3, "wood", 400, "stone", 500));
        CreateCraftableItem("sleeping_bag", 45, 1, CreateStruct("cloth", 20, "stick", 50, "rope", 5));
        CreateCraftableItem("chest", 30, 1, CreateStruct("forged_iron", 100, "wood", 200, "screw", 1));
        CreateCraftableItem("farming_plot", 20, 1, CreateStruct("soil", 5, "wood", 200, "glue", 1, "screw", 1));
        CreateCraftableItem("ladder", 10, 1, CreateStruct("wood", 200, "screw", 1));
        CreateCraftableItem("splint", 15, 1, CreateStruct("stick", 100, "screw", 5, "tape", 2));


        PlaceItems();
        UpdateCraftingAmountText();
    }
    void Update()
    {
        CheckCraftingOrderSlots();
        CheckCounter();
    }
    private void CheckCraftingOrderSlots()
    {
        for (int i = 0; i < craftingOrderSlots.Count; i++)
        {
            if (craftingOrderSlots[i] == null)
            {
                craftingOrderSlots.Remove(craftingOrderSlots[i]);
            }
        }
    }
    private void CheckCounter()
    {
        if (counter > 0 && craftingOrderSlots.Count > 0) counter -= Time.deltaTime;
        else counter = counterMax;

        if (counter <= 0)
        {
            CraftingOrder craftingOrder = craftingOrderSlots[0].GetComponent<CraftingOrder>();
            craftingOrder.DecreaseCraftingTime();
            counter = counterMax;
        }
    }
    private List<CraftStruct> CreateStruct(string nickname, int amount)
    {
        List<CraftStruct> craftStruct = new List<CraftStruct>
        {
            { new CraftStruct { nickName = nickname, amount = amount } }
        };

        return craftStruct;
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

        craftableItems.Add(new CraftableItem { nickName = nickName, name = item.name, description = item.description,
            craftingTime = craftingTime,  multiplier = multiplier, requiredItems = requiredItems, image = item.image});
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
        craftableItems.Sort((a,b) => a.name.CompareTo(b.name));

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

            GameObject myObject = Instantiate(craftableObjectPrefab, pages[pages.Count-1].transform);

            myObject.GetComponentInChildren<Text>().text = craftableItems[i].name;
            myObject.GetComponentsInChildren<Image>()[1].sprite = craftableItems[i].image;

            string referanceID = craftableItems[i].nickName;
            myObject.GetComponent<Button>().onClick.AddListener(delegate { UpdateDescription(referanceID);});

        }

        ChangePage(1);
    }
    public void SearchItem()
    {
        if (searchItemInputfield.text == "" && !craftingOrderPanel.gameObject.activeInHierarchy) return;

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
    public void ChangePage(int pageToOpen)
    {
        if (!craftingOrderPanel.gameObject.activeInHierarchy) return;
        if (pages.Count <= pageToOpen-1 || pageToOpen <= 0) return;

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

        int canCraftAmount = inventory.CalculateCraftingAmount(selectedItem.requiredItems);

        if (amount > canCraftAmount)
        {
            craftingAmount = canCraftAmount;
        }
        else craftingAmount = amount;

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
        if (selectedItem.nickName == null || !CheckCraftingOrderSpace()) return;

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

            inventory.DecreaseItem(selectedItem.requiredItems[i].nickName ,selectedItem.requiredItems[i].amount * craftingAmount);
        }

        GameObject myObject = Instantiate(craftingOrderPrefab, craftingOrderPanel);

        CraftingOrder craftingOrder = myObject.GetComponent<CraftingOrder>();
        craftingOrder.SetOrder(inventory, itemsInside, selectedItem.requiredItems, selectedItem.nickName, craftingAmount, selectedItem.multiplier, selectedItem.craftingTime, selectedItem.craftingTime, selectedItem.image);
        craftingOrder.RefreshText();

        craftingOrderSlots.Add(myObject);

        audioSource.PlayOneShot(craftingSound);

        myObject.GetComponent<Button>().onClick.AddListener(delegate { craftingOrder.CancelCraft();});


        UpdateDescription(selectedItem.nickName);
    }
    private bool CheckCraftingOrderSpace()
    {
        if (craftingOrderSlots.Count < 4) return true;
        else return false;
    }
    public void SaveCraftingOrder()
    {
        SaveCraftingOrder saveCraftingOrder = new SaveCraftingOrder();

        for (int i = 0; i < craftingOrderSlots.Count; i++)
        {
            CraftingOrder craftingOrder = craftingOrderSlots[i].GetComponent<CraftingOrder>();

            saveCraftingOrder.nickName.Add(craftingOrder.nickName);
            saveCraftingOrder.amount.Add(craftingOrder.amount);
            saveCraftingOrder.multiplier.Add(craftingOrder.multiplier);
            saveCraftingOrder.remainedTime.Add(craftingOrder.remainedTime);
            saveCraftingOrder.remainedTimeMax.Add(craftingOrder.remainedTimeMax);

            ListCraftStruct craftStruct = new ListCraftStruct();
            craftStruct.craftStruct.AddRange(craftingOrder.itemsInside);
            saveCraftingOrder.itemsInside.Add(craftStruct);

            ListCraftStruct craftStruct2 = new ListCraftStruct();
            craftStruct2.craftStruct.AddRange(craftingOrder.requiredMaterialForSingleItem);
            saveCraftingOrder.requiredMaterialForSingleItem.Add(craftStruct2);
        }

        StartCoroutine(saveManager.Save(saveCraftingOrder, SaveType.CraftingOrder));
    }
    public void LoadCraftingOrder()
    {
        SaveCraftingOrder loadedCraftingOrder = JsonUtility.FromJson<SaveCraftingOrder>(saveManager.Load(SaveType.CraftingOrder));
        if (loadedCraftingOrder == null) return;

        for (int i = 0; i < loadedCraftingOrder.nickName.Count; i++)
        {
            Item item = itemList.GetItem(loadedCraftingOrder.nickName[i]);
            GameObject myObject = Instantiate(craftingOrderPrefab, craftingOrderPanel);

            CraftingOrder craftingOrder = myObject.GetComponent<CraftingOrder>();
            craftingOrder.SetOrder(inventory, loadedCraftingOrder.itemsInside[i].craftStruct,
                loadedCraftingOrder.requiredMaterialForSingleItem[i].craftStruct, loadedCraftingOrder.nickName[i],
                loadedCraftingOrder.amount[i], loadedCraftingOrder.multiplier[i], loadedCraftingOrder.remainedTime[i],
                loadedCraftingOrder.remainedTimeMax[i], item.image);
            craftingOrder.RefreshText();

            craftingOrderSlots.Add(myObject);

            myObject.GetComponent<Button>().onClick.AddListener(delegate { craftingOrder.CancelCraft(); });
        }
    }
}

[System.Serializable]
public struct CraftableItem
{
    public string nickName;
    public string name;
    public string description;
    public int craftingTime;
    public int multiplier;
    public List<CraftStruct> requiredItems;
    public Sprite image;    
    
}

[System.Serializable]
public class SaveCraftingOrder
{
    public List<string> nickName = new List<string>();
    public List<int> amount = new List<int>();
    public List<int> multiplier = new List<int>();
    public List<int> remainedTime = new List<int>();
    public List<int> remainedTimeMax = new List<int>();
    public List<ListCraftStruct> itemsInside = new List<ListCraftStruct>();
    public List<ListCraftStruct> requiredMaterialForSingleItem = new List<ListCraftStruct>();
}

[System.Serializable]
public class ListCraftStruct
{
    public List<CraftStruct> craftStruct = new List<CraftStruct>();
}