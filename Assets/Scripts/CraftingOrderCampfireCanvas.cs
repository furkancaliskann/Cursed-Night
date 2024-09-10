using UnityEngine;
using UnityEngine.UI;

public class CraftingOrderCampfireCanvas : MonoBehaviour
{
    private CraftingOrderCampfire craftingOrderCampfire;
    private Inventory inventory;

    [SerializeField] private Image image;
    [SerializeField] private Text amountText;
    [SerializeField] private Text timeText;
    
    public void SetVariable(Inventory inventory, CraftingOrderCampfire craftingOrderCampfire, Sprite sprite)
    {
        this.craftingOrderCampfire = craftingOrderCampfire;
        this.inventory = inventory;
        image.sprite = sprite;
    }
    public void RefreshText(int amount, int remainedTime)
    {
        amountText.text = "x" + amount;

        int minute = remainedTime / 60;
        int second = remainedTime % 60;

        timeText.text = string.Format("{0:00}:{1:00}", minute, second);
    }
    public void CancelCraft()
    {
        for (int i = 0; i < craftingOrderCampfire.itemsInside.Count; i++)
        {
            inventory.AddItem(craftingOrderCampfire.itemsInside[i].nickName, craftingOrderCampfire.itemsInside[i].amount, true);
        }

        Destroy(gameObject);
        Destroy(craftingOrderCampfire.gameObject);
    }
}
