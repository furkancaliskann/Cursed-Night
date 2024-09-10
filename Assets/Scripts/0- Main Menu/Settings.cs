using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    List<Resolution> resolutions = new List<Resolution>();
    List<FullScreenMode> screenModes = new List<FullScreenMode>();
    List<string> qualitySettings = new List<string>();
    List<string> vsyncSettings = new List<string>();
    List<string> languageSettings = new List<string>();

    public Dropdown resolutionsDropdown;
    public Dropdown screenModesDropdown;
    public Dropdown qualitySettingsDropdown;
    public Dropdown vsyncDropdown;
    public Dropdown languageSettingsDropdown;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Text sensitivitySliderText;

    [SerializeField] private Button fpsCounterOnButton, fpsCounterOffButton;

    private FullScreenMode currentScreenMode;
    [SerializeField] private Button saveButton;

    void Start()
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false; // This is for pause menu -> main menu

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC", 1);
        currentScreenMode = Screen.fullScreenMode;

        FpsCounterButtonChanged(PlayerPrefs.GetInt("FpsCounter", 0) == 1 ? true : false);

        GetSensitivityValue();
        InsertResolutions();
        StartCoroutine(nameof(InsertScreenModes));
        InsertQualitySettings();
        InsertVSYNCSettings();
        InsertLanguageSettings();

        SetSaveButtonInteractable(false);
    }
    void GetSensitivityValue()
    {
        float sens = PlayerPrefs.GetFloat("Sensitivity", 2.0f);
        sensitivitySlider.value = sens;
        sensitivitySliderText.text = sens.ToString("F1");
    }
    public void SetSaveButtonInteractable(bool value)
    {
        saveButton.interactable = value;
    }
    void InsertResolutions()
    {
        resolutions.Clear();

        List<string> options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            float aspectRatio = (float)Screen.resolutions[i].width / Screen.resolutions[i].height;
            if (aspectRatio > 1.77f && aspectRatio < 1.78f)
            {
                string resolutionString = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;

                int roundedHertz = Mathf.RoundToInt((float)Screen.resolutions[i].refreshRateRatio.value);
                if (roundedHertz > 0)
                {
                    resolutionString += " @ " + roundedHertz + "Hz";
                }

                if (options.Contains(resolutionString)) continue;

                options.Add(resolutionString);
                resolutions.Add(Screen.resolutions[i]);

                if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                    Screen.resolutions[i].height == Screen.currentResolution.height &&
                    Mathf.RoundToInt((float)Screen.resolutions[i].refreshRateRatio.value) == Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value))
                {
                    currentIndex = i;
                }
            }
        }
        resolutionsDropdown.ClearOptions();
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentIndex;
        resolutionsDropdown.RefreshShownValue(); // Dropdown'ýn görselini güncelle
    }
    void InsertScreenModes()
    {
        screenModes.Clear();
        screenModes.Add(FullScreenMode.FullScreenWindow);
        screenModes.Add(FullScreenMode.Windowed);
        screenModes.Add(FullScreenMode.ExclusiveFullScreen);

        List<string> settingsList = new List<string>();

        int selectedValue = screenModes.IndexOf(currentScreenMode);

        if (LanguageSettings.language == "en")
        {
            settingsList.Add("Full Screen Window");
            settingsList.Add("Windowed");
            settingsList.Add("Exclusive Full Screen");
        }
        else if (LanguageSettings.language == "tr")
        {
            settingsList.Add("Tam Ekran Pencereli");
            settingsList.Add("Pencereli");
            settingsList.Add("Tam Ekran");
        }


        screenModesDropdown.ClearOptions();
        screenModesDropdown.AddOptions(settingsList);
        screenModesDropdown.value = selectedValue;
        screenModesDropdown.RefreshShownValue();
    }
    void InsertQualitySettings()
    {
        qualitySettings.Clear();

        if(LanguageSettings.language == "en")
        {
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                qualitySettings.Add(QualitySettings.names[i]);
            }
        }
        else if (LanguageSettings.language == "tr")
        {
            qualitySettings.Add("Çok Düþük");
            qualitySettings.Add("Düþük");
            qualitySettings.Add("Orta");
            qualitySettings.Add("Yüksek");
            qualitySettings.Add("Çok Yüksek");
            qualitySettings.Add("Maksimum");
        }

        List<Dropdown.OptionData> settingsList = new List<Dropdown.OptionData>();

        int selectedValue = QualitySettings.GetQualityLevel();

        for (int i = 0; i < qualitySettings.Count; i++)
        {
            Dropdown.OptionData newSetting = new Dropdown.OptionData();
            newSetting.text = qualitySettings[i];
            settingsList.Add(newSetting);
        }

        qualitySettingsDropdown.ClearOptions();
        qualitySettingsDropdown.AddOptions(settingsList);
        qualitySettingsDropdown.value = selectedValue;
        qualitySettingsDropdown.RefreshShownValue();
    }
    void InsertVSYNCSettings()
    {
        vsyncSettings.Clear();
        vsyncDropdown.ClearOptions();

        if (LanguageSettings.language == "tr") { vsyncSettings.Add("Açýk"); vsyncSettings.Add("Kapalý"); }
        else if (LanguageSettings.language == "en") { vsyncSettings.Add("On"); vsyncSettings.Add("Off"); }

        List<Dropdown.OptionData> settingList = new List<Dropdown.OptionData>();

        int selectedValue = 0;

        for (int i = 0; i < vsyncSettings.Count; i++)
        {
            Dropdown.OptionData yeniAyar = new Dropdown.OptionData();
            yeniAyar.text = vsyncSettings[i];
            settingList.Add(yeniAyar);

            if (((vsyncSettings[i] == "On" || vsyncSettings[i] == "Açýk") && QualitySettings.vSyncCount == 1) || ((vsyncSettings[i] == "Off" || vsyncSettings[i] == "Kapalý") && QualitySettings.vSyncCount == 0))
            {
                selectedValue = i;
            }
        }

        vsyncDropdown.ClearOptions();
        vsyncDropdown.AddOptions(settingList);
        vsyncDropdown.value = selectedValue;
    }
    void InsertLanguageSettings()
    {
        languageSettings.Clear();

        languageSettings.Add("English");
        languageSettings.Add("Türkçe");

        string dil = PlayerPrefs.GetString("Language", "en");

        List<Dropdown.OptionData> settingList = new List<Dropdown.OptionData>();

        int selectedValue = 0;

        for (int i = 0; i < languageSettings.Count; i++)
        {
            Dropdown.OptionData newSetting = new Dropdown.OptionData();
            newSetting.text = languageSettings[i];
            settingList.Add(newSetting);

            if ((languageSettings[i] == "English" && dil == "en") || (languageSettings[i] == "Türkçe" && dil == "tr"))
            {
                selectedValue = i;
            }
        }

        languageSettingsDropdown.ClearOptions();
        languageSettingsDropdown.AddOptions(settingList);
        languageSettingsDropdown.value = selectedValue;
    }
    public void SensitivitySliderValueChanged()
    {
        sensitivitySliderText.text = sensitivitySlider.value.ToString("F1");
    }
    public void ApplySettings()
    {
        ChangeResolution();
        ChangeScreenMode();
        ChangeGraphicQuality();
        ChangeVSYNC();
        ChangeLanguage();
        ChangeSensitivity();
        ChangeFpsCounter();

        InsertScreenModes();
        InsertQualitySettings();
        InsertVSYNCSettings();
        InsertLanguageSettings();

        SetSaveButtonInteractable(false);
    }
    void ChangeResolution()
    {
        Screen.SetResolution(resolutions[resolutionsDropdown.value].width, resolutions[resolutionsDropdown.value].height, screenModes[screenModesDropdown.value]);
    }
    void ChangeScreenMode()
    {
        Screen.fullScreenMode = screenModes[screenModesDropdown.value];
        currentScreenMode = screenModes[screenModesDropdown.value];
    }
    void ChangeGraphicQuality()
    {
        QualitySettings.SetQualityLevel(qualitySettingsDropdown.value);
    }
    void ChangeVSYNC()
    {
        if(vsyncDropdown.value == 0)
        {
            QualitySettings.vSyncCount = 1; 
            PlayerPrefs.SetInt("VSYNC", 1);
        }
        else
        {
            QualitySettings.vSyncCount = 0; 
            PlayerPrefs.SetInt("VSYNC", 0);
        }
    }
    void ChangeLanguage()
    {
        switch (languageSettingsDropdown.options[languageSettingsDropdown.value].text.ToString())
        {
            case "English": LanguageSettings.language = "en"; PlayerPrefs.SetString("Language", "en"); break;
            case "Türkçe": LanguageSettings.language = "tr"; PlayerPrefs.SetString("Language", "tr"); break;
        }
    }
    void ChangeSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
    }
    void ChangeFpsCounter()
    {
        if (!fpsCounterOnButton.interactable) PlayerPrefs.SetInt("FpsCounter", 1);
        else if (!fpsCounterOffButton.interactable) PlayerPrefs.SetInt("FpsCounter", 0);
    }
    public void FpsCounterButtonChanged(bool value)
    {
        if(value)
        {
            fpsCounterOnButton.interactable = false;
            fpsCounterOffButton.interactable = true;
        }
        else
        {
            fpsCounterOffButton.interactable = false;
            fpsCounterOnButton.interactable = true;
        }    
    }
}

public static class LanguageSettings
{
    public static string language = PlayerPrefs.GetString("Language", "en");
}
