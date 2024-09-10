using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SaveSpawnPoint saveSpawnPoint;
    private Terrain terrain;
    [SerializeField] private GameObject playerPrefab;

    void Awake()
    {
        saveSpawnPoint = GetComponent<SaveSpawnPoint>();
        terrain = Terrain.activeTerrain;
    }
    public void SpawnPlayer(GameObject oldPlayer)
    {
        if(oldPlayer != null)
            Destroy(oldPlayer);

        GameObject newPlayer = Instantiate(playerPrefab, GetRandomSpawnPos(), Quaternion.identity);
        newPlayer.GetComponent<PlayerStats>().MaximizeAllStats();
        newPlayer.GetComponent<Inventory>().GiveStartingItems();
    }
    public void SpawnWithRegisteredPoint(GameObject oldPlayer)
    {
        if (saveSpawnPoint.registeredSpawnPoint == Vector3.zero) return;

        if (oldPlayer != null)
            Destroy(oldPlayer);

        GameObject newPlayer = Instantiate(playerPrefab, saveSpawnPoint.registeredSpawnPoint + new Vector3(0,1,0), Quaternion.Euler(saveSpawnPoint.registeredSpawnRotation));
        newPlayer.GetComponent<PlayerStats>().MaximizeAllStats();
        newPlayer.GetComponent<Inventory>().GiveStartingItems();
    }
    public void SpawnPlayerWithAutoDestroyOthers()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < player.Length; i++)
        {
            Destroy(player[i]);
        }

        GameObject newPlayer = Instantiate(playerPrefab, GetRandomSpawnPos(), Quaternion.identity);
        newPlayer.GetComponent<PlayerStats>().MaximizeAllStats();
        newPlayer.GetComponent<Inventory>().GiveStartingItems();
    }
    public GameObject SpawnLoadedPlayer()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < player.Length; i++)
        {
            Destroy(player[i]);
        }

        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
    public Vector3 GetRandomSpawnPos()
    {
        float x = Random.Range(10, terrain.terrainData.size.x - 10);
        float z = Random.Range(10, terrain.terrainData.size.x - 10);

        Vector3 scale = terrain.terrainData.heightmapScale;
        float y = terrain.terrainData.GetHeight(Mathf.RoundToInt(x / scale.x), Mathf.RoundToInt(z / scale.z));

        return new Vector3(x, y + 2, z);
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
