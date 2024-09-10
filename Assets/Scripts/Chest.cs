using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    void Awake()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        for (int i = 0; i < 35; i++)
        {
            items.Add(null);
        }
    }
}
