using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOrder : MonoBehaviour
{
    private Inventory inventory;

    public string nickName { get; private set; }
    public int amount { get; private set; }
    public int multiplier { get; private set; }
    public int remainedTime { get; private set; }
    public int remainedTimeMax { get; private set; }

    public List<CraftStruct> itemsInside { get; private set; }
    public List<CraftStruct> requiredMaterialForSingleItem { get; private set; }

    [SerializeField] private Image image;
    [SerializeField] private Text amountText;
    [SerializeField] private Text timeText;

    public void SetOrder(Inventory inventory, List<CraftStruct> itemsInside, List<CraftStruct> requiredMaterialForSingleItem,
        string nickName, int amount, int multiplier, int craftingTime, int craftingTimeMax, Sprite sprite)
    {
        this.inventory = inventory;
        this.itemsInside = itemsInside;
        this.requiredMaterialForSingleItem = requiredMaterialForSingleItem;
        this.nickName = nickName;
        this.amount = amount;
        this.multiplier = multiplier;
        remainedTimeMax = craftingTimeMax;
        remainedTime = craftingTime;
        image.sprite = sprite;
    }
    public void DecreaseCraftingTime()
    {
        remainedTime--;

        if (remainedTime <= 0)
        {
                inventory.AddItem(nickName, multiplier, true);
                //campfire.AddItem(nickName, multiplier);

                DecreaseItemInside();

            amount--;

            if (amount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                remainedTime = remainedTimeMax;
            }
        }

        RefreshText();
    }
    private void DecreaseItemInside()
    {
        for (int i = 0; i < requiredMaterialForSingleItem.Count; i++)
        {
            var index = itemsInside.FindIndex(x => x.nickName == requiredMaterialForSingleItem[i].nickName);
            if(index != -1)
            {
                var item = itemsInside[index];
                item.amount -= requiredMaterialForSingleItem[i].amount;
                itemsInside[index] = item;
            }
        }
    }
    public void CancelCraft()
    {
        for (int i = 0; i < itemsInside.Count; i++)
        {
            inventory.AddItem(itemsInside[i].nickName, itemsInside[i].amount, true);
        }

        Destroy(gameObject);
    }
    public void RefreshText()
    {
        amountText.text = "x" + amount;

        int minute = remainedTime / 60;
        int second = remainedTime % 60;

        timeText.text = string.Format("{0:00}:{1:00}", minute, second);
    }
}
[System.Serializable]
public struct CraftStruct
{
    public string nickName;
    public int amount;
    public Sprite image;
}
