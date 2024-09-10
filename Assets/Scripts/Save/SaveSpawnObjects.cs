using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSpawnObjects : MonoBehaviour
{
    private SaveManager saveManager;
    private SpawnManager spawnManager;

    public bool allObjectsLoaded {  get; private set; }

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
        spawnManager = GetComponent<SpawnManager>();
    }
    private void SaveTree()
    {
        SaveTree saveTree = new SaveTree();

        for (int i = 0; i < spawnManager.treeList.Count; i++)
        {
            TreeCode treeCode = spawnManager.treeList[i].GetComponent<TreeCode>();

            saveTree.position.Add(spawnManager.treeList[i].transform.position);
            saveTree.health.Add(treeCode.health);
            saveTree.type.Add(treeCode.type);
            saveTree.prefabModel.Add(treeCode.prefabModel);
        }

        StartCoroutine(saveManager.Save(saveTree, SaveType.Tree));
    }
    private void LoadTree()
    {
        SaveTree saveTree = JsonUtility.FromJson<SaveTree>(saveManager.Load(SaveType.Tree));
        if (saveTree == null) return;

        for (int i = 0; i < saveTree.position.Count; i++)
        {
            spawnManager.SpawnTreeWithType(saveTree.position[i], saveTree.type[i],
                saveTree.prefabModel[i], saveTree.health[i]);
        }
    }
    private void SaveRock()
    {
        SaveRock saveRock = new SaveRock();

        for (int i = 0; i < spawnManager.rockList.Count; i++)
        {
            Rock rock = spawnManager.rockList[i].GetComponent<Rock>();

            saveRock.position.Add(spawnManager.rockList[i].transform.position);
            saveRock.health.Add(rock.health);
            saveRock.type.Add(rock.rockType);
            saveRock.prefabModel.Add(rock.prefabModel);
        }

        StartCoroutine(saveManager.Save(saveRock, SaveType.Rock));
    }
    private void LoadRock()
    {
        SaveRock saveRock = JsonUtility.FromJson<SaveRock>(saveManager.Load(SaveType.Rock));
        if (saveRock == null) return;

        for (int i = 0; i < saveRock.position.Count; i++)
        {
            spawnManager.SpawnRockWithType(saveRock.position[i], saveRock.type[i],
                saveRock.prefabModel[i], saveRock.health[i]);
        }
    }
    private void SaveCollectable()
    {
        SaveCollectable saveCollectable = new SaveCollectable();

        for (int i = 0; i < spawnManager.collectableList.Count; i++)
        {
            CollectableItem collectableItem = spawnManager.collectableList[i].GetComponent<CollectableItem>();

            saveCollectable.position.Add(spawnManager.collectableList[i].transform.position);
            saveCollectable.type.Add(collectableItem.type);
        }

        StartCoroutine(saveManager.Save(saveCollectable, SaveType.Collectable));
    }
    private void LoadCollectable()
    {
        SaveCollectable saveCollectable = JsonUtility.FromJson<SaveCollectable>(saveManager.Load(SaveType.Collectable));
        if (saveCollectable == null) return;

        for (int i = 0; i < saveCollectable.position.Count; i++)
        {
            spawnManager.SpawnCollectableWithType(saveCollectable.position[i], saveCollectable.type[i]);
        }
    }
    public void SaveAll()
    {
        SaveTree();
        SaveRock();
        SaveCollectable();
    }
    public void LoadAll()
    {
        LoadTree();
        LoadRock();
        LoadCollectable();
        StartCoroutine(ChangeObjectsLoadedState());
    }
    public IEnumerator ChangeObjectsLoadedState()
    {
        yield return new WaitForSeconds(0.5f);
        allObjectsLoaded = true;
    }
}

[System.Serializable]
public class SaveTree
{
    public List<Vector3> position = new List<Vector3>();
    public List<int> health = new List<int>();
    public List<TreeType> type = new List<TreeType>();
    public List<int> prefabModel = new List<int>(); // Prefab
}

[System.Serializable]
public class SaveRock
{
    public List<Vector3> position = new List<Vector3>();
    public List<int> health = new List<int>();
    public List<RockType> type = new List<RockType>();
    public List<int> prefabModel = new List<int>(); // Prefab
}

[System.Serializable]
public class SaveCollectable
{
    public List<Vector3> position = new List<Vector3>();
    public List<CollectableType> type = new List<CollectableType>();
}
