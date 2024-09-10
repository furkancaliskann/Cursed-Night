using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    private Death death;
    private FpsCounter fpsCounter;
    private GameManager gameManager;
    private Inventory inventory;
    private LockMovement lockMovement;
    private Minimap minimap;
    private MouseMovement mouseMovement;
    private Player player;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private SpawnManager spawnManager;
    private TimeController timeController;
    private Translations translations;

    [SerializeField] private GameObject consolePanel;
    [SerializeField] private Text consoleText;
    [SerializeField] private InputField consoleInput;
    [SerializeField] private Scrollbar consoleScrollbar;

    private List<string> cachedInputs = new List<string>();
    private int selectedInputNumber;
    private int inputCacheLength = 10;

    private int maxLineSize = 100;
    private bool resetScrollbar;

    void Awake()
    {
        death = GetComponent<Death>();
        fpsCounter = GetComponent<FpsCounter>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        minimap = GetComponent<Minimap>();
        mouseMovement = GetComponent<MouseMovement>();
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        spawnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>();
        timeController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeController>();
        translations = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Translations>();
    }
    void Update()
    {
        if (lockMovement.playerInCar) return;

        CheckInputs();
        ResetScrollbarValue();
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            OpenOrClose();
        }

        if (Input.GetKeyDown(KeyCode.Return) && consolePanel.activeInHierarchy)
        {
            Commands();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && consolePanel.activeInHierarchy)
        {
            SelectPreviousInput();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && consolePanel.activeInHierarchy)
        {
            SelectNextInput();
        }
    }
    private void OpenOrClose()
    {
        if (consolePanel.activeInHierarchy)
        {
            consolePanel.SetActive(false);
            lockMovement.Unlock();
            minimap.Open();
            fpsCounter.RemoveReason("console");
        }
        else if (!lockMovement.locked)
        {
            lockMovement.Lock();
            ClearInput();
            consolePanel.SetActive(true);
            ActivateInput();
            minimap.Close();
            fpsCounter.AddReason("console");
        }
    }
    private void Commands()
    {
        if (!consolePanel.activeInHierarchy || consoleInput.text == "") return;

        AddPreviousInput(consoleInput.text);

        string[] command = consoleInput.text.Split(' ');

        switch (command[0])
        {
            case "clear": ClearConsole(); break;
            case "quit": Application.Quit(); WriteConsole(translations.Get("QuitCommand")); break;
            
            case "additem": if (!CheckLength(command, 3)) return; inventory.AddItem(command[1], int.Parse(command[2]), true);
                WriteConsole(translations.Get("ItemAdded")); break;        
            case "energy": if (!CheckLength(command, 2)) return; playerStats.ChangeEnergyValue(float.Parse(command[1]));
                WriteConsole(translations.Get("EnergyValueChanged")); break;
            case "water": if (!CheckLength(command, 2)) return; playerStats.ChangeWaterValue(float.Parse(command[1]));
                WriteConsole(translations.Get("WaterValueChanged")); break;
            case "food": if (!CheckLength(command, 2)) return; playerStats.ChangeFoodValue(float.Parse(command[1]));
                WriteConsole(translations.Get("FoodValueChanged")); break;
            case "god": if (!CheckLength(command, 1)) return; WriteConsole(player.ChangeGodMode()); break;
            case "timespeed": if (!CheckLength(command, 2)) return; timeController.SetMultiplier(float.Parse(command[1]));
                WriteConsole(translations.Get("TimeSpeedChanged")); break;
            case "suicide": if (!CheckLength(command, 1)) return; death.StartDeathProcess();
                WriteConsole(translations.Get("SuicideCommand")); break;
            case "respawn": if (!CheckLength(command, 1)) return; player.Respawn();
                WriteConsole(translations.Get("RespawnCommand")); break;
            case "menu": if (!CheckLength(command, 1)) return; gameManager.ChangeScene("Main Menu");
                WriteConsole(translations.Get("MenuCommand")); break;
            case "airdrop": if (!CheckLength(command, 1)) return; bool result = spawnManager.SpawnPlane();
                if(result) WriteConsole(translations.Get("AirdropComing")); else WriteConsole(translations.Get("AirdropFailed")); break;

            case "showfps": if (!CheckLength(command, 2)) return;
                int value = int.Parse(command[1]); 
                if (value == 0) WriteConsole(translations.Get("FpsCounterTurnedOff")); else if (value == 1) WriteConsole(translations.Get("FpsCounterTurnedOn"));
                fpsCounter.ChangePanelSetting(value); break;

            case "minute": if (!CheckLength(command, 2)) return; timeController.SetMinute(int.Parse(command[1])); WriteConsole(translations.Get("MinuteValueUpdated")); break;
            case "hour": if (!CheckLength(command, 2)) return; timeController.SetHour(int.Parse(command[1])); WriteConsole(translations.Get("HourValueUpdated")); break;
            case "day": if (!CheckLength(command, 2)) return; timeController.SetDay(int.Parse(command[1])); WriteConsole(translations.Get("DayValueUpdated")); break;

            case "speed": if (!CheckLength(command, 2)) return; WriteConsole(translations.Get("NewRunningSpeed") + int.Parse(command[1])); playerMovement.ChangeRunSpeed(int.Parse(command[1])); break;
            case "sensitivity": if (!CheckLength(command, 2)) return; mouseMovement.ChangeSensitivityWithConsole(float.Parse(command[1], NumberStyles.Float, CultureInfo.InvariantCulture));
                WriteConsole(translations.Get("SensitivityChanged")); break;
            default: WriteConsole(translations.Get("UnknownCommand")); break;
        }
        
        ClearInput();
        ActivateInput();
    }
    private bool CheckLength(string[] command, int length)
    {
        if (command.Length != length)
        {
            SendCommandError();
            return false;
        }       
        else
            return true;
    }
    private void AddPreviousInput(string input)
    {
        cachedInputs.Add(input);

        if(cachedInputs.Count > inputCacheLength)
        {
            int deleteCount = cachedInputs.Count - inputCacheLength;

            for (int i = 0; i < deleteCount; i++)
            {
                cachedInputs.RemoveAt(0);
            }
        }

        selectedInputNumber = cachedInputs.Count;
    }
    private void SelectPreviousInput()
    {
        if (cachedInputs.Count <= 0 || selectedInputNumber < 0) return; 

        if(selectedInputNumber >= 1)
            selectedInputNumber--;

        consoleInput.text = cachedInputs[selectedInputNumber];
        consoleInput.MoveTextEnd(false);
    }
    private void SelectNextInput()
    {
        if (selectedInputNumber >= cachedInputs.Count) return;

        if (selectedInputNumber < cachedInputs.Count - 1)
            selectedInputNumber++;

        consoleInput.text = cachedInputs[selectedInputNumber];        
        consoleInput.MoveTextEnd(false);
    }
    private void ClearInput()
    {
        consoleInput.text = string.Empty;
    }
    private void ActivateInput()
    {
        consoleInput.ActivateInputField();
    }
    private void WriteConsole(string text)
    {
        consoleText.text += DateTime.Now + " -- " + text + "\n";
        CheckLineSize();
        resetScrollbar = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(consoleText.GetComponent<RectTransform>());
    }
    private void ClearConsole()
    {
        consoleText.text = "";
    }
    private void SendCommandError()
    {
        WriteConsole(translations.Get("WrongCommandUsage"));
        ClearInput();
        ActivateInput();
    }
    private void CheckLineSize()
    {
        string[] lines = consoleText.text.Split('\n');

        if (lines.Length > maxLineSize)
        {
            ClearConsole();
            for (int i = lines.Length - maxLineSize; i < lines.Length; i++)
            {
                if (lines[i] != "")
                    WriteConsole(lines[i]);
            }
        }
    }
    private void ResetScrollbarValue()
    {
        if (!resetScrollbar || consoleScrollbar.value == 0) return;

        consoleScrollbar.value = 0;
        if(consoleScrollbar.value == 0) resetScrollbar = false;
    }
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string newString = "[" + type + "] : " + logString;

        if (type == LogType.Exception)
        {
            newString += "\n" + stackTrace;
        }

        WriteConsole(newString);
    }
}
