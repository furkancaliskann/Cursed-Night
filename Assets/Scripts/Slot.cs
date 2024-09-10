using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler
{
    private Inventory inventory;
    private ItemList itemList;

    [SerializeField] private Image slotColorImage;
    private Color baseColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    private Color selectedColor = new Color(168f / 255f, 168f / 255f, 168f / 255f);

    private AudioSource audioSource;
    [SerializeField] private AudioClip beginDragClip;
    [SerializeField] private AudioClip dropClip;

    [SerializeField] private GameObject selectedSlotImageObject;
    [SerializeField] private Text amountText;
    [SerializeField] private Image durabilityBar;
    [SerializeField] private Image image;
    public Item item {  get; private set; }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SetParameters(Inventory inventory, ItemList itemList, Item item)
    {
        this.inventory = inventory;
        this.itemList = itemList;
        this.item = item;

        UpdateSlot();
    }
    public void SetItem(Item item)
    {
        this.item = item;
        UpdateSlot();
    }
    public void UpdateSlot()
    {
        if(item == null || item.amount <= 0 || (item.durability == 0 && item.maxDurability > 0))
        {
            if (item != null && item.durability == 0 && item.maxDurability > 0) inventory.PlayItemBreakingSound();
            amountText.text = "";
            durabilityBar.gameObject.SetActive(false);
            image.gameObject.SetActive(false);
            image.sprite = null;
            item = null;
            return;
        }

        image.gameObject.SetActive(true);
        image.sprite = item.image;

        if (item.maxAmount <= 1)
        {
            amountText.gameObject.SetActive(false);
            amountText.text = "";
        }
        else
        {
            amountText.gameObject.SetActive(true);
            amountText.text = item.amount.ToString();
        }

        if (item.durability > 0 && item.blockType == BlockTypes.Empty)
        {
            durabilityBar.gameObject.SetActive(true);
            durabilityBar.transform.localScale = new Vector3((float)item.durability / item.maxDurability, 1, 1);
        }
    }
    public void ResetSlotColor()
    {
        slotColorImage.color = baseColor;
    }
    public void ActivateSlotColor()
    {
        GetComponent<Image>().color = selectedColor;
    }
    public void ActivateSelectedSlot()
    {
        if(!selectedSlotImageObject.activeInHierarchy)
        selectedSlotImageObject.SetActive(true);
    }
    public void DeactivateSelectedSlot()
    {
        selectedSlotImageObject.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        inventory.ResetSlotsColor();
        audioSource.PlayOneShot(beginDragClip);

        if (Input.GetMouseButton(0))
        {
            inventory.draggingItem.SetItem(item);
            SetItem(null);
        }

        if(Input.GetMouseButton(1))
        {
            if (item.maxAmount < 2 || item.amount < 2) return;

            int amount = Mathf.RoundToInt(item.amount / 2);
            inventory.draggingItem.SetItem(itemList.CreateNewItem(item.id, amount));
            item.DecreaseAmount(amount);
            UpdateSlot();
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        inventory.ShowDraggingItem(true);
        inventory.ChangeDraggingItemPosition(Input.mousePosition);
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (inventory.draggingItem.item == null) return;

        audioSource.PlayOneShot(dropClip);

        if (item == null)
        {
            SetItem(inventory.draggingItem.item);
        }
        else if(item.id == inventory.draggingItem.item.id && item.amount != item.maxAmount) // ayný eþyanýn üzerine ekle
        {
            if (item.amount + inventory.draggingItem.item.amount > inventory.draggingItem.item.maxAmount)
            {
                int addingAmount = item.maxAmount - item.amount;
                int remainedAmount = inventory.draggingItem.item.amount - addingAmount;
                item.IncreaseAmount(addingAmount);
                UpdateSlot();
                inventory.AddItem(inventory.draggingItem.item.id, remainedAmount, false);
            }
            else
            {
                item.IncreaseAmount(inventory.draggingItem.item.amount);
                UpdateSlot();
            }
            
        }
        else
        {
            inventory.AddItem(inventory.draggingItem.item.id, inventory.draggingItem.item.amount, false);
        }

        inventory.draggingItem.SetItem(null);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        inventory.ShowDraggingItem(false);

        if (inventory.draggingItem.item != null)
        {
            inventory.DropItem(inventory.draggingItem.item);
            inventory.draggingItem.SetItem(null);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(Input.GetKey(KeyCode.LeftShift) && item != null)
        {
            Item newItem = itemList.CreateNewItem(item.id, item.amount);
            SetItem(null);
            inventory.AddItem(newItem, true);
            inventory.ResetSlotsColor();
            return;
        }

        inventory.ResetSlotsColor();
        ActivateSlotColor();

        if (item != null)
            inventory.SetItemDescriptionText("-  " + item.name + "  -" + "\n" + item.description);
        else
            inventory.SetItemDescriptionText("");
    }
}
