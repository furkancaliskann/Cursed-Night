using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private SavedSlots savedSlots;

    public List<GameObject> panels = new List<GameObject>();
    [SerializeField] private GameObject gameOptionsPrefab;

    [SerializeField] private GameObject savedSlotPanel;
    [SerializeField] private GameObject gameOptionsPanel; // for new game

    [SerializeField] private Dropdown difficultyDropdown;
    [SerializeField] private Dropdown lootAbundanceDropdown;
    [SerializeField] private Dropdown blockDamageDropdown;

    void Awake()
    {
        savedSlots = GetComponent<SavedSlots>();
    }
    void Start()
    {
        DestroyGameOptions();
        OpenPanel(3);
    }
    public void OpenPanel(int panelNo)
    {
        ClosePanels();

        panels[panelNo].SetActive(true);
    }
    public void ClosePanels()
    {
        savedSlots.ResetButtons();
        gameOptionsPanel.SetActive(false);
        savedSlotPanel.SetActive(true);

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OpenGameOptionsPanel()
    {
        savedSlotPanel.SetActive(false);
        gameOptionsPanel.SetActive(true);
    }
    public void StartGame()
    {
        GameObject gameOptionObject = Instantiate(gameOptionsPrefab);
        GameOptions gameOptions = gameOptionObject.GetComponent<GameOptions>();

        Difficulty difficulty = Difficulty.Normal;
        if (difficultyDropdown.value == 0) difficulty = Difficulty.Easy;
        else if (difficultyDropdown.value == 2) difficulty = Difficulty.Hard;

        int lootAbundance;
        string lootValue = lootAbundanceDropdown.options[lootAbundanceDropdown.value].text.Replace("%", "");
        lootAbundance = int.Parse(lootValue);

        int blockDamage;
        string damageValue = blockDamageDropdown.options[blockDamageDropdown.value].text.Replace("%", "");
        blockDamage = int.Parse(damageValue);

        string directory = Application.persistentDataPath + "/Saved Games/" + "Save" + savedSlots.selectedNumber + "/";
        System.IO.Directory.CreateDirectory(directory);

        gameOptions.SetOptions(savedSlots.selectedNumber, difficulty, lootAbundance, blockDamage);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void LoadGame()
    {
        GameObject gameOptionObject = Instantiate(gameOptionsPrefab);

        gameOptionObject.GetComponent<GameOptions>().LoadGameOptions("Save" + savedSlots.selectedNumber);

        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void DestroyGameOptions()
    {
        GameObject[] options = GameObject.FindGameObjectsWithTag("GameOptions");

        for (int i = 0; i < options.Length; i++)
        {
            Destroy(options[i]);
        }
    }
}
