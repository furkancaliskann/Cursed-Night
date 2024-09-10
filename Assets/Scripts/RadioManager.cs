using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    private SaveManager saveManager;

    public List<RadioChannel> radioChannels;

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
    }
    void Update()
    {
        CheckTime();
    }
    private void CheckTime()
    {
        for (int i = 0; i < radioChannels.Count; i++) 
        {
            if (radioChannels[i].currentTime < radioChannels[i].selectedMaxTime) 
                radioChannels[i].currentTime += Time.deltaTime;

            if (radioChannels[i].currentTime >= radioChannels[i].selectedMaxTime)
            {
                radioChannels[i].selected = false;
                radioChannels[i].selectedIndex = -1;
                SelectRandomMusic(radioChannels[i]);
            }
        } 
    }
    private void SelectRandomMusic(RadioChannel channel)
    {
        if (channel.selected) return;

        int selectedIndex = Random.Range(0, channel.clips.Count);
        channel.currentTime = 0;
        channel.selectedMaxTime = channel.clips[selectedIndex].length;
        channel.selectedClip = channel.clips[selectedIndex];
        channel.selected = true;
        channel.selectedIndex = selectedIndex;
    }
    public void SaveRadio()
    {
        SaveRadio saveRadio = new SaveRadio();

        for (int i = 0; i < radioChannels.Count; i++)
        {
            saveRadio.channelName.Add(radioChannels[i].channelName);
            saveRadio.currentTime.Add(radioChannels[i].currentTime);
            saveRadio.selectedMaxTime.Add(radioChannels[i].selectedMaxTime);
            saveRadio.selected.Add(radioChannels[i].selected);
            saveRadio.selectedIndex.Add(radioChannels[i].selectedIndex);
        }

        StartCoroutine(saveManager.Save(saveRadio, SaveType.Radio));
    }
    public void LoadRadio()
    {
        SaveRadio loadedRadio = JsonUtility.FromJson<SaveRadio>(saveManager.Load(SaveType.Radio));
        if (loadedRadio == null) return;

        for (int i = 0; i < loadedRadio.channelName.Count; i++)
        {
            RadioChannel channel = radioChannels.Find(x => x.channelName == loadedRadio.channelName[i]);
            if (channel == null) continue;

            channel.currentTime = loadedRadio.currentTime[i];
            channel.selectedMaxTime = loadedRadio.selectedMaxTime[i];
            channel.selected = loadedRadio.selected[i];
            channel.selectedIndex = loadedRadio.selectedIndex[i];
            channel.selectedClip = channel.clips[channel.selectedIndex];
        }
    }
}

[System.Serializable]
public class RadioChannel
{
    public List<AudioClip> clips;
    public string channelName;
    public float currentTime;
    public float selectedMaxTime;
    public bool selected;
    public int selectedIndex;
    public AudioClip selectedClip;
}

[System.Serializable]
public class SaveRadio
{
    public List<string> channelName = new List<string>();
    public List<float> currentTime = new List<float>();
    public List<float> selectedMaxTime = new List<float>();
    public List<bool> selected = new List<bool>();
    public List<int> selectedIndex = new List<int>();
}
