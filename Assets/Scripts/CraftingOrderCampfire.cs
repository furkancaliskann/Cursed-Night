using System.Collections.Generic;
using UnityEngine;

public class CraftingOrderCampfire : MonoBehaviour
{
    private Campfire campfire;
    public string nickName { get; private set; }
    public int amount { get; private set; }
    public int multiplier { get; private set; }
    public int remainedTime { get; private set; }
    public int remainedTimeMax { get; private set; }

    public List<CraftStruct> itemsInside { get; private set; }
    public List<CraftStruct> requiredMaterialForSingleItem { get; private set; }

    public void SetOrder(Campfire campfire, List<CraftStruct> itemsInside, List<CraftStruct> requiredMaterialForSingleItem,
        string nickName, int amount, int multiplier, int craftingTime, int craftingTimeMax)
    {
        this.campfire = campfire;
        this.itemsInside = itemsInside;
        this.requiredMaterialForSingleItem = requiredMaterialForSingleItem;
        this.nickName = nickName;
        this.amount = amount;
        this.multiplier = multiplier;
        remainedTimeMax = craftingTimeMax;
        remainedTime = craftingTime;
    }
    public void DecreaseCraftingTime()
    {
        remainedTime--;

        if (remainedTime <= 0)
        {
            campfire.AddItem(nickName, multiplier);

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
    }
    private void DecreaseItemInside()
    {
        for (int i = 0; i < requiredMaterialForSingleItem.Count; i++)
        {
            var index = itemsInside.FindIndex(x => x.nickName == requiredMaterialForSingleItem[i].nickName);
            if (index != -1)
            {
                var item = itemsInside[index];
                item.amount -= requiredMaterialForSingleItem[i].amount;
                itemsInside[index] = item;
            }
        }
    }
}
