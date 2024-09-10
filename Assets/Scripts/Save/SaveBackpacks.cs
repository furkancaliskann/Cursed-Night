using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBackpacks : MonoBehaviour
{
    private SaveManager saveManager;

    [SerializeField] private GameObject backpackPrefab;
    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
    }
    public void SaveBackpack()
    {
        SaveBackpack saveBackpack = new SaveBackpack();

        GameObject[] backpacks = GameObject.FindGameObjectsWithTag("Backpack");

        for (int i = 0; i < backpacks.Length; i++)
        {
            Backpack backpack = backpacks[i].GetComponent<Backpack>();

            saveBackpack.position.Add(backpacks[i].transform.position);
            saveBackpack.rotation.Add(backpacks[i].transform.rotation);
            saveBackpack.destroyCounter.Add(backpack.destroyCounter);

            SaveInventory saveChest = new SaveInventory();

            for (int j = 0; j < backpack.items.Count; j++)
            {
                if (backpack.items[j] != null)
                {
                    saveChest.nickName.Add(backpack.items[j].nickName);
                    saveChest.amount.Add(backpack.items[j].amount);
                    saveChest.durability.Add(backpack.items[j].durability);
                    saveChest.ammoInside.Add(backpack.items[j].ammoInside);
                }
                else
                {
                    saveChest.nickName.Add(null);
                    saveChest.amount.Add(0);
                    saveChest.durability.Add(0);
                    saveChest.ammoInside.Add(0);
                }
            }

            saveBackpack.chest.Add(saveChest);
        }

        StartCoroutine(saveManager.Save(saveBackpack, SaveType.Backpack));
    }
    public void LoadBackpack()
    {
        SaveBackpack saveBackpack = JsonUtility.FromJson<SaveBackpack>(saveManager.Load(SaveType.Backpack));

        if (saveBackpack == null) return;

        for (int i = 0; i < saveBackpack.position.Count; i++)
        {
            GameObject backpackObject = Instantiate(backpackPrefab, saveBackpack.position[i], saveBackpack.rotation[i]);    
            Backpack backpack = backpackObject.GetComponent<Backpack>();

            backpack.LoadItems(saveBackpack.destroyCounter[i], saveBackpack.chest[i]);
        }
    }
}

[System.Serializable]
public class SaveBackpack
{
    public List<Vector3> position = new List<Vector3>();
    public List<Quaternion> rotation = new List<Quaternion>();
    public List<float> destroyCounter = new List<float>();
    public List<SaveInventory> chest = new List<SaveInventory>();
}
