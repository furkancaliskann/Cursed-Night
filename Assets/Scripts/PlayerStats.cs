using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private Death death;
    private HealthNotification healthNotification;
    private Injury injury;
    private LockMovement lockMovement;
    private Player player;
    private PlayerMovement playerMovement;
    private SaveManager saveManager;

    public float health { get; private set; }
    public float food { get; private set; }
    public float water { get; private set; }
    public float energy { get; private set; }

    public bool canRun { get; private set; }
    private float canRunTimer = 5f;
    private float canRunTimerMax = 5f;

    private const float foodDecreaseValue = 0.03f;
    private const float waterDecreaseValue = 0.07f;

    private const float energyDecreaseValue = 2f;
    private const float energyIncreaseValue = 4f;  
    private const float foodDecreaseForEnergyValue = 0.015f;
    private const float waterDecreaseForEnergyValue = 0.035f;

    private const float healthIncreaseValue = 0.1f;
    private const float foodDecreaseForHealthValue = 0.06f;

    private const float starvingHealthDecreaseValue = 0.5f;
    private const float dehydrationHealthDecreaseValue = 1f;  

    [SerializeField] private Image healthBar;
    [SerializeField] private Image foodBar;
    [SerializeField] private Image waterBar;
    [SerializeField] private Image energyBar;

    [SerializeField] private GameObject parentObject;

    [SerializeField] private AudioSource breathAudioSource;


    void Awake()
    {
        death = GetComponent<Death>();
        healthNotification = GetComponent<HealthNotification>();
        injury = GetComponent<Injury>();
        lockMovement = GetComponent<LockMovement>();
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();

        canRun = true;
    }
    void Update()
    {
        CheckHealthForDie();
        CheckCanRun();
        IncreaseEnergyOverTime();
        IncreaseHealthOverTime();
        DecreaseFoodOverTime();
        DecreaseWaterOverTime();

        if(lockMovement.zoomOn) ActivateBar(false);
        else ActivateBar(true);
    }
    private void CheckHealthForDie()
    {
        if (health <= 0)
        {
            death.StartDeathProcess();
        }
    }
    private void CheckCanRun()
    {
        if (canRun) return;

        if(canRunTimer > 0)
        {
            canRunTimer -= Time.deltaTime;
        }

        if(canRunTimer <= 0)
        {
            canRun = true;
            canRunTimer = canRunTimerMax;
        }
    }
    public void FallDamage(float velocity)
    {
        if (player.godMode) return;

        DecreaseHealth(velocity * 8f, true);

        if (velocity >= 5f)
        {
            healthNotification.Create(HealthEffects.Broken);
        }
    }
    public void DecreaseHealth(float amount, bool showEffect)
    {
        if (player.godMode || health <= 0) return;

        health -= amount;

        if(showEffect && health > 0)
        {
            injury.StartInjuryEffect();

            int number = Random.Range(0, 100);

            if (number >= 95)
            {
                healthNotification.Create(HealthEffects.Bleeding);
            }
        }
    }    
    public void DecreaseEnergy(float amount)
    {
        if (player.godMode || energy <= 0) return;

        if (energy > 0)
            energy -= amount;
        else
            energy = 0;

        if (energy < 0)
        {
            canRun = false;
            energy = 0;
            PlayBreathSound();
        }
        UpdateBars();
    }
    private void DecreaseFoodOverTime()
    {
        if (player.godMode) return;
        if (food > 0)
            food -= Time.deltaTime * foodDecreaseValue;
        else
        {
            food = 0;
            health -= Time.deltaTime * starvingHealthDecreaseValue;
        }
            

        UpdateBars();
    }
    private void DecreaseWaterOverTime()
    {
        if (player.godMode) return;

        if (water > 0)
            water -= Time.deltaTime * waterDecreaseValue;
        else
        {
            water = 0;
            energy = 0;
            health -= Time.deltaTime * dehydrationHealthDecreaseValue;
        }

        UpdateBars();
    }
    public void DecreaseEnergyOverTime()
    {
        if (player.godMode || energy <= 0) return;

        if (energy > 0)
            energy -= Time.deltaTime * energyDecreaseValue;
        if (energy <= 0)
        {
            canRun = false;
            energy = 0;
            PlayBreathSound();
        }         

        UpdateBars();
    }
    private void IncreaseEnergyOverTime()
    {
        if (energy >= 100 || water <= 0 || food <= 0 || playerMovement.isRunning) return;

        water -= Time.deltaTime * waterDecreaseForEnergyValue;
        food -= Time.deltaTime * foodDecreaseForEnergyValue;
        energy += Time.deltaTime * energyIncreaseValue;

        if (energy > 100) energy = 100;

        UpdateBars();
    }
    private void IncreaseHealthOverTime()
    {
        if (health >= 100 || water <= 0 || food < 50) return;

        food -= Time.deltaTime * foodDecreaseForHealthValue;
        health += Time.deltaTime * healthIncreaseValue;

        if (health >= 100) health = 100;

        UpdateBars();
    }
    private void PlayBreathSound()
    {
        if (breathAudioSource.isPlaying) return;

        breathAudioSource.time = 0.05f;
        breathAudioSource.Play();
        breathAudioSource.SetScheduledEndTime(AudioSettings.dspTime + (04.80f - 0.25f));
    }
    public void IncreaseFood(int value)
    {
        if (food + value > 100) food = 100;
        else if (food + value < 0) food = 0;
        else food += value;

        UpdateBars();
    }
    public void IncreaseWater(int value)
    {
        if (water + value > 100) water = 100;
        else if (water + value < 0) water = 0;
        else water += value;

        UpdateBars();
    }
    public void IncreaseHealth(int value)
    {
        if (health + value > 100) health = 100;
        else if (health + value < 0) health = 0;
        else health += value;

        UpdateBars();
    }
    public void IncreaseStats(int foodValue, int waterValue, int healthValue)
    {
        if (food + foodValue > 100) food = 100;
        else if (food + foodValue < 0) food = 0;
        else food += foodValue;

        if (water + waterValue > 100) water = 100;
        else if (water + waterValue < 0) water = 0;
        else water += waterValue;


        if (health + healthValue > 100) health = 100;
        else if (health + healthValue < 0) health = 0;
        else health += healthValue;

        UpdateBars();
    }
    public void ChangeFoodValue(float value)
    {
        food = value;
        UpdateBars();
    }
    public void ChangeWaterValue(float value)
    {
        water = value;
        UpdateBars();
    }
    public void ChangeEnergyValue(float value)
    {
        energy = value;
        UpdateBars();
    }
    public void MaximizeAllStats()
    {
        food = water = energy = health = 100;
        UpdateBars();
    }
    private void UpdateBars()
    {
        healthBar.transform.localScale = new Vector3(health / 100f, 1, 1);
        foodBar.transform.localScale = new Vector3(food / 100f, 1, 1);
        waterBar.transform.localScale = new Vector3(water / 100f, 1, 1);
        energyBar.transform.localScale = new Vector3(energy / 100f, 1, 1);
    }
    private void ActivateBar(bool value)
    {
        if(value)
        {
            if (parentObject.activeInHierarchy) return;
            parentObject.SetActive(true);
        }
        else
        {
            if (!parentObject.activeInHierarchy) return;
            parentObject.SetActive(false);
        }
    }
    public void SaveStats()
    {
        SaveStats saveStats = new SaveStats(health, food, water, energy);
        StartCoroutine(saveManager.Save(saveStats, SaveType.PlayerStats));
    }
    public void LoadStats()
    {
        SaveStats loadedStats = JsonUtility.FromJson<SaveStats>(saveManager.Load(SaveType.PlayerStats));
        if(loadedStats == null)
        {
            MaximizeAllStats();
            return;
        }

        health = loadedStats.health;
        food = loadedStats.food;
        water = loadedStats.water;
        energy = loadedStats.energy;
        UpdateBars();
    }
}

[System.Serializable]
public class SaveStats
{
    public float health;
    public float food;
    public float water;
    public float energy;

    public SaveStats(float health, float food, float water, float energy)
    { 
        this.health = health;
        this.food = food;
        this.water = water;
        this.energy = energy;
    }
}
