using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class News : MonoBehaviour
{
    [SerializeField] private Text newsHeader;
    [SerializeField] private Text newsContent;

    [SerializeField] private Text creditsHeader;
    [SerializeField] private Text creditsContent;

    [SerializeField] private Text developerNoteHeader;
    [SerializeField] private Text developerNoteContent;

    private TextAsset newsHeaderEN, newsHeaderTR, newsContentEN, newsContentTR;
    private TextAsset creditsHeaderEN, creditsHeaderTR, creditsContentEN, creditsContentTR;
    private TextAsset developerNoteHeaderEN, developerNoteHeaderTR, developerNoteContentEN, developerNoteContentTR;

    private string currentLanguage = "";

    void Awake()
    {
        newsHeaderEN = Resources.Load<TextAsset>("Main Menu/News/HeaderEN");
        newsHeaderTR = Resources.Load<TextAsset>("Main Menu/News/HeaderTR");
        newsContentEN = Resources.Load<TextAsset>("Main Menu/News/ContentEN");
        newsContentTR = Resources.Load<TextAsset>("Main Menu/News/ContentTR");

        creditsHeaderEN = Resources.Load<TextAsset>("Main Menu/Credits/HeaderEN");
        creditsHeaderTR = Resources.Load<TextAsset>("Main Menu/Credits/HeaderTR");
        creditsContentEN = Resources.Load<TextAsset>("Main Menu/Credits/ContentEN");
        creditsContentTR = Resources.Load<TextAsset>("Main Menu/Credits/ContentTR");

        developerNoteHeaderEN = Resources.Load<TextAsset>("Main Menu/Developer Note/HeaderEN");
        developerNoteHeaderTR = Resources.Load<TextAsset>("Main Menu/Developer Note/HeaderTR");
        developerNoteContentEN = Resources.Load<TextAsset>("Main Menu/Developer Note/ContentEN");
        developerNoteContentTR = Resources.Load<TextAsset>("Main Menu/Developer Note/ContentTR");

        currentLanguage = LanguageSettings.language;
        RefreshAll();
    }
    void Update()
    {
        CheckLanguage();
    }
    private void CheckLanguage()
    {
        if (LanguageSettings.language == currentLanguage) return;

        currentLanguage = LanguageSettings.language;
        RefreshAll();
    }
    private void RefreshAll()
    {
        newsHeader.text = string.Empty;
        newsContent.text = string.Empty;

        creditsHeader.text = string.Empty;
        creditsContent.text = string.Empty;

        developerNoteHeader.text = string.Empty;
        developerNoteContent.text = string.Empty;

        if(currentLanguage == "tr")
        {
            newsHeader.text = newsHeaderTR.text;
            newsContent.text = newsContentTR.text;

            creditsHeader.text = creditsHeaderTR.text;
            creditsContent.text = creditsContentTR.text;

            developerNoteHeader.text = developerNoteHeaderTR.text;
            developerNoteContent.text = developerNoteContentTR.text;
        }
        else
        {
            newsHeader.text = newsHeaderEN.text;
            newsContent.text = newsContentEN.text;

            creditsHeader.text = creditsHeaderEN.text;
            creditsContent.text = creditsContentEN.text;

            developerNoteHeader.text = developerNoteHeaderEN.text;
            developerNoteContent.text = developerNoteContentEN.text;
        }
    }
}
