using System.Collections;
using UnityEngine;

public class MineSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] locations;
    [SerializeField] private GameObject minePrefab;
    private GameObject[] spawnedMines;

    void Start()
    {
        spawnedMines = new GameObject[locations.Length];
        InvokeRepeating(nameof(Check), 0f, 450f);
    }
    private void SpawnMine(int id, GameObject location)
    {
        GameObject myObject = Instantiate(minePrefab, location.transform.position, Quaternion.identity);

        //myObject.transform.localPosition = minePrefab.transform.localPosition;
        spawnedMines[id] = myObject;
        return;
    }
    private void Check()
    {
        StartCoroutine(CheckNumerator());
    }
    private IEnumerator CheckNumerator()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < locations.Length; i++)
        {
            if (spawnedMines[i] == null)
            {
                SpawnMine(i, locations[i]);
            }
        }
    }
}
