using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerItems : MonoBehaviour
{
    private AmmoText ammoText;
    [SerializeField] private AudioSource selectSlotAudioSource;
    private Inventory inventory;
    private SaveManager saveManager;
    private ShakeEffect shakeEffect; 
    private LockMovement lockMovement;  
    public Item selectedItem {  get; private set; }
    public int selectedSlotNo { get; private set; }
    private int previousSlotNo = -1;

    private List<GameObject> items = new List<GameObject>();
    [SerializeField] private Transform itemsParent;

    private bool needRefreshForZoom = false;
    [SerializeField] private AudioClip selectSlotAudio;

    void Awake()
    {
        ammoText = GetComponent<AmmoText>();
        inventory = GetComponent<Inventory>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
        shakeEffect = GetComponent<ShakeEffect>();
        lockMovement = GetComponent<LockMovement>();
    }
    void Update()
    {
        if (lockMovement.playerInCar) return;
        if (lockMovement.zoomOn)
        {
            CloseItems();
            needRefreshForZoom = true;
            return;
        }

        CheckInputs();
        SelectSlot(selectedSlotNo);
    }
    private void CheckInputs()
    {
        if (lockMovement.locked || lockMovement.playerBusy || lockMovement.playerInCar) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedSlotNo = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedSlotNo = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedSlotNo = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedSlotNo = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedSlotNo = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) selectedSlotNo = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) selectedSlotNo = 6;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // tekeri yukarý kaydýrmak (bir önceki slotu seçer)
        {
            if (selectedSlotNo - 1 < 0) selectedSlotNo = inventory.beltSlotCount - 1;
                else selectedSlotNo--;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // tekeri aþaðý kaydýrmak (bir sonraki slota geçer)
        {
            if (selectedSlotNo + 1 >= inventory.beltSlotCount) selectedSlotNo = 0;
            else selectedSlotNo++;
        }
    }
    public void FindPlayerItems()
    {
        for (int i = 1; i < itemsParent.childCount; i++)
        {
            items.Add(itemsParent.GetChild(i).gameObject);
        }
    }
    private void OpenItem(Item item)
    {
        CloseItems();

        if (item == null)
        {
            OpenEmptyItem();
            return;
        }
        string itemName = item.nickName;
        if (item.category == Categories.Food) itemName = "food";
        if (item.category == Categories.Water) itemName = "water";
        if (item.nickName == "first_aid_kit") itemName = "bandage";

        var result = items.Find(x => x.name == itemName);
        if (result == null)
        {
            OpenEmptyItem();
            return;
        }
            
        result.SetActive(true);
        shakeEffect.SetSelectedItem(result);
    }
    private void OpenEmptyItem()
    {
        var emptyItem = items.Find(x => x.name == "empty");
        if (emptyItem == null) return;

        emptyItem.SetActive(true);
        return;
    }
    public void CloseItems()
    {
        for (int i = 0; i < items.Count; i++)
        { 
            if(items[i].activeInHierarchy)
            {
                items[i].SetActive(false);
            }
        }
    }
    private void SelectSlot(int slotNumber)
    {
        Item item = inventory.slots[slotNumber].GetComponent<Slot>().item;

        if (selectedItem == item && selectedItem != null && !needRefreshForZoom) return;
        if (selectedItem == null && item == null && previousSlotNo == slotNumber) return;

        if(item != null && !needRefreshForZoom && previousSlotNo != slotNumber)
            selectSlotAudioSource.PlayOneShot(selectSlotAudio);

        previousSlotNo = slotNumber;
        selectedItem = item;
        needRefreshForZoom = false;

        ammoText.ClosePanel();
        inventory.OpenSelectedSlotImage(slotNumber);

        OpenItem(selectedItem);
    }
    public void ClearSelectedItemForCar()
    {
        selectedItem = null;
        ammoText.ClosePanel();
    }
    public void LoadSelectedSlot()
    {
        SaveSelectedSlot loadedSlot = JsonUtility.FromJson<SaveSelectedSlot>(saveManager.Load(SaveType.SelectedSlot));
        if (loadedSlot == null) return;

        selectedSlotNo = loadedSlot.selectedSlotNo;
    }
    public void SaveSelectedSlot()
    {
        SaveSelectedSlot saveSelectedSlot = new SaveSelectedSlot(selectedSlotNo);
        StartCoroutine(saveManager.Save(saveSelectedSlot, SaveType.SelectedSlot));
    }
}

[System.Serializable]
public class SaveSelectedSlot
{
    public int selectedSlotNo;

    public SaveSelectedSlot(int selectedSlotNo)
    {
        this.selectedSlotNo = selectedSlotNo;
    }
}
