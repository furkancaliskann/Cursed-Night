using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherLootDestroyCounter : MonoBehaviour
{
    private LootManager lootManager;

    public float destroyCounter {  get; private set; }

    void Awake()
    {
        lootManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootManager>();

        destroyCounter = 10f;
    }
    void Update()
    {
        CheckCounter();
    }
    private void CheckCounter()
    {
        if (destroyCounter > 0) destroyCounter -= Time.deltaTime;
        else
        {
            lootManager.RemoveOtherLoot(gameObject);
            Destroy(gameObject);
        }
    }
    public void SetCounter(float value)
    {
        destroyCounter = value;
    }
}
