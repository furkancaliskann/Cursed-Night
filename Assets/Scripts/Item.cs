using UnityEngine;

public class Item
{
    public string nickName { get; private set; }
    public int id { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public int amount {  get; private set; }
    public int maxAmount { get; private set; }
    public int durability { get; private set; }
    public int maxDurability { get; private set; }
    public int ammoInside { get; private set; }
    public int maxAmmoInside { get; private set; }
    public string ammoTypeNick { get; private set; }
    public int foodValue { get; private set; }
    public int waterValue { get; private set; }
    public int healthValue { get; private set; }
    public BlockTypes blockType { get; private set; }
    public PlaceableGrounds placeableGrounds { get; private set; }
    public GameObject blockPreview { get; private set; }
    public GameObject blockPrefab { get; private set; }
    public bool upgradeable { get; private set; }
    public MaterialType materialType { get; private set; }
    public Categories category { get; private set; }
    public Sprite image { get; private set; }


    public Item(string nickName, int id, string name, string description, int amount, int maxAmount, int durability, int maxDurability,
        int ammoInside, int maxAmmoInside, string ammoTypeNick, int foodValue,
        int waterValue, int healthValue, BlockTypes blockType, PlaceableGrounds placeableGrounds,
        bool upgradeable, MaterialType materialType, Categories category)
    {
        this.nickName = nickName;
        this.id = id;
        if (description == "") Debug.Log(nickName);
        this.name = name;
        this.description = description;
        this.amount = amount;
        this.maxAmount = maxAmount;
        this.durability = durability;
        this.maxDurability = maxDurability;
        this.ammoInside = ammoInside;
        this.maxAmmoInside = maxAmmoInside;
        this.ammoTypeNick = ammoTypeNick;
        this.foodValue = foodValue;
        this.waterValue = waterValue;
        this.healthValue = healthValue;
        this.blockType = blockType;
        this.placeableGrounds = placeableGrounds;

        if (this.blockType != BlockTypes.Empty)
        {
            blockPreview = Resources.Load<GameObject>("Buildings/Previews/" + nickName);
            blockPrefab = Resources.Load<GameObject>("Buildings/" + nickName);
        }
        this.upgradeable = upgradeable;
        this.materialType = materialType;
            
        this.category = category;

        image = Resources.Load<Sprite>("Item Images/" + nickName);
    }

    public void IncreaseAmount(int value)
    {
        amount += value;
    }
    public void DecreaseAmount(int value)
    {
        amount -= value;
    }
    public void DecreaseDurability(int value)
    {
        durability -= value;
    }
    public void DecreaseAmmoInside(int value)
    {
        ammoInside -= value;
    }
    public void IncreaseAmmoInside(int value)
    {
        ammoInside += value;
        if(ammoInside > maxAmmoInside) ammoInside = maxAmmoInside;
    }
}

public enum BlockTypes
{
    Empty,
    Frame,
    Stair,
    Door,
    Glass,    
    Campfire,
    Furnace,
    SleepingBag,
    Chest,
    Mine,
    FarmingPlot,
    Ladder,
    WoodSpike
}

public enum PlaceableGrounds
{
    None,
    Terrain,
    Frames,
    All
}

public enum MaterialType
{
    Empty,
    Frame,
    Wood,
    Stone,
    Iron
}

public enum Categories
{
    Empty,
    Weapon,
    CloseRange,
    Ammo,
    Food,
    Water,
    Health,
    Block,
    Tools,
    RawMaterial,
    Explosive,
    Material,
    Seed
}