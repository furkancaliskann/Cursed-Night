using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private SaveManager saveManager;

    private float timeMultiplierDay = 40;
    private float timeMultiplierNight = 100;
    [SerializeField] private Light sunLight;
    [SerializeField] private Light moonLight;

    private float sunriseHour = 5.30f;
    private float sunsetHour = 18.30f;

    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;
    [SerializeField] private float maxSunLightIntensity;
    [SerializeField] private float maxMoonLightIntensity;
    [SerializeField] private AnimationCurve lightChangeCurve;

    private DateTime currentTime;

    void Awake()
    {
        saveManager = GetComponent<SaveManager>();
    }
    void Start()
    {
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    } 
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }
    private void UpdateTimeOfDay()
    {
        if(currentTime.Hour >= 4 && currentTime.Hour < 20)
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplierDay);
        else
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplierNight);
    }
    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }
    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }
    private TimeSpan CalculateTimeDifference (TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
    public void SetMinute(int minute)
    {
        if (minute < 0 || minute >= 60) return;

        currentTime = currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
            currentTime.Hour, minute, 0);
    }
    public void SetHour(int hour)
    {
        if (hour < 0 || hour >= 24) return;

        currentTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
            hour, 0, 0);
    }
    public void SetDay(int day)
    {
        if (day < 1 || day >= 31) return;

        currentTime = new DateTime(currentTime.Year, currentTime.Month, day,
            0, 0, 0);
    }
    public void SetMultiplier(float value)
    {
        timeMultiplierDay = value;
        timeMultiplierNight = value;
    }
    public DateTime GetTime()
    {
        return currentTime;
    }
    public void SaveTime()
    {
        SaveTime saveTimeClass = new SaveTime(currentTime.Year, currentTime.Month, currentTime.Day,
            currentTime.Hour, currentTime.Minute, currentTime.Second);

        StartCoroutine(saveManager.Save(saveTimeClass, SaveType.Time));
    }
    public void LoadTime()
    {
        SaveTime loadedTime = JsonUtility.FromJson<SaveTime>(saveManager.Load(SaveType.Time));
        if (loadedTime == null)
        {
            currentTime = new DateTime(0001, 1, 1, 8, 0, 0);
            return;
        }

        currentTime = new DateTime(loadedTime.year, loadedTime.month, loadedTime.day, loadedTime.hour, loadedTime.minute, loadedTime.second);
    }
}

[System.Serializable]
public class SaveTime
{
    public int year;
    public int month;
    public int day;  
    public int hour;
    public int minute;
    public int second;

    public SaveTime(int year, int month, int day, int hour, int minute, int second)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
        this.second = second;
    }
}
