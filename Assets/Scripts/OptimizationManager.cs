using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizationManager : MonoBehaviour
{
    private FpsCounter fpsCounter;
    private SpawnManager spawnManager;

    private GameObject player;
    private List<GameObject> list = new List<GameObject>();
    private bool optimized;
    private Vector3 lastOptimizedPos;

    void Awake()
    {
        spawnManager = GetComponent<SpawnManager>(); 
    }
    void Update()
    {
        StartCoroutine(nameof(StartOptimization));
    }
    public void AddList(GameObject myObject)
    {
        if(!list.Contains(myObject))
        list.Add(myObject);
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    private int GetWaitTime()
    {
        int value;

        if (fpsCounter.lastFpsValue <= 30) value = 240;
        else if (fpsCounter.lastFpsValue > 30 && fpsCounter.lastFpsValue <= 60) value = 120;
        else if (fpsCounter.lastFpsValue > 60 && fpsCounter.lastFpsValue <= 90) value = 90;
        else if (fpsCounter.lastFpsValue > 90 && fpsCounter.lastFpsValue <= 120) value = 60;
        else if (fpsCounter.lastFpsValue > 120 && fpsCounter.lastFpsValue <= 180) value = 50;
        else if (fpsCounter.lastFpsValue > 180 && fpsCounter.lastFpsValue <= 240) value = 40;
        else if (fpsCounter.lastFpsValue > 240 && fpsCounter.lastFpsValue <= 300) value = 35;
        else if (fpsCounter.lastFpsValue > 300 && fpsCounter.lastFpsValue <= 400) value = 30;
        else if (fpsCounter.lastFpsValue > 400 && fpsCounter.lastFpsValue <= 500) value = 20;
        else value = 15;

        if (list.Count < 15000) return value;
        else
        {
            value *= list.Count / 15000;
            return value;
        }  
    }
    private IEnumerator StartOptimization()
    {
        if (player == null || list.Count == 0 || !spawnManager.spawnCompleted || optimized) yield break;
        if (Vector3.Distance(player.transform.position, lastOptimizedPos) < 25) yield break;

        optimized = true;
        lastOptimizedPos = player.transform.position;

        fpsCounter = player.GetComponent<FpsCounter>();
        int index = -1;
        int waitTime = GetWaitTime();

        while (index < list.Count - 1)
        {
            if (player == null) yield break;
            index++;
            waitTime--;

            if(index == list.Count - 1) optimized = false;

            if(waitTime <= 0)
            {
                yield return null;
                waitTime = GetWaitTime();
            }
            

            if (list[index] == null)
            {
                list.RemoveAt(index);
                continue;
            }

            if (Vector3.Distance(player.transform.position, list[index].transform.position) > 450)
            {
                if (list[index].activeInHierarchy)
                    list[index].SetActive(false);
            }
            else
            {
                if (!list[index].activeInHierarchy)
                    list[index].SetActive(true);
            }   
        }

        yield return new WaitForSeconds(0.5f);
    } 
}
