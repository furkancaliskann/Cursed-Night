using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class FurnaceCanvas : MonoBehaviour
{
    private Furnace furnace;
    private Inventory inventory;
    private ItemList itemList;
    private LockMovement lockMovement;

    [SerializeField] private GameObject furnacePanel;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParentSmelt, slotParentSmelted, slotParentFuel;

    private Slot smeltSlot;
    private Slot smeltedSlot;
    private Slot fuelSlot;

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
        GameObject slot = Instantiate(slotPrefab, slotParentSmelt);
        Slot slotCode = slot.GetComponent<Slot>();
        slotCode.SetParameters(inventory, itemList, null);
        smeltSlot = slotCode;

        GameObject slot2 = Instantiate(slotPrefab, slotParentSmelted);
        Slot slotCode2 = slot2.GetComponent<Slot>();
        slotCode2.SetParameters(inventory, itemList, null);
        smeltedSlot = slotCode2;

        GameObject slot3 = Instantiate(slotPrefab, slotParentFuel);
        Slot slotCode3 = slot3.GetComponent<Slot>();
        slotCode3.SetParameters(inventory, itemList, null);
        fuelSlot = slotCode3;
    }
    private void CheckCounter()
    {
        if (counter > 0) counter -= Time.deltaTime;
    }
    public void PlaceItems(Item smeltItem, Item smeltedItem, Item fuelItem)
    {
        smeltSlot.SetItem(smeltItem);
        smeltSlot.UpdateSlot();

        smeltedSlot.SetItem(smeltedItem);
        smeltedSlot.UpdateSlot();

        fuelSlot.SetItem(fuelItem);
        fuelSlot.UpdateSlot();
    }
    private void CheckSlots()
    {
        if (furnace == null) return;

        if (furnace.smeltItem != smeltSlot.item) furnace.smeltItem = smeltSlot.item;
        if (furnace.smeltedItem != smeltedSlot.item) furnace.smeltedItem = smeltedSlot.item;
        if (furnace.fuelItem != fuelSlot.item) furnace.fuelItem = fuelSlot.item;
    }
    public void ResetSlotsColor()
    {
        smeltSlot.GetComponent<Slot>().ResetSlotColor();
        smeltedSlot.GetComponent<Slot>().ResetSlotColor();
        fuelSlot.GetComponent<Slot>().ResetSlotColor();
    }
    public void StartStopFurnace()
    {
        furnace.StartStopFurnace();
    }
    public void Open(GameObject myObject)
    {
        if (counter > 0 || lockMovement.locked || lockMovement.playerBusy || furnacePanel.activeInHierarchy) return;

        counter = counterMax;

        Furnace furnaceCode = myObject.GetComponent<Furnace>();
        PlaceItems(furnaceCode.smeltItem, furnaceCode.smeltedItem, furnaceCode.fuelItem);
        furnace = furnaceCode;

        furnaceCode.SetFurnaceCanvas(this);
        
        furnacePanel.SetActive(true);
        inventory.OpenInventory(false, false);

        lockMovement.LockInventory();
    }
    private void Close()
    {
        if (counter > 0 || !lockMovement.locked || lockMovement.playerBusy || !furnacePanel.activeInHierarchy) return;

        counter = counterMax;
        lockMovement.UnlockInventory();
        furnace.SetFurnaceCanvas(null);
        furnace = null;

        furnacePanel.SetActive(false);
        inventory.CloseInventory();
    }
}
