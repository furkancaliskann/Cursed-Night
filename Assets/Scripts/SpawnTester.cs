using UnityEngine;

public class SpawnTester : MonoBehaviour
{
    private OptimizationManager optimizationManager;
    private SpawnManager spawnManager;

    private GameObject prefab;
    private int health;
    private SpawnType spawnType;
    private int prefabModel;

    void Start()
    {
        Invoke(nameof(Spawn), 0.1f);
    }
    public void SetPrefab(SpawnManager spawnManager, SpawnType spawnType, GameObject prefab, int prefabModel, int health)
    {
        this.spawnManager = spawnManager;
        optimizationManager = spawnManager.GetComponent<OptimizationManager>();
        this.spawnType = spawnType;
        this.prefab = prefab;
        this.health = health;
        this.prefabModel = prefabModel;
    }
    private void Spawn()
    {
        GameObject spawnedObject = Instantiate(prefab,transform.position,Quaternion.identity);

        switch(spawnType)
        {
            case SpawnType.Tree: 
                spawnedObject.GetComponent<TreeCode>().SetVariables(spawnManager, prefabModel, health);
                spawnManager.AddTree(spawnedObject); break;
            case SpawnType.Rock:
                spawnedObject.GetComponent<Rock>().SetVariables(spawnManager, prefabModel, health);
                spawnManager.AddRock(spawnedObject); break;
            case SpawnType.Collectable:
                spawnManager.AddCollectable(spawnedObject); break;
        }

        optimizationManager.AddList(spawnedObject);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

public enum SpawnType
{
    Tree,
    Rock,
    Collectable
}
