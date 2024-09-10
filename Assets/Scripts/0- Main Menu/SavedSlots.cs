using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SavedSlots : MonoBehaviour
{
    private Translations translations;

    private string directory;
    private bool save1Full, save2Full, save3Full;
    [SerializeField] private Button save1, save2, save3;
    [SerializeField] private Button deleteButton, loadButton, newGameButton;

    public int selectedNumber {  get; private set; }

    void Awake()
    {
        directory = Application.persistentDataPath + "/Saved Games/";
        translations = GetComponent<Translations>();
    }
    void Update()
    {
        CheckSaveFiles();
    }
    private void CheckSaveFiles()
    {
        if (Directory.Exists(directory + "Save1"))
        {
            save1.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 1\n(" + translations.Get("SavedSlotFull") + ")";

            save1Full = true;
        }
        else
        {
            save1.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 1\n(" + translations.Get("SavedSlotEmpty") + ")";

            save1Full = false;
        }
        
        if (Directory.Exists(directory + "Save2"))
        {
            save2.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 2\n(" + translations.Get("SavedSlotFull") + ")";

            save2Full = true;
        }
        else
        {
            save2.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 2\n(" + translations.Get("SavedSlotEmpty") + ")";

            save2Full = false;
        }

        if (Directory.Exists(directory + "Save3"))
        {
            save3.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 3\n(" + translations.Get("SavedSlotFull") + ")";
            
            save3Full = true;
        }
        else
        {
            save3.GetComponentInChildren<Text>().text =
                translations.Get("SavedSlotSave") + " 3\n(" + translations.Get("SavedSlotEmpty") + ")";

            save3Full = false;
        }     
    }
    public void SelectSave(int number)
    {
        selectedNumber = number;

        if(number == 1)
        {
            save1.interactable = false;
            save2.interactable = true; 
            save3.interactable = true;

            if(save1Full)
            {
                deleteButton.interactable = true;
                loadButton.interactable = true;
                newGameButton.interactable = false;
            }
            else
            {
                deleteButton.interactable = false;
                loadButton.interactable = false;
                newGameButton.interactable = true;
            }

        }
        else if (number == 2)
        {
            save1.interactable = true;
            save2.interactable = false;
            save3.interactable = true;

            if (save2Full)
            {
                deleteButton.interactable = true;
                loadButton.interactable = true;
                newGameButton.interactable = false;
            }
            else
            {
                deleteButton.interactable = false;
                loadButton.interactable = false;
                newGameButton.interactable = true;
            }
        }
        else if (number == 3)
        {
            save1.interactable = true;
            save2.interactable = true;
            save3.interactable = false;

            if (save3Full)
            {
                deleteButton.interactable = true;
                loadButton.interactable = true;
                newGameButton.interactable = false;
            }
            else
            {
                deleteButton.interactable = false;
                loadButton.interactable = false;
                newGameButton.interactable = true;
            }
        }

    }
    public void ResetButtons()
    {
        save1.interactable = true;
        save2.interactable = true;
        save3.interactable = true;
        deleteButton.interactable = false;
        loadButton.interactable = false;
        newGameButton.interactable = false;
    }
    public void DeleteSave()
    {
        string saveName = "Save" + selectedNumber;

        if (!Directory.Exists(directory + saveName)) return;

        Directory.Delete(directory + saveName, true);

        CheckSaveFiles();
        ResetButtons();
    }

}
