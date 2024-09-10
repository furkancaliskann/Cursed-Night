using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthNotification : MonoBehaviour
{
    private PlayerStats playerStats;
    private SaveManager saveManager;

    [SerializeField] private Transform notificationParent;
    [SerializeField] private GameObject notificationPrefab;

    private List<GameObject> notifications = new List<GameObject>();
    private List<HealthEffects> notificationsEnum = new List<HealthEffects>();

    [SerializeField] private Sprite bleeding, broken, health, hunger, thirst;

    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
    }
    void Update()
    {
        CheckStats();
    }
    private Sprite SpriteFromEffectType(HealthEffects type)
    {
        switch (type)
        {
            case HealthEffects.Bleeding: return bleeding;
            case HealthEffects.Broken: return broken;
            case HealthEffects.Health: return health;
            case HealthEffects.Hunger: return hunger;
            case HealthEffects.Thirst: return thirst;
            default: return null;
        }
    }
    private void CheckStats()
    {
        if (playerStats.health < 20) Create(HealthEffects.Health);
        else Delete(HealthEffects.Health);

        if (playerStats.water < 20) Create(HealthEffects.Thirst);
        else Delete(HealthEffects.Thirst);

        if (playerStats.food < 20) Create(HealthEffects.Hunger);
        else Delete(HealthEffects.Hunger);

        if (Check(HealthEffects.Bleeding))
            playerStats.DecreaseHealth(0.01f, false);
    }
    public void Create(HealthEffects type)
    {
        if (Check(type)) return;

        GameObject myObject = Instantiate(notificationPrefab, notificationParent);

        myObject.GetComponentsInChildren<Image>()[1].sprite = SpriteFromEffectType(type);

        notifications.Add(myObject);
        notificationsEnum.Add(type);
    }
    public void Delete(HealthEffects type)
    {
        if (!Check(type)) return;

        for (int i = 0; i < notifications.Count; i++)
        {
            if(notifications[i].GetComponentsInChildren<Image>()[1].sprite == SpriteFromEffectType(type))
            {
                Destroy(notifications[i]);
                notifications.RemoveAt(i);
                notificationsEnum.RemoveAt(i);
            }
        }
    }
    public void DeleteAll()
    {
        for (int i = 0; i < notifications.Count; i++)
        {
            Destroy(notifications[i]);
            notifications.RemoveAt(i);
            notificationsEnum.RemoveAt(i);
        }
    }
    public bool Check(HealthEffects type)
    {
        return notificationsEnum.Contains(type);
    }
    public void SaveHealthNotification()
    {
        SaveHealthNotification saveHealthNotification = new SaveHealthNotification();
        saveHealthNotification.healthEffects.AddRange(notificationsEnum);

        StartCoroutine(saveManager.Save(saveHealthNotification, SaveType.HealthNotification));
    }
    public void LoadHealthNotification()
    {
        SaveHealthNotification loadedHealthNotification = JsonUtility.FromJson<SaveHealthNotification>(saveManager.Load(SaveType.HealthNotification));
        if (loadedHealthNotification == null) return;

        for (int i = 0; i < loadedHealthNotification.healthEffects.Count; i++)
        {
            Create(loadedHealthNotification.healthEffects[i]);
        }
    }
}

public enum HealthEffects
{
    Bleeding,
    Broken,
    Hunger,
    Health,
    Thirst,
}

public class SaveHealthNotification
{
    public List<HealthEffects> healthEffects = new List<HealthEffects>();
}