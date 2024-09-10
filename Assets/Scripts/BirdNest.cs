using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdNest : MonoBehaviour
{
    void Start()
    {
        GetComponent<Loot>().FillItems();
    }
}
