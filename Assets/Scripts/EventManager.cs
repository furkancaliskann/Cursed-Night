using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private FogManager fogManager;
    private SaveManager saveManager;
    private TimeController timeController;

    private DateTime eventDate;
    private DateTime eventFinishDate;
    public bool eventStarted {  get; private set; }

    void Awake()
    {
        fogManager = GetComponent<FogManager>();
        saveManager = GetComponent<SaveManager>();
        timeController = GetComponent<TimeController>();
    }
    void Start()
    {
        Invoke(nameof(SetNewEventDate), 2f);
    }
    void Update()
    {
        CheckEventStartDate();
        CheckEventStopDate();
    }
    public void SetNewEventDate()
    {
        DateTime currentTime = timeController.GetTime();
        if (eventFinishDate > currentTime) return;

        int randomHour = UnityEngine.Random.Range(0, 25);
        int randomMinute = UnityEngine.Random.Range(0, 61);
        eventDate =  currentTime + TimeSpan.FromDays(1) + TimeSpan.FromHours(randomHour) + TimeSpan.FromMinutes(randomMinute);

        eventFinishDate = eventDate + TimeSpan.FromMinutes(UnityEngine.Random.Range(60, 150));
    }
    private void CheckEventStartDate()
    {
        if (eventStarted) return;

        DateTime currentTime = timeController.GetTime();

        if (currentTime >= eventDate && currentTime <= eventFinishDate) 
        {
            eventStarted = true;
            fogManager.SetEventFogState(true);
        }
    }
    private void CheckEventStopDate()
    {
        if (!eventStarted) return;

        DateTime currentTime = timeController.GetTime();

        if(currentTime > eventFinishDate)
        {
            eventStarted = false;
            fogManager.SetEventFogState(false);
            SetNewEventDate();
        }
    }
    public void SaveEvent()
    {
        SaveEvent saveEvent = new SaveEvent();
        saveEvent.eventDate = eventDate.ToString();
        saveEvent.eventFinishDate = eventFinishDate.ToString();
        saveEvent.eventStarted = eventStarted;

        StartCoroutine(saveManager.Save(saveEvent, SaveType.Event));
    }
    public void LoadEvent()
    {
        SaveEvent loaded = JsonUtility.FromJson<SaveEvent>(saveManager.Load(SaveType.Event));
        if (loaded == null) return;

        eventDate = DateTime.Parse(loaded.eventDate);
        eventFinishDate = DateTime.Parse(loaded.eventFinishDate);
        eventStarted = loaded.eventStarted;

        if (eventStarted)
        {
            fogManager.SetEventFogState(true);
            fogManager.LoadEventFogFast();
        }
            
    }
}

[System.Serializable]
public class SaveEvent
{
    public string eventDate;
    public string eventFinishDate;
    public bool eventStarted;
}
