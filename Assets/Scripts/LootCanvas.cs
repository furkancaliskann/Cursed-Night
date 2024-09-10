using System.Collections.Generic;
using UnityEngine;
public class LootCanvas : MonoBehaviour
{
    private Inventory inventory;
    private ItemList itemList;
    private LockMovement lockMovement;

    [HideInInspector] public List<Slot> slots = new List<Slot>();
    private int slotCount = 15;
    private float openCloseLatency = 0.1f;

    [SerializeField] private GameObject lootPanel;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;

    [HideInInspector] public Loot loot;

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
        CheckSlots();
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ClosePanel();
        }
    }
    private void CreateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject myObject = Instantiate(slotPrefab, slotParent);
            Slot slotCode = myObject.GetComponent<Slot>();
            slots.Add(slotCode);
            slotCode.SetParameters(inventory, itemList, null);
        }
    }
    private void PlaceItems()
    {
        if (loot == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item != loot.items[i])
            {
                slots[i].SetItem(loot.items[i]);
                slots[i].UpdateSlot();
            }    
        }
    }
    private void CheckSlots()
    {
        if (loot == null) return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item != loot.items[i])
            {
                loot.items[i] = slots[i].item;
            }
        }
    }
    public void OpenPanel(GameObject lootObject)
    {
        loot = lootObject.GetComponent<Loot>();

        if (lockMovement.locked || lockMovement.playerBusy || lootPanel.activeInHierarchy) return;

        inventory.OpenInventory(false, false);
        lockMovement.LockInventory();
        lootPanel.SetActive(true);

        lockMovement.Lock();
        lockMovement.KeepBusy(openCloseLatency);

        PlaceItems();
        loot.SetPanelOpen(true);
    }
    private void ClosePanel()
    {
        if (!lockMovement.locked || lockMovement.playerBusy || !lootPanel.activeInHierarchy) return;

        lockMovement.UnlockInventory();
        lootPanel.SetActive(false);
        inventory.CloseInventory();
        lockMovement.Unlock();
        lockMovement.KeepBusy(openCloseLatency);
        loot.SetPanelOpen(false);
    }
    public void ResetSlotsColor()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].GetComponent<Slot>().ResetSlotColor();
        }

    }
}
