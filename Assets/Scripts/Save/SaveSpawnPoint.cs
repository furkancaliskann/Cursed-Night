using UnityEngine;

public class SaveSpawnPoint : MonoBehaviour
{
    private SaveManager saveManager;

    public Vector3 registeredSpawnPoint;
    public Vector3 registeredSpawnRotation;

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
    }
    public void ChangePoint(Vector3 position, Vector3 rotation)
    {
        registeredSpawnPoint = position;
        registeredSpawnRotation = rotation;
    }
    public void ClearPoint()
    {
        registeredSpawnPoint = Vector3.zero;
        registeredSpawnRotation = Vector3.zero;
    }
    public void SaveSpawn()
    {
        SaveSpawn saveSpawn = new SaveSpawn();
        saveSpawn.position = registeredSpawnPoint;
        saveSpawn.rotation = registeredSpawnRotation;

        StartCoroutine(saveManager.Save(saveSpawn, SaveType.SpawnPoint));
    }
    public void LoadSpawn()
    {
        SaveSpawn saveSpawn = JsonUtility.FromJson<SaveSpawn>(saveManager.Load(SaveType.SpawnPoint));
        if (saveSpawn == null) return;

        registeredSpawnPoint = saveSpawn.position;
        registeredSpawnRotation = saveSpawn.rotation;
    }
}

public class SaveSpawn
{
    public Vector3 position;
    public Vector3 rotation;
}
