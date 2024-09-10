using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Game Manager Saves
    private EventManager eventManager;
    private LootManager lootManager;
    private RadioManager radioManager;
    private SaveBackpacks saveBackpacks;
    private SaveBuildings saveBuildings;
    private SaveCars saveCars;
    private SaveSpawnObjects saveSpawnObjects;
    private SaveSpawnPoint saveSpawnPoint;
    private TimeController timeController;
    
    // Player Saves
    private SavePlayer savePlayer;

    private GameManager gameManager;
    private GameOptions gameOptions;

    public string gameName {  get; private set; }

    private Dictionary<SaveType, string> typeToPath = new Dictionary<SaveType, string>
    {
        { SaveType.PlayerPosition, "PlayerPosition.json" },
        { SaveType.Time, "Time.json" },
        { SaveType.PlayerStats, "PlayerStats.json" },
        { SaveType.Inventory, "Inventory.json" },
        { SaveType.SelectedSlot, "SelectedSlot.json" },
        { SaveType.HealthNotification, "HealthNotification.json" },
        { SaveType.Tree, "Tree.json" },
        { SaveType.Rock, "Rock.json" },
        { SaveType.Collectable, "Collectable.json" },
        { SaveType.Block, "Block.json" },
        { SaveType.Car, "Car.json" },
        { SaveType.Backpack, "Backpack.json" },
        { SaveType.Loot, "Loot.json" },
        { SaveType.OtherLoot, "OtherLoot.json" },
        { SaveType.CraftingOrder, "CraftingOrder.json" },
        { SaveType.Radio, "Radio.json" },
        { SaveType.Event, "Event.json" },
        { SaveType.SpawnPoint, "SpawnPoint.json" },
    };

    private float saveCounter = 30f;

    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        lootManager = GetComponent<LootManager>();
        radioManager = GetComponent<RadioManager>();
        saveBackpacks = GetComponent<SaveBackpacks>();
        saveBuildings = GetComponent<SaveBuildings>();
        saveCars = GetComponent<SaveCars>();
        saveSpawnObjects = GetComponent<SaveSpawnObjects>();
        saveSpawnPoint = GetComponent<SaveSpawnPoint>();
        timeController = GetComponent<TimeController>();

        savePlayer = GetComponent<SavePlayer>();
    }
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        GameObject gameOptionsObject = CheckIfEditor();
        if (gameOptionsObject == null) return;

        gameOptions = gameOptionsObject.GetComponent<GameOptions>();

        gameName = "Save" + gameOptions.slotNumber;

        CheckDirectory();

        Invoke(nameof(LoadAll), 0.1f);

        InvokeRepeating(nameof(SaveAll), saveCounter, saveCounter);
    }
    private GameObject CheckIfEditor()
    {
        GameObject gameOptionsObject = GameObject.FindGameObjectWithTag("GameOptions");

        if (gameOptionsObject == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                gameManager.SpawnPlayer(null);
            else
            {
                player.GetComponent<PlayerStats>().MaximizeAllStats();
                player.GetComponent<Inventory>().GiveStartingItems();
                timeController.SetHour(9);
            }

            StartCoroutine(saveSpawnObjects.ChangeObjectsLoadedState());
            return null;
        }

        return gameOptionsObject;
    }
    private void CheckDirectory()
    {
        if (gameName == "") return;

        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/";

        if (!Directory.Exists(directory))
        {
            Debug.Log("Created new save folder.");
            Directory.CreateDirectory(directory);
            gameManager.SpawnPlayerWithAutoDestroyOthers();
        }
        else
        {
            Debug.Log("Save file found.");
            savePlayer.Load(gameManager.SpawnLoadedPlayer());
        }
    }
    private IEnumerator WriteToFileAsync(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.Write(content);
            writer.Close();
        }
        yield return null;
    }
    public IEnumerator Save(object save, SaveType saveType)
    {
        if (gameName == "") yield break;

        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/";

        string filePath = Path.Combine(directory, typeToPath[saveType]);
        string pos = JsonUtility.ToJson(save);
        yield return WriteToFileAsync(filePath, pos);
    }
    public string Load(SaveType saveType)
    {
        if (gameName == "") return null;

        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/";

        string filePath = Path.Combine(directory, typeToPath[saveType]);

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else return null;
    }
    public void RemovePlayerSave()
    {
        if (gameName == "") return;
        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/";

        if(File.Exists(directory + typeToPath[SaveType.PlayerPosition]))
            File.Delete(directory + typeToPath[SaveType.PlayerPosition]);

        if (File.Exists(directory + typeToPath[SaveType.PlayerStats]))
            File.Delete(directory + typeToPath[SaveType.PlayerStats]);

        if (File.Exists(directory + typeToPath[SaveType.Inventory]))
            File.Delete(directory + typeToPath[SaveType.Inventory]);

        if (File.Exists(directory + typeToPath[SaveType.SelectedSlot]))
            File.Delete(directory + typeToPath[SaveType.SelectedSlot]);

        if (File.Exists(directory + typeToPath[SaveType.HealthNotification]))
            File.Delete(directory + typeToPath[SaveType.HealthNotification]);

        if (File.Exists(directory + typeToPath[SaveType.CraftingOrder]))
            File.Delete(directory + typeToPath[SaveType.CraftingOrder]);
    }
    public void SaveAll()
    {
        if (gameName == null) return;

        savePlayer.Save(); // 5

        eventManager.SaveEvent();
        lootManager.SaveAll();
        radioManager.SaveRadio();
        saveBackpacks.SaveBackpack();
        saveBuildings.SaveBlock(); // 1
        saveCars.SaveCar();
        saveSpawnObjects.SaveAll(); // 3
        saveSpawnPoint.SaveSpawn();

        timeController.SaveTime(); // 1
    }
    public void LoadAll()
    {
        if (gameName == null) return;

        eventManager.LoadEvent();
        timeController.LoadTime();
        lootManager.LoadAll();
        radioManager.LoadRadio();
        saveBackpacks.LoadBackpack();
        saveBuildings.LoadBlock();
        saveCars.LoadCar();
        saveSpawnObjects.LoadAll(); 
        saveSpawnPoint.LoadSpawn();
    }
    private void OnApplicationQuit()
    {
        if (gameName == null) return;

        SaveAll();
    }
}

public enum SaveType
{
    PlayerPosition,
    Time,
    PlayerStats,
    Inventory,
    SelectedSlot,
    HealthNotification,
    Tree,
    Rock,
    Collectable,
    Block,
    Car,
    Backpack,
    Loot,
    OtherLoot,
    CraftingOrder,
    Radio,
    Event,
    SpawnPoint
}
