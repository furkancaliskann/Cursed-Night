using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{
    private AudioSource audioSource;
    private FurnaceCanvas furnaceCanvas;
    private ItemList itemList;

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Light lightSource;
    
    public Item smeltItem;
    public Item smeltedItem;
    public Item fuelItem;

    private Dictionary<string, string> smeltResults = new Dictionary<string, string>();

    public bool furnaceStarted {  get; private set; }

    private float burningTime = 0.5f;
    private float burningTimeMax = 0.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();

        smeltResults.Add("scrap_metal", "forged_iron");
        smeltResults.Add("coin", "forged_iron");
        smeltResults.Add("sulfur", "forged_sulfur");
        CreateItems();
    }
    void Update()
    {
        CheckFuel();
    }
    private void CheckFuel()
    {
        if (!furnaceStarted) return;

        if (fuelItem == null || fuelItem.nickName != "wood")
        {
            StartStopFurnace();
            burningTime = burningTimeMax;
            return;
        }

        if (burningTime > 0) burningTime -= Time.deltaTime;

        if (burningTime <= 0)
        { 

            MeltMine();

            if (fuelItem.amount > 0)
            {
                fuelItem.DecreaseAmount(1);

                if (furnaceCanvas != null)
                    furnaceCanvas.PlaceItems(smeltItem, smeltedItem, fuelItem);
                burningTime = burningTimeMax;
            }

            if (fuelItem.amount <= 0)
            {
                StartStopFurnace();
            }
        }
    }
    private void MeltMine()
    {
        if (smeltItem == null || !smeltResults.ContainsKey(smeltItem.nickName)) return;

        if (smeltItem.amount <= 0)
        {
            smeltItem = null;
        }
        else
        {
            if(smeltedItem == null)
            {
                smeltedItem = itemList.CreateNewItem(smeltResults[smeltItem.nickName], 1);
                smeltItem.DecreaseAmount(1);
            }
            else if(smeltedItem.nickName == smeltResults[smeltItem.nickName] && smeltedItem.amount + 1 <= smeltedItem.maxAmount)
            {
                smeltedItem.IncreaseAmount(1);
                smeltItem.DecreaseAmount(1);
            }   
        }
        

        if(furnaceCanvas != null)
            furnaceCanvas.PlaceItems(smeltItem, smeltedItem, fuelItem);
    }
    public void StartStopFurnace()
    {
        if (!furnaceStarted && fuelItem != null && fuelItem.amount > 0)
        {
            furnaceStarted = true;
            particle.Play();
            lightSource.enabled = true;
            audioSource.Play();
        }
        else if (furnaceStarted)
        {
            furnaceStarted = false;
            particle.Stop();
            lightSource.enabled = false;
            audioSource.Stop();
        }
    }
    public void SetFurnaceCanvas(FurnaceCanvas furnaceCanvas)
    {
        this.furnaceCanvas = furnaceCanvas;
    }
    private void CreateItems()
    {
        smeltItem = null;
        smeltedItem = null;
        fuelItem = null;
    }
}
