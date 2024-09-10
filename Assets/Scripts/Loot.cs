using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private ItemList itemList;

    private List<LootChance> boxItems = new List<LootChance>
    {
        new LootChance{nickName = null, minAmount = 0, maxAmount = 0, percent = 10},
        new LootChance{nickName = "screw", minAmount = 1, maxAmount = 4, percent = 5},
        new LootChance{nickName = "cloth", minAmount = 1, maxAmount = 3, percent = 5},
        new LootChance{nickName = "cream", minAmount = 1, maxAmount = 1, percent = 5},
        new LootChance{nickName = "paper", minAmount = 2, maxAmount = 5, percent = 5},
        new LootChance{nickName = "plastic", minAmount = 1, maxAmount = 4, percent = 5},
        new LootChance{nickName = "pan", minAmount = 1, maxAmount = 1, percent = 4},
        new LootChance{nickName = "oil", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "glue", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "tape", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "soil", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "bullet_mold", minAmount = 1, maxAmount = 5, percent = 4},
        new LootChance{nickName = "bullet_tip", minAmount = 1, maxAmount = 8, percent = 4},
        new LootChance{nickName = "empty_bottle", minAmount = 1, maxAmount = 4, percent = 4},
        new LootChance{nickName = "dirty_water", minAmount = 1, maxAmount = 4, percent = 4},
        new LootChance{nickName = "stick", minAmount = 1, maxAmount = 20, percent = 4},
        new LootChance{nickName = "powder", minAmount = 1, maxAmount = 10, percent = 4},
        new LootChance{nickName = "chemical", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "rope", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "sewing_kit", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "broken_bottle", minAmount = 1, maxAmount = 4, percent = 4},
        new LootChance{nickName = "axe", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "pickaxe", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "scope", minAmount = 1, maxAmount = 1, percent = 1},
    };

    private List<LootChance> militaryChestItems = new List<LootChance>
    {
        new LootChance{nickName = "bullet_mold", minAmount = 5, maxAmount = 25, percent = 14},
        new LootChance{nickName = "bullet_tip", minAmount = 15, maxAmount = 40, percent = 14},
        new LootChance{nickName = "powder", minAmount = 20, maxAmount = 50, percent = 11},
        new LootChance{nickName = "9mm_ammo", minAmount = 5, maxAmount = 25, percent = 10},
        new LootChance{nickName = "7.62mm_ammo", minAmount = 5, maxAmount = 25, percent = 10},
        new LootChance{nickName = "shotgun_shell", minAmount = 5, maxAmount = 25, percent = 10},
        new LootChance{nickName = "landmine", minAmount = 1, maxAmount = 3, percent = 8},
        new LootChance{nickName = "dynamite", minAmount = 1, maxAmount = 3, percent = 8},
        new LootChance{nickName = "scope", minAmount = 1, maxAmount = 1, percent = 5},
        new LootChance{nickName = "pistol", minAmount = 1, maxAmount = 1, percent = 4},
        new LootChance{nickName = "shotgun", minAmount = 1, maxAmount = 1, percent = 3},
        new LootChance{nickName = "assault_rifle", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "hunting_rifle", minAmount = 1, maxAmount = 1, percent = 1},
    };

    private List<LootChance> airdropItems = new List<LootChance>
    {
        new LootChance{nickName = "bottled_water", minAmount = 3, maxAmount = 10, percent = 13},
        new LootChance{nickName = "first_aid_kit", minAmount = 2, maxAmount = 7, percent = 12},
        new LootChance{nickName = "iron_door", minAmount = 1, maxAmount = 1, percent = 11},
        new LootChance{nickName = "painkiller", minAmount = 3, maxAmount = 7, percent = 11},
        new LootChance{nickName = "meat_stew", minAmount = 3, maxAmount = 10, percent = 9},
        new LootChance{nickName = "pistol", minAmount = 1, maxAmount = 1, percent = 8},
        new LootChance{nickName = "scope", minAmount = 1, maxAmount = 1, percent = 7},
        new LootChance{nickName = "landmine", minAmount = 2, maxAmount = 6, percent = 7},
        new LootChance{nickName = "dynamite", minAmount = 2, maxAmount = 6, percent = 7},
        new LootChance{nickName = "shotgun", minAmount = 1, maxAmount = 1, percent = 6},
        new LootChance{nickName = "assault_rifle", minAmount = 1, maxAmount = 1, percent = 5},
        new LootChance{nickName = "hunting_rifle", minAmount = 1, maxAmount = 1, percent = 4},
    };

    private List<LootChance> fridgeItems = new List<LootChance>
    {
        new LootChance{nickName = "empty_bottle", minAmount = 1, maxAmount = 3, percent = 16},
        new LootChance{nickName = "dirty_water", minAmount = 1, maxAmount = 2, percent = 13},
        new LootChance{nickName = "bottled_water", minAmount = 1, maxAmount = 2, percent = 10},
        new LootChance{nickName = "fruit_juice", minAmount = 1, maxAmount = 2, percent = 7},
        new LootChance{nickName = "egg", minAmount = 1, maxAmount = 2, percent = 7},
        new LootChance{nickName = "rosehip", minAmount = 1, maxAmount = 3, percent = 7},
        new LootChance{nickName = "animal_fat", minAmount = 1, maxAmount = 3, percent = 6},
        new LootChance{nickName = "salt", minAmount = 1, maxAmount = 2, percent = 6},
        new LootChance{nickName = "rosehip_tea", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "coffee", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "milk", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "canned_pea", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "canned_bean", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "canned_fish", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "pancake", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "potato_balls", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "pizza", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "tomato_soup", minAmount = 1, maxAmount = 1, percent = 2},
        
    };

    private List<LootChance> foodBoxItems = new List<LootChance>
    {
        new LootChance{nickName = "empty_bottle", minAmount = 1, maxAmount = 4, percent = 8},
        new LootChance{nickName = "bottled_water", minAmount = 1, maxAmount = 2, percent = 8},
        new LootChance{nickName = "salt", minAmount = 1, maxAmount = 4, percent = 6},
        new LootChance{nickName = "spice", minAmount = 1, maxAmount = 3, percent = 6},
        new LootChance{nickName = "potato_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "corn_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "mushroom_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "cucumber_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "onion_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "pepper_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "tomato_seed", minAmount = 2, maxAmount = 4, percent = 5},
        new LootChance{nickName = "rosehip_seed", minAmount = 1, maxAmount = 3, percent = 5},
        new LootChance{nickName = "potato", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "corn", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "mushroom", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "cucumber", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "onion", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "pepper", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "tomato", minAmount = 1, maxAmount = 3, percent = 4},
        new LootChance{nickName = "rosehip", minAmount = 1, maxAmount = 2, percent = 4},   
    };
    private List<LootChance> mineItems = new List<LootChance>
    {
        new LootChance{nickName = "stick", minAmount = 20, maxAmount = 70, percent = 12},
        new LootChance{nickName = "scrap_iron", minAmount = 20, maxAmount = 250, percent = 12},
        new LootChance{nickName = "sulfur", minAmount = 20, maxAmount = 150, percent = 12},
        new LootChance{nickName = "coal", minAmount = 20, maxAmount = 150, percent = 12},
        new LootChance{nickName = "broken_bottle", minAmount = 1, maxAmount = 3, percent = 8},
        new LootChance{nickName = "glue", minAmount = 1, maxAmount = 3, percent = 5},
        new LootChance{nickName = "tape", minAmount = 1, maxAmount = 3, percent = 5},
        new LootChance{nickName = "soil", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "chemical", minAmount = 1, maxAmount = 4, percent = 5},       
        new LootChance{nickName = "screw", minAmount = 1, maxAmount = 5, percent = 5},
        new LootChance{nickName = "powder", minAmount = 10, maxAmount = 50, percent = 5},
        new LootChance{nickName = "chest", minAmount = 1, maxAmount = 1, percent = 4},
        new LootChance{nickName = "pickaxe", minAmount = 1, maxAmount = 1, percent = 4},
        new LootChance{nickName = "dynamite", minAmount = 1, maxAmount = 1, percent = 3},
        new LootChance{nickName = "gasoline", minAmount = 20, maxAmount = 200, percent = 2},
        new LootChance{nickName = "pistol", minAmount = 1, maxAmount = 1, percent = 1},
    };

    private List<LootChance> fuelItems = new List<LootChance>
    {
        new LootChance{nickName = "wood", minAmount = 20, maxAmount = 200, percent = 40},
        new LootChance{nickName = "stick", minAmount = 20, maxAmount = 200, percent = 25},
        new LootChance{nickName = "chest", minAmount = 1, maxAmount = 1, percent = 15},
        new LootChance{nickName = "bullet_mold", minAmount = 1, maxAmount = 10, percent = 10},
        new LootChance{nickName = "bullet_tip", minAmount = 5, maxAmount = 20, percent = 10},
    };

    private List<LootChance> buildingItems = new List<LootChance>
    {
        new LootChance{nickName = "wood_frame", minAmount = 5, maxAmount = 20, percent = 18},
        new LootChance{nickName = "wood_block", minAmount = 3, maxAmount = 10, percent = 13},
        new LootChance{nickName = "wood_stair", minAmount = 3, maxAmount = 10, percent = 13},
        new LootChance{nickName = "wood_door", minAmount = 1, maxAmount = 1, percent = 10},
        new LootChance{nickName = "stone_block", minAmount = 2, maxAmount = 8, percent = 8},
        new LootChance{nickName = "stone_stair", minAmount = 2, maxAmount = 8, percent = 8},
        new LootChance{nickName = "iron_block", minAmount = 1, maxAmount = 3, percent = 6},
        new LootChance{nickName = "iron_stair", minAmount = 1, maxAmount = 3, percent = 6},
        new LootChance{nickName = "iron_door", minAmount = 1, maxAmount = 1, percent = 3},
        new LootChance{nickName = "glass", minAmount = 1, maxAmount = 5, percent = 2},
        new LootChance{nickName = "ladder", minAmount = 1, maxAmount = 5, percent = 2},
        new LootChance{nickName = "soil", minAmount = 1, maxAmount = 2, percent = 2},
        new LootChance{nickName = "glue", minAmount = 1, maxAmount = 2, percent = 2},
        new LootChance{nickName = "tape", minAmount = 1, maxAmount = 2, percent = 2},
        new LootChance{nickName = "campfire", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "sleeping_bag", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "chest", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "farming_plot", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "furnace", minAmount = 1, maxAmount = 1, percent = 1},
    };

    private List<LootChance> birdNestItems = new List<LootChance>
    { 
        new LootChance{nickName = "feather", minAmount = 1, maxAmount = 5, percent = 80},
        new LootChance{nickName = "egg", minAmount = 1, maxAmount = 2, percent = 20},
    };

    private List<LootChance> zombieLoot = new List<LootChance>
    {
        new LootChance{nickName = "empty_bottle", minAmount = 1, maxAmount = 2, percent = 12},
        new LootChance{nickName = "dirty_water", minAmount = 1, maxAmount = 2, percent = 9},
        new LootChance{nickName = "bottled_water", minAmount = 1, maxAmount = 2, percent = 7},   
        new LootChance{nickName = "splint", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "arrow", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "bullet_tip", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "bullet_mold", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "flour", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "powder", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "cream", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "screw", minAmount = 1, maxAmount = 2, percent = 5},
        new LootChance{nickName = "canned_pea", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "canned_bean", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "canned_fish", minAmount = 1, maxAmount = 2, percent = 4},
        new LootChance{nickName = "9mm_ammo", minAmount = 1, maxAmount = 2, percent = 3},
        new LootChance{nickName = "7.62mm_ammo", minAmount = 1, maxAmount = 2, percent = 3},
        new LootChance{nickName = "shotgun_shell", minAmount = 1, maxAmount = 2, percent = 3},
        new LootChance{nickName = "bandage", minAmount = 1, maxAmount = 2, percent = 3},
        new LootChance{nickName = "first_aid_kit", minAmount = 1, maxAmount = 2, percent = 3},
        new LootChance{nickName = "pistol", minAmount = 1, maxAmount = 1, percent = 2},
        new LootChance{nickName = "shotgun", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "assault_rifle", minAmount = 1, maxAmount = 1, percent = 1},
        new LootChance{nickName = "hunting_rifle", minAmount = 1, maxAmount = 1, percent = 1},
    };
    private List<LootChance> deerLoot = new List<LootChance>
    {
        new LootChance{nickName = "raw_meat", minAmount = 1, maxAmount = 1, percent = 50},
        new LootChance{nickName = "leather", minAmount = 1, maxAmount = 1, percent = 25},
        new LootChance{nickName = "animal_fat", minAmount = 1, maxAmount = 1, percent = 25},
    };


    public List<Item> items = new List<Item>();
    private int itemCount = 15;

    public Loots lootType;
    public int minimumItemCount;
    public int maximumItemCount;

    private bool isPanelOpen;
    public bool firstOpen { get; private set; }

    void Awake()
    {
        itemList = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ItemList>();

        CreateItems();
    }
    void Update()
    {
        CheckDestroy();
    }
    private void CheckDestroy()
    {
        if (isPanelOpen || !CheckItemsNull() || !firstOpen) return;

        if (lootType == Loots.Airdrop || lootType == Loots.Zombie || lootType == Loots.Deer)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootManager>().RemoveOtherLoot(gameObject);

        else if (lootType == Loots.BirdNest)
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>().RemoveCollectable(gameObject);

        Destroy(gameObject);
    }
    private bool CheckItemsNull()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                return false;
            }
        }
        return true;
    }
    private void CreateItems()
    {
        for (int i = 0; i < itemCount; i++)
        {
            items.Add(null);
        }
    }
    public void FillItems()
    {
        if(lootType == Loots.Deer)
        {
            int totalAmount = GetRandomCount() - 5;
            int meat = 3;
            int leather = 1;
            int animalFat = 1;

            for (int i = 0; i < totalAmount; i++)
            {
                int number = Random.Range(0, 3);

                if (number == 0) meat++;
                else if (number == 1) leather++;
                else if (number == 2) animalFat++;
            }

            Item meatItem = itemList.CreateNewItem("raw_meat", meat);
            Item leatherItem = itemList.CreateNewItem("leather", leather);
            Item animalFatItem = itemList.CreateNewItem("animal_fat", animalFat);

            PlaceItemToList(meatItem);
            PlaceItemToList(leatherItem);
            PlaceItemToList(animalFatItem);
            return;
        }

        for (int i = 0; i < GetRandomCount(); i++)
        {
            string randomNick = SelectItem();
            if (randomNick == null) continue;
            int randomAmount = SelectAmount(randomNick);

            Item item = itemList.CreateNewItem(randomNick, randomAmount);
            if (item == null) continue;
            PlaceItemToList(item);
        }
    }
    private void PlaceItemToList(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return;
            }
        }
    }
    private int GetRandomCount()
    {
        return Random.Range(minimumItemCount, maximumItemCount + 1);
    }
    private List<LootChance> SelectList()
    {
        List<LootChance> loots = new List<LootChance>();

        switch (lootType)
        {
            case Loots.Box: loots.AddRange(boxItems); break;
            case Loots.MilitaryChest: loots.AddRange(militaryChestItems); break;
            case Loots.Airdrop: loots.AddRange(airdropItems); break;
            case Loots.Fridge: loots.AddRange(fridgeItems); break;
            case Loots.FoodBox: loots.AddRange(foodBoxItems); break;
            case Loots.Mine: loots.AddRange(mineItems); break;
            case Loots.Fuel: loots.AddRange(fuelItems); break;
            case Loots.Building: loots.AddRange(buildingItems); break;
            case Loots.BirdNest: loots.AddRange(birdNestItems); break;
            case Loots.Zombie: loots.AddRange(zombieLoot); break;
            case Loots.Deer: loots.AddRange(deerLoot); break;
        }
        return loots;
    }
    private string SelectItem()
    {
        int random = Random.Range(0, 100);

        List<LootChance> loots = SelectList();

        if(loots.Count <= 0) return null;

        while(random >= 0)
        {
            random -= loots[0].percent;

            if(random <= 0)
            {
                return loots[0].nickName;
            }

            loots.RemoveAt(0);
        }

        return null;
    }
    private int SelectAmount(string nickName)
    {
        List<LootChance> loots = SelectList();

        if (loots.Count <= 0) return 0;

        var item = loots.Find(x => x.nickName == nickName);

        return Random.Range(item.minAmount, item.maxAmount + 1);
    }
    public void SetPanelOpen(bool value)
    {
        isPanelOpen = value;

        if(value) firstOpen = true;
    }
    public void LoadItems(SaveInventory loaded)
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i] = itemList.CreateNewItem(loaded.nickName[i], loaded.amount[i], loaded.durability[i], loaded.ammoInside[i]);
        }
    }
}

public struct LootChance
{
    public string nickName;
    public int minAmount;
    public int maxAmount;
    public int percent;
}
public enum Loots
{
    Box,
    MilitaryChest,
    Airdrop,
    Fridge,
    FoodBox,
    Mine,
    Fuel,
    Building,
    BirdNest,
    Zombie,
    Deer
}
