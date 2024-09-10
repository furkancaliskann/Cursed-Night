using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    private AudioSource audioSource;
    public CampfireCanvas campfireCanvas;
    private ItemList itemList;

    [SerializeField] private Light lightSource;
    [SerializeField] private ParticleSystem particle;   

    public List<Item> items = new List<Item>();

    private bool animationsPlaying;

    public List<GameObject> craftingOrderSlots = new List<GameObject>();
    public GameObject craftingOrderPrefab; // This is for load crafting orders
    public Transform craftingOrderPanel;// This is for load crafting orders

    private float counter = 1f;
    private float counterMax = 1f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();

        CreateItems();
    }
    void Update()
    {
        CheckCounter();
        CheckCraftingOrderSlots();
        CheckEffects();
    }
    private void CreateItems()
    {
        for (int i = 0; i < 8; i++)
        {
            items.Add(null);
        }
    }
    public void SetCampfireCanvas(CampfireCanvas campfireCanvas)
    {
        this.campfireCanvas = campfireCanvas;
    }
    private void CheckEffects()
    {
        if (craftingOrderSlots.Count > 0 && !animationsPlaying)
        {
            animationsPlaying = true;
            StartCampfireEffects();
        }
        else if (craftingOrderSlots.Count <= 0 && animationsPlaying)
        {
            animationsPlaying = false;
            StopCampfireEffects();
        }
    }
    private void StopCampfireEffects()
    {
        particle.Stop();
        lightSource.enabled = false;
        audioSource.Stop();
    }
    private void StartCampfireEffects()
    {
        particle.Play();
        lightSource.enabled = true;
        audioSource.Play();
    }



    private void CheckCounter()
    {
        if (counter > 0 && craftingOrderSlots.Count > 0) counter -= Time.deltaTime;
        else counter = counterMax;

        if (counter <= 0)
        {
            CraftingOrderCampfire craftingOrderCampfire = craftingOrderSlots[0].GetComponent<CraftingOrderCampfire>();
            craftingOrderCampfire.DecreaseCraftingTime();
            counter = counterMax;

            if (campfireCanvas != null) campfireCanvas.RefreshCraftingOrderCampfireCanvas();
        }
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
    public bool CheckCraftingOrderSpace()
    {
        if (craftingOrderSlots.Count < 4) return true;
        else return false;
    }
    public void CreateCraftingOrder(List<CraftStruct> itemsInside, CraftableItem selectedItem, int craftingAmount)
    {
        GameObject myObject = Instantiate(craftingOrderPrefab, craftingOrderPanel);

        CraftingOrderCampfire craftingOrderCampfire = myObject.GetComponent<CraftingOrderCampfire>();

        craftingOrderCampfire.SetOrder(this, itemsInside, selectedItem.requiredItems, selectedItem.nickName, craftingAmount, selectedItem.multiplier, selectedItem.craftingTime, selectedItem.craftingTime);

        craftingOrderSlots.Add(myObject);
    }
    public void LoadCraftingOrder(List<CraftStruct> itemsInside, List<CraftStruct> requiredMaterialForSingleItem,
        string nickName, int amount, int multiplier, int remainedTime, int remainedTimeMax)
    {
        GameObject myObject = Instantiate(craftingOrderPrefab, craftingOrderPanel);

        CraftingOrderCampfire craftingOrderCampfire = myObject.GetComponent<CraftingOrderCampfire>();

        craftingOrderCampfire.SetOrder(this, itemsInside, requiredMaterialForSingleItem, nickName,
            amount,multiplier, remainedTime, remainedTimeMax);

        craftingOrderSlots.Add(myObject);
    }
    public void LoadItems(SaveInventory loaded)
    {
        for (int i = 0; i < loaded.nickName.Count; i++)
        {
            items[i] = itemList.CreateNewItem(loaded.nickName[i], loaded.amount[i], loaded.durability[i], loaded.ammoInside[i]);
        }
    }
    public void AddItem(string nickName, int multiplier)
    {
        int remainedAmount = multiplier;

        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            if (item == null) continue;

            if (item.nickName == nickName && item.amount < item.maxAmount)
            {
                if (item.amount + remainedAmount > item.maxAmount)
                {
                    int addedAmount = item.maxAmount - item.amount;
                    remainedAmount -= addedAmount;
                    item.IncreaseAmount(addedAmount);
                }
                else
                {
                    item.IncreaseAmount(remainedAmount);
                    if (campfireCanvas != null) campfireCanvas.PlaceItems(items);
                    return;
                }
            }
        }

        for (int i = 0; i < items.Count; i++) // boþ olan slota ekle
        {
            if (items[i] != null) continue;

            items[i] = itemList.CreateNewItem(nickName, multiplier);
            if (campfireCanvas != null) campfireCanvas.PlaceItems(items);
            return;
        }
    }
}
