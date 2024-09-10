using System.Collections.Generic;
using UnityEngine;

public class ChestCanvas : MonoBehaviour
{
    private Chest chest;
    private Inventory inventory;
    private ItemList itemList;
    private LockMovement lockMovement;

    [SerializeField] private GameObject chestPanel;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotsPanel;

    private List<Slot> slots = new List<Slot>();

    float counter = 0.2f;
    float counterMax = 0.2f;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();
        lockMovement = GetComponent<LockMovement>();
    }
    void Start()
    {
        CreateSlots();
    }
    void Update()
    {
        CheckInputs();
        CheckCounter();
        CheckSlots();
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.F))
        {
            Close();
        }
    }
    private void CheckCounter()
    {
        if (counter > 0) counter -= Time.deltaTime;
    }
    private void CreateSlots()
    {
        for (int i = 0; i < 35; i++)
        {
            GameObject slotObject = Instantiate(slotPrefab, slotsPanel);
            slots.Add(slotObject.GetComponent<Slot>());

            Slot slotCode = slotObject.GetComponent<Slot>();
            slotCode.SetParameters(inventory, itemList, null);
        }
    }
    private void PlaceSlots(List<Item> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetItem(items[i]);
            slots[i].UpdateSlot();
        }
    }
    private void CheckSlots()
    {
        if (chest == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (chest.items[i] != slots[i].item)
            {
                chest.items[i] = slots[i].item;
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
    public void Open(GameObject chestObject)
    {
        if (counter > 0 || lockMovement.locked || lockMovement.playerBusy || chestPanel.activeInHierarchy) return;

        counter = counterMax;

        Chest chestCode = chestObject.GetComponent<Chest>();
        PlaceSlots(chestCode.items);
        chest = chestCode;

        chestPanel.SetActive(true);
        inventory.OpenInventory(false, false);
        lockMovement.LockInventory();
    }
    public void Close()
    {
        if (counter > 0 || !lockMovement.locked || lockMovement.playerBusy || !chestPanel.activeInHierarchy || inventory.draggingItem.item != null) return;

        counter = counterMax;
        chest = null;
        lockMovement.UnlockInventory();

        chestPanel.SetActive(false);
        inventory.CloseInventory();
        lockMovement.KeepBusy(0.1f);
    }
}
