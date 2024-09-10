using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    private Translations translations;

    public List<Item> items = new List<Item>();

    void Awake()
    {
        translations = GetComponent<Translations>();

        CreateTool("hammer", 100, 100);
        CreateWeapon("pistol", 150, 150, 13, 13, "9mm_ammo");
        CreateWeapon("assault_rifle", 300, 300, 30, 30, "7.62mm_ammo");
        CreateWeapon("shotgun", 200, 200, 8, 8, "shotgun_shell");
        CreateWeapon("hunting_rifle", 400, 400, 5, 5, "7.62mm_ammo");
        CreateWeapon("bow", 75, 75, 1, 1, "arrow");
        CreateWeapon("crossbow", 100, 100, 1, 1, "arrow");
        CreateCloseRange("axe", 100, 100);
        CreateCloseRange("pickaxe", 250, 250);
        CreateCloseRange("sword", 75, 75);
        CreateCloseRange("baseball_bat", 125, 125);
        CreateCloseRange("spear", 100, 100);
        CreateAmmo("9mm_ammo", 1, 100);
        CreateAmmo("7.62mm_ammo", 1, 100);
        CreateAmmo("shotgun_shell", 1, 100);
        CreateAmmo("arrow", 1, 30);
        CreateExplosive("dynamite", 1, 10, BlockTypes.Empty, PlaceableGrounds.None);
        CreateExplosive("landmine", 1, 10, BlockTypes.Mine, PlaceableGrounds.Terrain);
        CreateMaterial("coffee_bean", 1, 10);
        CreateMaterial("tape", 1, 10);
        CreateMaterial("glue", 1, 10);
        CreateMaterial("oil", 1, 10);
        CreateMaterial("gasoline", 1, 1000);
        CreateMaterial("plastic", 1, 100);
        CreateMaterial("screw", 1, 20);
        CreateMaterial("cloth", 1, 20);
        CreateMaterial("cream", 1, 10);
        CreateMaterial("paper", 1, 100);
        CreateMaterial("feather", 1, 100);
        CreateMaterial("pan", 1, 1);
        CreateMaterial("soil", 1, 20);
        CreateMaterial("bullet_mold", 1, 100);
        CreateMaterial("bullet_tip", 1, 100);
        CreateMaterial("empty_bottle", 1, 10);
        CreateMaterial("animal_fat", 1, 20);
        CreateMaterial("leather", 1, 20);
        CreateMaterial("powder", 1, 1000);
        CreateMaterial("chemical", 1, 100);
        CreateMaterial("salt", 1, 10);
        CreateMaterial("spice", 1, 10);
        CreateMaterial("flour", 1, 20);
        CreateMaterial("rope", 1, 20);
        CreateMaterial("sewing_kit", 1, 10);
        CreateMaterial("scope", 1, 1);
        CreateMaterial("broken_bottle", 1, 10);
        CreateMaterial("cotton", 1, 20);
        CreateMaterial("cucumber_extract", 1, 10);
        CreateMaterial("pan", 1, 10);
        CreateRawMaterial("wood", 1, 1000);
        CreateRawMaterial("stick", 1, 100);
        CreateRawMaterial("stone", 1, 1000);
        CreateRawMaterial("scrap_metal", 1, 1000);
        CreateRawMaterial("coal", 1, 1000);
        CreateRawMaterial("sulfur", 1, 1000);
        CreateRawMaterial("forged_iron", 1, 1000);
        CreateRawMaterial("forged_sulfur", 1, 1000);
        CreateRawMaterial("coin", 1, 1000);   
        CreateBlock("wood_frame", 1, 100, 25, 25, BlockTypes.Frame, PlaceableGrounds.All, true, MaterialType.Frame);
        CreateBlock("wood_block", 1, 100, 250, 250, BlockTypes.Frame, PlaceableGrounds.All, true, MaterialType.Wood);
        CreateBlock("stone_block", 1, 100, 500, 500, BlockTypes.Frame, PlaceableGrounds.All, true, MaterialType.Stone);
        CreateBlock("iron_block", 1, 100, 1000, 1000, BlockTypes.Frame, PlaceableGrounds.All, true, MaterialType.Iron);
        CreateBlock("wood_stair", 1, 100, 200, 200, BlockTypes.Stair, PlaceableGrounds.All, true, MaterialType.Wood);
        CreateBlock("stone_stair", 1, 100, 400, 400, BlockTypes.Stair, PlaceableGrounds.All, true, MaterialType.Stone);
        CreateBlock("iron_stair", 1, 100, 800, 800, BlockTypes.Stair, PlaceableGrounds.All, true, MaterialType.Iron);
        CreateBlock("wood_door", 1, 1, 350, 350, BlockTypes.Door, PlaceableGrounds.Frames, true, MaterialType.Wood);
        CreateBlock("iron_door", 1, 1, 1200, 1200, BlockTypes.Door, PlaceableGrounds.Frames, true, MaterialType.Iron);
        CreateBlock("glass", 1, 100, 100, 100, BlockTypes.Glass, PlaceableGrounds.All, false, MaterialType.Empty);
        CreateBlock("campfire", 1, 1, 50, 50, BlockTypes.Campfire, PlaceableGrounds.All, false, MaterialType.Empty);
        CreateBlock("furnace", 1, 1, 200, 200, BlockTypes.Furnace, PlaceableGrounds.Frames, false, MaterialType.Empty);
        CreateBlock("sleeping_bag", 1, 1, 100, 100, BlockTypes.SleepingBag, PlaceableGrounds.All, false, MaterialType.Empty);
        CreateBlock("chest", 1, 1, 250, 250, BlockTypes.Chest, PlaceableGrounds.Frames, false, MaterialType.Empty);
        CreateBlock("farming_plot", 1, 10, 100, 100, BlockTypes.FarmingPlot, PlaceableGrounds.All, false, MaterialType.Empty);
        CreateBlock("ladder", 1, 10, 200, 200, BlockTypes.Ladder, PlaceableGrounds.Frames, false, MaterialType.Empty);
        CreateBlock("wood_spike", 1, 10, 500, 500, BlockTypes.WoodSpike, PlaceableGrounds.Terrain, false, MaterialType.Empty);
        CreateFoodOrWater("tomato", 1, 10, 5, 7, 1, Categories.Food);
        CreateFoodOrWater("pepper", 1, 10, 2, 0, 0, Categories.Food);
        CreateFoodOrWater("corn", 1, 10, 4, 0, 1, Categories.Food);
        CreateFoodOrWater("potato", 1, 10, 6, 0, 1, Categories.Food);
        CreateFoodOrWater("mushroom", 1, 10, 3, 0, 0, Categories.Food);
        CreateFoodOrWater("onion", 1, 10, 3, 0, 0, Categories.Food);
        CreateFoodOrWater("cucumber", 1, 10, 5, 5, 1, Categories.Food);
        CreateFoodOrWater("egg", 1, 10, 5, -5, 0, Categories.Food);
        CreateFoodOrWater("raw_meat", 1, 10, 5, -30, -20, Categories.Food);
        CreateFoodOrWater("bottled_water", 1, 10, 0, 15, 0, Categories.Water);
        CreateFoodOrWater("dirty_water", 1, 10, 0, 10, -20, Categories.Water);
        CreateFoodOrWater("cola", 1, 10, 0, 20, 0, Categories.Water);
        CreateFoodOrWater("soda", 1, 10, 0, 20, 0, Categories.Water);
        CreateFoodOrWater("fruit_juice", 1, 10, 0, 20, 5, Categories.Water);
        CreateFoodOrWater("rosehip", 1, 100, 3, 2, 1, Categories.Food);
        CreateFoodOrWater("milk", 1, 10, 5, 15, 10, Categories.Water);
        CreateFoodOrWater("canned_pea", 1, 10, 15, 15, 15, Categories.Food);
        CreateFoodOrWater("canned_bean", 1, 10, 20, 10, 15, Categories.Food);
        CreateFoodOrWater("canned_fish", 1, 10, 35, 0, 20, Categories.Food);
        CreateFoodOrWater("grilled_meat", 1, 10, 30, 0, 2, Categories.Food);
        CreateFoodOrWater("coffee", 1, 10, 0, 30, 10, Categories.Water);
        CreateFoodOrWater("tomato_soup", 1, 10, 40, 30, 20, Categories.Food);
        CreateFoodOrWater("fried_potatoes", 1, 10, 30, -5, 0, Categories.Food);
        CreateFoodOrWater("steak_with_vegetables", 1, 10, 70, 0, 20, Categories.Food);
        CreateFoodOrWater("pizza", 1, 10, 40, 0, 15, Categories.Food);
        CreateFoodOrWater("potato_chips", 1, 10, 20, -5, 0, Categories.Food);
        CreateFoodOrWater("corn_chips", 1, 10, 20, -5, 0, Categories.Food);
        CreateFoodOrWater("mushroom_onion_rings", 1, 10, 30, 0, 5, Categories.Food);
        CreateFoodOrWater("boiled_tomato_salad", 1, 10, 15, 5, 10, Categories.Food);
        CreateFoodOrWater("potato_balls", 1, 10, 40, 0, 3, Categories.Food);
        CreateFoodOrWater("pancake", 1, 10, 30, 0, 2, Categories.Food);
        CreateFoodOrWater("meat_skewer", 1, 10, 30, -5, 10, Categories.Food);
        CreateFoodOrWater("corn_bread", 1, 10, 10, 0, 5, Categories.Food);
        CreateFoodOrWater("pasta_with_sauce", 1, 10, 40, 2, 5, Categories.Food);
        CreateFoodOrWater("rosehip_tea", 1, 10, 5, 25, 25, Categories.Water);
        CreateFoodOrWater("meat_stew", 1, 10, 80, 50, 40, Categories.Food);
        CreateHealth("bandage", 1, 10, 0, 0, 10);
        CreateHealth("first_aid_kit", 1, 10, 0, 0, 50);
        CreateHealth("syringe", 1, 10, 15, 10, 30);
        CreateHealth("painkiller", 1, 10, 0, -20, 60);
        CreateHealth("splint", 1, 10, 0, 0, 0);
        CreateSeed("cotton_seed", 1, 10);
        CreateSeed("potato_seed", 1, 10);
        CreateSeed("tomato_seed", 1, 10);
        CreateSeed("corn_seed", 1, 10);
        CreateSeed("mushroom_seed", 1, 10);
        CreateSeed("onion_seed", 1, 10);
        CreateSeed("pepper_seed", 1, 10);
        CreateSeed("cucumber_seed", 1, 10);
        CreateSeed("rosehip_seed", 1, 10);
    }
    private void CreateWeapon(string nickName, int durability, int maxDurability, int bulletInside, int maxAmmo, string ammoTypeNick)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName),translations.GetItemDescription(nickName),
            1, 1, durability, maxDurability, bulletInside, maxAmmo, ammoTypeNick, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Weapon));
    }
    private void CreateCloseRange(string nickName, int durability, int maxDurability)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            1, 1, durability, maxDurability, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None, false,
            MaterialType.Empty, Categories.CloseRange));
    }
    private void CreateAmmo(string nickName, int amount, int maxAmount)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Ammo));
    }
    private void CreateFoodOrWater(string nickName, int amount, int maxAmount, int foodValue, int waterValue, int healthValue, Categories foodOrWater)
    {
        string itemName = translations.GetItemName(nickName);
        string itemDescription = translations.GetItemDescription(nickName) + WriteStats(foodValue, waterValue, healthValue);

        items.Add(new Item(nickName, CreateID(), itemName, itemDescription, amount, maxAmount, 0, 0, 0, 0, null, 
            foodValue, waterValue, healthValue, BlockTypes.Empty, PlaceableGrounds.None, false, MaterialType.Empty, foodOrWater));
    }
    private void CreateHealth(string nickName, int amount, int maxAmount, int foodValue, int waterValue, int healthValue)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, foodValue, waterValue, healthValue, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Health));
    }
    private void CreateBlock(string nickName, int amount, int maxAmount, int durability, int maxDurability, BlockTypes blockType, PlaceableGrounds placeableGrounds, bool upgradeable, MaterialType materialType)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, durability, maxDurability, 0, 0, null, 0, 0, 0, blockType, placeableGrounds,
            upgradeable, materialType, Categories.Block));
    }
    private void CreateTool(string nickName, int durability, int maxDurability)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            1, 1, durability, maxDurability, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Tools));
    }
    private void CreateRawMaterial(string nickName, int amount, int maxAmount)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.RawMaterial));
    }
    private void CreateExplosive(string nickName, int amount, int maxAmount, BlockTypes blockType, PlaceableGrounds placeableGrounds)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, 0, 0, 0, blockType, placeableGrounds,
            false, MaterialType.Empty, Categories.Explosive));
    }
    private void CreateMaterial(string nickName, int amount, int maxAmount)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Material));
    }
    private void CreateSeed(string nickName, int amount, int maxAmount)
    {
        items.Add(new Item(nickName, CreateID(), translations.GetItemName(nickName), translations.GetItemDescription(nickName),
            amount, maxAmount, 0, 0, 0, 0, null, 0, 0, 0, BlockTypes.Empty, PlaceableGrounds.None,
            false, MaterialType.Empty, Categories.Seed));
    }
    private int CreateID()
    {
        return items.Count;
    }
    private string WriteStats(int food, int water, int health)
    {
        string text = "\n\n" +
            translations.Get("HealthValue") + health + "\n" +
            translations.Get("FoodValue") + food + "\n" +
            translations.Get("WaterValue") + water;

        return text;
    }
    public Item CreateNewItem(int id, int amount)
    {
        return new Item(items[id].nickName, id, items[id].name, items[id].description, amount, items[id].maxAmount,
            items[id].durability, items[id].maxDurability, items[id].ammoInside, items[id].maxAmmoInside, items[id].ammoTypeNick,
            items[id].foodValue, items[id].waterValue, items[id].healthValue, items[id].blockType, items[id].placeableGrounds,
            items[id].upgradeable, items[id].materialType, items[id].category);
    }
    public Item CreateNewItem(string nickName, int amount)
    {
        var item = items.Find(x => x.nickName == nickName);
        if (item == null) return null;

        return CreateNewItem(item.id, amount);
    }
    public Item CreateNewItem(string nickName, int amount, int durability, int ammoInside)
    {
        var item = items.Find(x => x.nickName == nickName);
        if (item == null) return null;

        return new Item(item.nickName, item.id, item.name, item.description, amount, item.maxAmount, durability, item.maxDurability,
            ammoInside, item.maxAmmoInside, item.ammoTypeNick,
            item.foodValue, item.waterValue, item.healthValue, item.blockType, item.placeableGrounds,
            item.upgradeable, item.materialType, item.category);
    }

    public Item GetItem(string nickName)
    {
        var item = items.Find(x => x.nickName == nickName);
        if(item == null) return null;

        return item;
    }
    public Item GetUpgradeBlock(BlockTypes blockType, MaterialType materialType)
    {
        var item = items.Find(x => x.blockType == blockType && x.materialType == materialType);
        if (item == null) return null;

        return item;
    }
}