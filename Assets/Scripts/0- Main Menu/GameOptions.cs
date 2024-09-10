using System.Collections;
using System.IO;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public int slotNumber;
    public Difficulty difficulty;
    public int lootAbundance;
    public int blockDamage;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        slotNumber = -1;
    }
    public void SetOptions(int slotNumber, Difficulty difficulty, int lootAbundance, int blockDamage)
    {
        this.slotNumber = slotNumber;
        this.difficulty = difficulty;
        this.lootAbundance = lootAbundance;
        this.blockDamage = blockDamage;

        SaveGameOptions("Save" + slotNumber);
    }
    public void SaveGameOptions(string gameName)
    {
        SaveGameOptions saveGameOptions = new SaveGameOptions();
        saveGameOptions.slotNumber = slotNumber;
        saveGameOptions.difficulty = difficulty;
        saveGameOptions.lootAbundance = lootAbundance;
        saveGameOptions.blockDamage = blockDamage;

        StartCoroutine(Save(saveGameOptions, gameName));
    }
    public void LoadGameOptions(string gameName)
    {
        SaveGameOptions loadedOptions = JsonUtility.FromJson<SaveGameOptions>(Load(gameName));
        if (loadedOptions == null) return;

        slotNumber = loadedOptions.slotNumber;
        difficulty = loadedOptions.difficulty;
        lootAbundance = loadedOptions.lootAbundance;
        blockDamage = loadedOptions.blockDamage;
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
    public IEnumerator Save(object save, string gameName)
    {
        if (gameName == "") yield break;

        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/" + "GameOptions.json";

        string pos = JsonUtility.ToJson(save);
        yield return WriteToFileAsync(directory, pos);
    }
    public string Load(string gameName)
    {
        if (gameName == "") return null;

        string directory = Application.persistentDataPath + "/Saved Games/" + gameName + "/" + "GameOptions.json";

        if (File.Exists(directory))
        {
            return File.ReadAllText(directory);
        }
        else return null;
    }
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
}

[System.Serializable]
public class SaveGameOptions
{
    public int slotNumber;
    public Difficulty difficulty;
    public int lootAbundance;
    public int blockDamage;
}