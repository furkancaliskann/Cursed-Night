using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryChange : MonoBehaviour
{
    List <GameObject> changeItems = new List <GameObject> ();
    [SerializeField] private GameObject changeItemPrefab;
    [SerializeField] private GameObject changeItemPanel;

    private float counter = 2.5f;
    private float counterMax = 2.5f;

    void Update()
    {
        CheckChangeItems();
    }
    public void OpenPanel()
    {
        if(!changeItemPanel.activeInHierarchy)
            changeItemPanel.SetActive (true);
    }
    public void ClosePanel()
    {
        if (changeItemPanel.activeInHierarchy)
            changeItemPanel.SetActive(false);
    }
    private void CheckChangeItems()
    {
        if (changeItems.Count > 0)
        {
            if (counter > 0)
                counter -= Time.deltaTime;
            else
            {
                counter = counterMax;
                Delete();
            }
        }
        else counter = counterMax;
    }
    public void AddChangeItem(Sprite itemImage, int changeAmount, int totalAmount)
    {
        if(changeItems.Count >= 4)
        {
            Destroy(changeItems[0]);
            changeItems.RemoveAt(0);
        }

        GameObject myObject = Instantiate(changeItemPrefab, changeItemPanel.transform);
        myObject.GetComponentsInChildren<Image>()[1].sprite = itemImage;

        if (changeAmount > 0)
            myObject.GetComponentInChildren<Text>().text = "+" + changeAmount + " (" + totalAmount + ")";
        else
            myObject.GetComponentInChildren<Text>().text = changeAmount + " (" + totalAmount + ")";

        changeItems.Add(myObject);
    }
    private void Delete()
    {
        Destroy(changeItems[0]);
        changeItems.RemoveAt(0);
    }
}
