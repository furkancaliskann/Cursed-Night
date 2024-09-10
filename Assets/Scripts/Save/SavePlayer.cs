using UnityEngine;

public class SavePlayer : MonoBehaviour
{
    private SaveManager saveManager;
    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
    }
    public void Load(GameObject playerObject)
    {
        playerObject.GetComponent<HealthNotification>().LoadHealthNotification();
        playerObject.GetComponent<Inventory>().LoadInventory();
        playerObject.GetComponent<Player>().LoadPlayerPosition();
        playerObject.GetComponent<PlayerItems>().LoadSelectedSlot();
        playerObject.GetComponent<PlayerStats>().LoadStats();
        playerObject.GetComponent<CraftingCanvas>().LoadCraftingOrder();
    }
    public void Save()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            saveManager.RemovePlayerSave();
            return;
        }

        playerObject.GetComponent<HealthNotification>().SaveHealthNotification();
        playerObject.GetComponent<Inventory>().SaveInventory();
        playerObject.GetComponent<Player>().SavePlayerPosition();
        playerObject.GetComponent<PlayerItems>().SaveSelectedSlot();
        playerObject.GetComponent<PlayerStats>().SaveStats();
        playerObject.GetComponent<CraftingCanvas>().SaveCraftingOrder();
    }
}
