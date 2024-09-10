using System.Collections.Generic;
using UnityEngine;

public class BackpackCanvas : MonoBehaviour
{
    private Backpack backpack;
    private Inventory inventory;
    private ItemList itemList;
    private LockMovement lockMovement;

    [SerializeField] private GameObject backpackPanel;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsParent;

    private List<Slot> slots = new List<Slot>();

    private float counter = 0.2f;
    private float counterMax = 0.2f;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
    }
    void Start()
    {
        CreateSlots();
    }
    void Update()
    {
        CheckInputs();
        CheckSlots();
        CheckCounter();
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Close();
        }
    }
    private void CreateSlots()
    {
        for (int i = 0; i < 63; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            Slot slotCode = slot.GetComponent<Slot>();
            slotCode.SetParameters(inventory, itemList, null);
            slots.Add(slotCode);
        }
    }
    private void CheckCounter()
    {
        if (counter > 0) counter -= Time.deltaTime;
    }
    private void PlaceItems(List<Item> items)
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
    private void CheckSlots()
    {
        if (backpack == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (backpack.items[i] != slots[i].item)
            {
                backpack.items[i] = slots[i].item;
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
    public void Open(GameObject myObject)
    {
        if (counter > 0 || lockMovement.locked || lockMovement.playerBusy || backpackPanel.activeInHierarchy) return;

        counter = counterMax;

        Backpack backpackCode = myObject.GetComponent<Backpack>();
        PlaceItems(backpackCode.items);
        backpack = backpackCode;
        backpackCode.SetOpenVariable(true);

        backpackPanel.SetActive(true);
        inventory.OpenInventory(false, false);

        lockMovement.LockInventory();
    }
    private void Close()
    {
        if (counter > 0 || !lockMovement.locked || lockMovement.playerBusy || !backpackPanel.activeInHierarchy) return;

        counter = counterMax;
        backpack.SetOpenVariable(false);
        backpack = null;
        lockMovement.UnlockInventory();

        backpackPanel.SetActive(false);
        inventory.CloseInventory();
    }

}
