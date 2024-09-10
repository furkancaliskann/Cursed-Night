using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequiredMaterialSlot : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text amountText;

    public void SetSlot(Sprite imageSprite, string name, int amount, int amountOfInventory)
    {
        image.sprite = imageSprite;
        nameText.text = name;
        amountText.text = amountOfInventory.ToString() + " / " + amount.ToString();
    }
    public void ClearSlot()
    {
        image.sprite = null;
        nameText.text = string.Empty;
        amountText.text = string.Empty;
    }
}
