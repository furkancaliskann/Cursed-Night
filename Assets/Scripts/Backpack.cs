using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    private ItemList itemList;

    private BoxCollider boxCollider;

    public List<Item> items = new List<Item>();

    private int slotCount = 63;
    private bool isOpen;
    public float destroyCounter {  get; private set; }

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
    }
    void Start()
    {
        Invoke(nameof(ActivateCollider), 3f);
    }
    void Update()
    {
        CheckNull();

        destroyCounter -= Time.deltaTime;
        if(destroyCounter <= 0) Destroy(gameObject);
    }
    private void CheckNull()
    {
        if (items.Count < slotCount || isOpen) return;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null) return;
        }

        Destroy(gameObject);
    }
    public void SetOpenVariable(bool value)
    {
        isOpen = value;
    }
    private void ActivateCollider()
    {
        boxCollider.isTrigger = false;
    }
    private void CreateItems()
    {
        for (int i = 0; i < slotCount; i++) 
        {
            items.Add(null);
        }
    }
    public void TransferItems(List<GameObject> inventorySlots)
    {
        CreateItems();

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Item item = inventorySlots[i].GetComponent<Slot>().item;

            for (int j = 0; j < items.Count; j++)
            {
                if (items[j] == null)
                {
                    items[j] = item;
                    break;
                }
            }
            
        }

        destroyCounter = 300;
    }
    public void LoadItems(float destroyCounter, SaveInventory loaded)
    {
        CreateItems();

        for (int i = 0; i < items.Count; i++)
        {
            items[i] = itemList.CreateNewItem(loaded.nickName[i], loaded.amount[i], loaded.durability[i], loaded.ammoInside[i]);
        }

        this.destroyCounter = destroyCounter;
    }
}
