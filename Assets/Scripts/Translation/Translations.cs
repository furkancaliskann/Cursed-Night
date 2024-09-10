using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Translations : MonoBehaviour
{
    private List<TranslationClass> translations = new List<TranslationClass>();
    private List<TranslationItemClass> translationItems = new List<TranslationItemClass>();

    [SerializeField] private List<ConstText> constTexts = new List<ConstText>();

    private string currentLanguage = "";

    void Awake()
    {
        SetConstVariables();
        Place();
        PlaceItem();

        currentLanguage = LanguageSettings.language;
        RefreshConsts();
    }
    void Update()
    {
        CheckLanguage();
    }
    private void CheckLanguage()
    {
        if (LanguageSettings.language == currentLanguage) return;

        currentLanguage = LanguageSettings.language;
        RefreshConsts();
    }
    private void SetConstVariables()
    {
        for (int i = 0; i < constTexts.Count; i++)
        {
            Text text = constTexts[i].GetComponent<Text>();
            constTexts[i].SetConstVariable(text);
        }
    }
    private void RefreshConsts()
    {
        for (int i = 0; i < constTexts.Count; i++)
        {
            constTexts[i].SetText(Get(constTexts[i].key));
        }
    }
    private void Place()
    {
        // Item
        Add("HealthValue", "Health Value : ", "Sa�l�k De�eri : ");
        Add("FoodValue", "Food Value : ", "Yiyecek De�eri : ");
        Add("WaterValue", "Water Value : ", "Su De�eri : ");

        // Car Canvas
        Add("CarSpeed", "Speed : ", "H�z : ");
        Add("CarFuel", "Fuel : ", "Yak�t : ");
        Add("CarDurability", "Durability : ", "Dayan�kl�l�k : ");

        // Car Controller
        Add("RadioTurnedOff", "The radio was turned off.", "Radyo kapat�ld�.");

        // Console
        Add("WrongCommandUsage", "Wrong command usage !", "Yanl�� komut kullan�m� !");
        Add("UnknownCommand", "Unknown command !", "Bilinmeyen komut !");
        Add("DayValueUpdated", "Day value updated !", "G�n de�eri g�ncellendi !");
        Add("MinuteValueUpdated", "Minute value updated !", "Dakika de�eri g�ncellendi !");
        Add("HourValueUpdated", "Hour value updated !", "Saat de�eri g�ncellendi !");
        Add("NewRunningSpeed", "New running speed : ", "Yeni ko�u h�z� : ");
        Add("FpsCounterTurnedOff", "Fps counter turned off!", "Fps g�stergesi kapat�ld�!");
        Add("FpsCounterTurnedOn", "Fps counter turned on!", "Fps g�stergesi a��ld�!");
        Add("ItemAdded", "Item added!", "E�ya eklendi!");
        Add("EnergyValueChanged", "Energy value changed!", "Enerji de�eri de�i�tirildi!");
        Add("FoodValueChanged", "Food value changed!", "Yiyecek de�eri de�i�tirildi!");
        Add("WaterValueChanged", "Water value changed!", "Su de�eri de�i�tirildi!");
        Add("TimeSpeedChanged", "Time speed changed!", "Zaman h�z� de�i�tirildi!");
        Add("SuicideCommand", "Player died!", "Oyuncu �ld�!");
        Add("RespawnCommand", "Player respawned!", "Oyuncu yeniden do�du!");
        Add("MenuCommand", "Returning to the main menu..", "Ana men�ye d�n�l�yor..");
        Add("AirdropComing", "Airdrop is on the way!", "Hava deste�i yolda!");
        Add("AirdropFailed", "Wait for the previous airdrop to complete!", "�nceki hava deste�inin tamamlanmas�n� bekleyin!");
        Add("SensitivityChanged", "Mouse sensitivity has been changed.", "Fare hassasiyeti de�i�tirildi.");
        Add("QuitCommand", "Exiting the game..", "Oyundan ��k�� yap�l�yor..");

        // Player
        Add("GodModeActivated", "God mode activated !", "Tanr� modu a��ld� !");
        Add("GodModeDeactivated", "God mode deactivated !", "Tanr� modu kapat�ld� !");

        // Raycast
        Add("CollectMushroomDialog", "Press 'E' to collect mushroom.", "Mantar� toplamak i�in 'E' tu�una bas�n.");
        Add("CollectWoodPileDialog", "Press 'E' to collect wood.", "Odunu yerden almak i�in 'E' tu�una bas�n.");
        Add("CollectStoneDialog", "Press 'E' to collect stone.", "Ta�� yerden almak i�in 'E' tu�una bas�n.");
        Add("OpenCampfireDialog", "Press 'E' to open campfire panel.", "Kamp ate�i panelini a�mak i�in 'E' tu�una bas�n.");
        Add("OpenFurnaceDialog", "Press 'E' to open furnace panel.", "F�r�n panelini a�mak i�in 'E' tu�una bas�n.");
        Add("OpenBackpackDialog", "Press 'E' to open backpack.", "�antay� a�mak i�in 'E' tu�una bas�n.");
        Add("OpenChestDialog", "Press 'E' to open chest.", "Sand��� a�mak i�in 'E' tu�una bas�n.");
        Add("OpenMaterialBoxDialog", "Press 'E' to open material box.", "Malzeme kutusunu a�mak i�in 'E' tu�una bas�n.");
        Add("Durability", "Durability : ", "Dayan�kl�l�k : ");
        Add("PlantDialog", "Press 'E' to plant.", "Tarlay� ekmek i�in 'E' tu�una bas�n.");
        Add("PlantNeedWater", "Plant (Need Water)", "Ekin (Sulama Gerekli)");
        Add("PlantWaterDialog", "Press 'E' to water the plant", "Bitkiyi sulamak i�in 'E' tu�una bas�n.");
        Add("PlantGrowingDialog", "Plant growing.", "Ekin b�y�yor.");
        Add("HarvestDialog", "Press 'E' to harvest", "Bitkiyi hasat etmek i�in 'E' tu�una bas�n.");
        Add("Refill", "Refill", "Doldur");
        Add("FuelAndCost", "% fuel.\n Cost : ", "% yak�t. \n T�ketir : ");
        Add("Gasoline", "Gasoline", "Benzin");
        Add("GasTankFull", "The gas tank is full!", "Yak�t tank� tamamen dolu!");
        Add("GetWoodFrame", "Press 'E' to get wood frame.", "Odun �er�eveyi almak i�in 'E' tu�una bas�n.");
        Add("FillWater", "Press 'E' to fill water.", "Su doldurmak i�in 'E' tu�una bas�n.");
        Add("Car", "Press 'E' to drive car. \n Press 'F' to open trunk.", "Arabay� s�rmek i�in 'E' tu�una bas�n. \n Bagaj� a�mak i�in 'F' tu�una bas�n.");
        Add("OpenLootDialog", "Press 'E' to open.", "A�mak i�in 'E' tu�una bas�n.");

        // SavedSlots
        Add("SavedSlotSave", "Save", "Kay�t");
        Add("SavedSlotFull", "Full", "Dolu");
        Add("SavedSlotEmpty", "Empty", "Bo�");

        // TimeCanvas
        Add("Day", "Day : ", "G�n : ");

        // ConstTexts (Main Menu)
        Add("STARTGAME", "START GAME", "OYUNA BA�LA");
        Add("SETTINGS", "SETTINGS", "AYARLAR");
        Add("KEYBOARD", "KEYBOARD", "KLAVYE");
        Add("NEWS", "NEWS", "HABERLER");
        Add("QUIT", "QUIT", "�IKI�");
        Add("NEW_GAME_HEADER", "- NEW GAME -", "- YEN� OYUN -");
        Add("DeleteSelected", "Delete selected", "Se�ili kayd� sil");
        Add("LoadSelected", "Load selected", "Se�ili kayd� ba�lat");
        Add("NewGame", "New game", "Yeni oyun");
        Add("Difficulty", "Difficulty :", "Zorluk :");
        Add("LootAbundance", "Loot Abundance :", "Ganimet Bollu�u :");
        Add("BlockDamage", "Block Damage :", "Blok Hasar� :");
        Add("StartGame", "Start Game", "Oyunu Ba�lat");
        Add("SETTINGS_HEADER", "- SETTINGS -", "- AYARLAR -");
        Add("ScreenResolution", "Screen Resolution :", "Ekran ��z�n�rl��� :");
        Add("ScreenMode", "Screen Mode :", "Ekran Modu :");
        Add("GraphicsQuality", "Graphics Quality :", "Grafik Kalitesi :");
        Add("FixedFrameRate", "Fixed Frame Rate :", "Sabit Kare H�z� :");
        Add("Language", "Language :", "Dil :");
        Add("Sensitivity", "Sensitivity :", "Fare Hassasiyeti :");
        Add("FpsCounter", "Fps Counter :", "Fps G�stergesi :");
        Add("SaveChanges", "Save Changes", "De�i�iklikleri Kaydet");
        Add("On", "On", "A��k");
        Add("Off", "Off", "Kapal�");
        Add("KEYBOARD_HEADER", "- KEYBOARD -", "- KLAVYE -");
        Add("MoveForward", "Move forward", "�leriye hareket et");
        Add("MoveLeft", "Move left", "Sola hareket et");
        Add("MoveBack", "Move back", "Geriye hareket et");
        Add("MoveRight", "Move right", "Sa�a hareket et");
        Add("KeyR", "Reload / Change Radio Channel", "�arj�r de�i�tir / Radyo kanal�n� de�i�tir");
        Add("Space", "Space", "Bo�luk");
        Add("Jump", "Jump", "Z�pla");
        Add("Crouch", "Crouch", "E�il");
        Add("MouseScroll", "Mouse Scroll", "Fare Tekerle�i");
        Add("MouseScrollDesc", "Change the selected slot", "Se�ili slotu de�i�tir");
        Add("InteractObjects", "Interact with objects", "Nesnelerle etkile�ime ge�");
        Add("ToggleCar", "Toggle car chest", "Ara� sand���n� a�/kapat");
        Add("ToggleMap", "Toggle map", "Haritay� a�/kapat");
        Add("ToggleInventory", "Toggle inventory", "Envanteri a�/kapat");
        Add("ToggleConsole", "Toggle console", "Konsolu a�/kapat");
        Add("LeftClick", "Left Click", "Sol T�k");
        Add("LeftClickDesc", "Use the primary function of the selected item", "Se�ili ��enin birincil fonksiyonunu kullan");
        Add("RightClick", "Right Click", "Sa� T�k");
        Add("RightClickDesc", "Fast shoot / Zoom / Use selected item", "H�zl� at�� / Yak�nla�t�r / Se�ili e�yay� kullan");
        Add("LOADING", "LOADING..", "Y�KLEN�YOR..");

        // ConstTexts (Game)
        Add("BACKPACK_HEADER", "BACKPACK", "SIRT �ANTASI");
        Add("CHEST_HEADER", "CHEST", "SANDIK");
        Add("CRAFTING_PANEL_HEADER", "CRAFTING PANEL", "�RET�M PANEL�");
        Add("FURNACE_PANEL_FUEL_HEADER", "FUEL", "YAKIT");
        Add("FURNACE_PANEL_SMELTED_HEADER", "SMELTED", "ER�T�LD�");
        Add("FURNACE_PANEL_SMELT_HEADER", "SMELT", "ER�T�LECEK");
        Add("START_STOP_FIRE", "START/STOP FIRE", "ATE�� BA�LAT/DURDUR");
        Add("DESCRIPTION", "DESCRIPTION", "A�IKLAMA");
        Add("CRAFT", "CRAFT", "�RET");
        Add("REQUIRED_MATERIALS", "REQUIRED MATERIALS", "GEREKL� MALZEMELER");
        Add("CRAFTING_PANEL_HEADER", "CRAFTING PANEL", "�RET�M PANEL�");
        Add("INVENTORY_HEADER", "INVENTORY", "ENVANTER");
        Add("LOOT_HEADER", "LOOT", "GAN�MET");
        Add("Resume", "Resume", "Oyuna Geri D�n");
        Add("Respawn", "Respawn", "Yeniden Do�");
        Add("RespawnRegisteredPoint", "Respawn Registered Point", "Kay�tl� Noktada Yeniden Do�");
        Add("MainMenu", "Main Menu", "Men�ye D�n");
        Add("YOU_DIED", "YOU DIED.", "�LD�N.");
    }
    private void PlaceItem()
    {
        AddItem("7.62mm_ammo", "7.62mm Ammo", "Assault and sniper rifle ammunition.", "7.62mm Mermi", "Taarruz ve ni�anc� t�fe�ine ait m�himmat.");
        AddItem("9mm_ammo", "9mm Ammo", "Ammunition for pistol.", "9mm Mermi", "Tabanca i�in m�himmat.");
        AddItem("animal_fat", "Animal Fat", "Maybe you can produce some gasoline.", "Hayvan Ya��", "Belki biraz benzin �retebilirsin.");
        AddItem("arrow", "Arrow", "Ammunition for bows.", "Ok", "Germeli silahlar i�in m�himmat.");
        AddItem("assault_rifle", "Assault Rifle", "A powerful and automatic weapon.", "Taarruz T�fe�i", "G��l� ve otomatik bir silah.");
        AddItem("axe", "Axe", "Ideal hand tool for cutting trees.", "Balta", "A�a� kesmek i�in ideal bir el aleti.");
        AddItem("bandage", "Bandage", "Stops bleeding, slightly increases health.", "Bandaj", "Kanamalar� durdurur, sa�l��� biraz art�r�r.");
        AddItem("baseball_bat", "Baseball Bat", "Medium damaged stick.", "Beyzbol Sopas�", "Orta hasarl� sopa.");
        AddItem("boiled_tomato_salad", "Boiled Tomato Salad", "Even though it doesn't taste good, it is nutritious.", "Ha�lanm�� Domates Salatas�", "Tad� g�zel olmasa da besleyicidir.");
        AddItem("bottled_water", "Water Bottle", "Water is an essential source of survival.", "�i�e Su", "Su temel bir hayatta kalma kayna��d�r.");
        AddItem("bow", "Bow", "A primitive but effective bow.", "Yay", "�lkel ama etkili bir yay.");
        AddItem("broken_bottle", "Broken Bottle", "A shattered bottle.", "K�r�k �i�e", "Par�alanm�� halde bir �i�e.");
        AddItem("bullet_mold", "Bullet Mold", "It is the mold required for bullet production.", "Mermi Kal�b�", "Mermi �retimi i�in gerekli olan kal�pt�r.");
        AddItem("bullet_tip", "Bullet Tip", "It is the tip required for bullet production.", "Mermi Ucu", "Mermi �retimi i�in gerekli olan u�tur.");
        AddItem("campfire", "Campfire", "Ideal for cooking and finding some peace.", "Kamp Ate�i", "Yemek pi�irmek ve biraz huzur bulmak i�in ideal.");
        AddItem("canned_bean", "Canned Bean", "A nutritious preserve.", "Konserve Fasulye", "Besleyici bir konserve.");
        AddItem("canned_fish", "Canned Fish", "A nutritious preserve.", "Konserve Bal�k", "Besleyici bir konserve.");
        AddItem("canned_pea", "Canned Pea", "A nutritious preserve.", "Konserve Bezelye", "Besleyici bir konserve.");
        AddItem("chemical", "Chemical", "Used in production.", "Kimyasal", "�retimde kullan�l�r.");
        AddItem("chest", "Chest", "An area where you can store items.", "Sand�k", "��erisine e�ya depolayabilece�iniz bir alan.");
        AddItem("cloth", "Cloth", "A soft cloth.", "Kuma�", "Yumu�ac�k bir kuma�.");
        AddItem("coal", "Coal", "Material used to make gunpowder.", "K�m�r", "Barut yapmak i�in kullan�lan malzeme.");
        AddItem("coffee", "Coffee", "A delicious beverage made from coffee beans.", "Kahve", "Kahve �ekirdeklerinden yap�lm��, enfes bir i�ecek.");
        AddItem("coffee_bean", "Coffee Bean", "Use some water to make coffee.", "Kahve �ekirde�i", "Kahve yapmak i�in biraz su kullan�n.");
        AddItem("coin", "Coin", "It is now only used for smelting in the furnace.", "Madeni Para", "Art�k sadece f�r�nda eritmek i�in kullan�l�yor.");
        AddItem("cola", "Cola", "It's a delicious drink.", "Kola", "Leziz bir i�ecek.");
        AddItem("corn", "Corn", "You can make a variety of dishes using some corn.", "M�s�r", "Biraz m�s�r kullanarak �e�itli yemekler yapabilirsin.");
        AddItem("corn_bread", "Corn Bread", "Delicious bread made using some corn.", "M�s�r Ekme�i", "Biraz m�s�r kullan�larak yap�lan leziz ekmek.");
        AddItem("corn_chips", "Corn Chips", "Another delicious variety of chips.", "M�s�r Cipsi", "Bir ba�ka leziz cips �e�idi.");
        AddItem("corn_seed", "Corn Seed", "It is used to grow corn.", "M�s�r Tohumu", "M�s�r yeti�tirmek i�in kullan�l�r.");
        AddItem("cotton", "Cotton", "It is used in cloth production.", "Pamuk", "Kuma� �retiminde kullan�l�r.");
        AddItem("cotton_seed", "Cotton Seed", "It is used to grow cotton.", "Pamuk Tohumu", "Pamuk yeti�tirmek i�in kullan�l�r.");
        AddItem("cucumber", "Cucumber", "It is used in foods.", "Salatal�k", "Yemeklerde kullan�l�r.");
        AddItem("cucumber_extract", "Cucumber Extract", "It is used in making cream.", "Salatal�k �z�", "Krem yap�m�nda kullan�l�r.");
        AddItem("cucumber_seed", "Cucumber Seed", "It is used to grow cucumber.", "Salatal�k Tohumu", "Salatal�k yeti�tirmek i�in kullan�l�r.");
        AddItem("cream", "Cream", "It is used in first aid kit.", "Krem", "�lk yard�m kitinde kullan�l�r.");
        AddItem("crossbow", "Crossbow", "It is slower than the regular bow, but it shoots more effectively.", "Kundakl� Yay", "Normal Yay'a g�re daha yava�t�r ama daha etkili at��lar yapar.");
        AddItem("dirty_water", "Dirty Water", "Unreliable water. It must be boiled to drink.", "Kirli Su", "G�venilmez bir su. ��mek i�in kaynat�lmal�d�r.");
        AddItem("dynamite", "Dynamite", "You can blow up doors and walls with this explosive.", "Dinamit", "Bu patlay�c� ile kap� ve duvarlar� patlatabilirsin.");
        AddItem("egg", "Egg", "It is nutritious but should be used in meals.", "Yumurta", "Besleyici ama yemeklerde kullan�lmal�.");
        AddItem("empty_bottle", "Empty Bottle", "An empty bottle. Maybe you can fill it with water.", "Bo� �i�e", "Bo� bir �i�e. Belki su doldurabilirsin.");
        AddItem("farming_plot", "Farming Plot", "It is used to grow seeds.", "Tar�m Arsas�", "Tohumlar� yeti�tirmek i�in kullan�l�r.");
        AddItem("feather", "Feather", "A common material used in arrow making.", "Ku� T�y�", "Ok yap�m�nda kullan�lan yayg�n bir malzeme.");
        AddItem("first_aid_kit", "First Aid Kit", "Stops bleeding, increases health at a high rate.", "�lk Yard�m Kiti", "Kanamalar� durdurur, sa�l��� y�ksek oranda art�r�r.");
        AddItem("flour", "Flour", "It is used in dough dishes.", "Un", "Hamurlu yemeklerde kullan�l�r.");
        AddItem("forged_iron", "Forged Iron", "It is used in production.", "D�vme Demir", "�retimde kullan�l�r.");
        AddItem("forged_sulfur", "Forged Sulfur", "It is used in gunpowder production.", "D�vme S�lf�r", "Barut �retiminde kullan�l�r.");
        AddItem("fried_potatoes", "Fried Potatoes", "Children's favorite food.", "Patates K�zartmas�", "�ocuklar�n favori yiyece�i.");
        AddItem("fruit_juice", "Fruit Juice", "A delicious and vitamin-containing beverage.", "Meyve Suyu", "Leziz ve vitamin i�eren bir i�ecek.");
        AddItem("furnace", "Furnace", "It is used to smelt raw materials.", "F�r�n", "Ham malzemeleri eritmek i�in kullan�l�r.");
        AddItem("gasoline", "Gasoline", "Fuel source for car.", "Benzin", "Araba i�in yak�t kayna��.");
        AddItem("glass", "Glass", "It is indispensable for a home.", "Cam", "Bir evin olmazsa olmaz�d�r.");
        AddItem("glue", "Glue", "It is used in the manufacture of certain items.", "Yap��t�r�c�", "Baz� ��elerin �retiminde kullan�l�r.");
        AddItem("grilled_meat", "Grilled Meat", "It looks pretty nutritious.", "Izgara Et", "Olduk�a besleyici g�r�n�yor.");
        AddItem("hammer", "Hammer", "Security lock that can be added to the doors.", "�eki�", "Yap�lar� ta�, metal gibi daha geli�mi� maddelerle kaplar.");
        AddItem("hunting_rifle", "Hunting Rifle", "The indispensable power of long distances.", "Av T�fe�i", "Uzak mesafelerin vazge�ilmez g�c�.");
        AddItem("iron_block", "Iron Block", "It was reinforced with iron.", "Demir Blok", "Demirin g�c� ile ta�land�r�ld�.");
        AddItem("iron_door", "Iron Door", "It is used for entry and exit from the house. It is made of iron which is very durable.", "Demir Kap�", "Eve giri� ��k�� i�in kullan�l�r. �ok dayan�kl� olan demirden �retilmi�tir.");
        AddItem("iron_stair", "Iron Stair", "Used to go higher.", "Demir Basamak", "Daha y�kse�e ��kmak i�in kullan�l�r.");
        AddItem("landmine", "Land Mine", "Military ammunition used to destroy creatures that step on it.", "Kara May�n�", "�zerine basan canl�lar� yok etmek i�in kullan�lan askeri m�himmat.");
        AddItem("ladder", "Ladder", "The best way to climb.", "Merdiven", "T�rmanman�n en iyi yolu.");
        AddItem("leather", "Leather", "Maybe you can craft an armor using this.", "Deri", "Belki bunu kullanarak bir z�rh �retebilirsin.");
        AddItem("meat_skewer", "Meat Skewer", "Where is the barbecue?", "Et �i�", "Mangal nerede?");
        AddItem("meat_stew", "Meat Stew", "A healthy food to fill up on.", "Etli G�ve�", "T�ka basa doymak i�in sa�l�kl� bir besin.");
        AddItem("milk", "Milk", "Moo", "S�t", "M��");
        AddItem("mushroom", "Mushroom", "You can eat it right away or use it to make pizza.", "Mantar", "Hemen yiyebilir veya pizza yapmak i�in kullanabilirsiniz.");
        AddItem("mushroom_onion_rings", "Mushroom Onion Rings", "Delicious onion rings.", "Mantarl� So�an Halkas�", "Leziz so�an halkalar�.");
        AddItem("mushroom_seed", "Mushroom Seed", "It is used to grow mushroom.", "Mantar Tohumu", "Mantar yeti�tirmek i�in kullan�l�r.");
        AddItem("oil", "Oil", "It is used in production.", "Ya�", "�retimde kullan�l�r.");
        AddItem("onion", "Onion", "Spicy, but it is very healthy.", "So�an", "Ac� ama �ok sa�l�kl�.");
        AddItem("onion_seed", "Onion Seed", "It is used to grow onion.", "So�an Tohumu", "So�an yeti�tirmek i�in kullan�l�r.");
        AddItem("painkiller", "Painkiller", "A powerful pain reliever.", "A�r� Kesici", "G��l� bir a�r� kesici.");
        AddItem("painted_block", "Painted Block", "It was decorated with paint.", "Boyama Blok", "Boyayla dekore edilmi�tir.");
        AddItem("pan", "Pan", "Maybe you can cook.", "Tava", "Belki yemek pi�irebilirsin.");
        AddItem("pancake", "Pancake", "Some sugar.", "G�zleme", "Biraz da �eker.");
        AddItem("paper", "Paper", "You definitely won't write anything.", "Ka��t", "Bir �eyler yazmayaca��n kesin.");
        AddItem("pasta_with_sauce", "Pasta With Sauce", "A powerful source of carbohydrates.", "Soslu Makarna", "G��l� bir karbonhidrat kayna��.");
        AddItem("pepper", "Pepper", "It adds spice to your meals.", "Biber", "Yemeklerine ac� katar.");
        AddItem("pepper_seed", "Pepper Seed", "It is used to grow pepper.", "Biber Tohumu", "Biber yeti�tirmek i�in kullan�l�r.");
        AddItem("pickaxe", "Pickaxe", "Expensive hand tool used to break stones.", "Kazma", "Ta�lar� k�rmak i�in kullan�lan, pahal� bir el arac�.");
        AddItem("pistol", "Pistol", "Perfect for scaring your enemies.", "Tabanca", "D��manlar�n� korkutmak i�in birebir.");
        AddItem("pizza", "Pizza", "A unique combination of mushrooms and corn.", "Pizza", "Mantar ve m�s�r�n e�siz uyumu.");
        AddItem("plastic", "Plastic", "It is used in production.", "Plastik", "�retimde kullan�l�r.");
        AddItem("potato", "Potato", "Here is an all-around great food.", "Patates", "��te her y�n�yle muazzam bir yiyecek.");
        AddItem("potato_balls", "Potato Balls", "Potatoes are delicious in every form.", "Patates Toplar�", "Patatesin her hali leziz.");
        AddItem("potato_chips", "Potato Chips", "A delicious and salty chip.", "Patates Cipsi", "Bol tuzlu ve leziz bir cips.");
        AddItem("potato_seed", "Potato Seed", "It is used to grow potato.", "Patates Tohumu", "Patates yeti�tirmek i�in kullan�l�r.");
        AddItem("powder", "Powder", "It is used in bullet production.", "Barut", "Mermi �retiminde kullan�l�r.");
        AddItem("raw_meat", "Raw Meat", "Do not eat without cooking!", "�i� Et", "Pi�irmeden sak�n yemeyin!");
        AddItem("rope", "Rope", "It is used in production.", "�p", "�retimde kullan�l�r.");
        AddItem("rosehip", "Rosehip", "It is used to make rosehip tea.", "Ku�burnu", "Ku�burnu �ay� yap�m�nda kullan�l�r.");
        AddItem("rosehip_seed", "Rosehip Seed", "It is used to grow rosehip.", "Ku�burnu Tohumu", "Ku�burnu yeti�tirmek i�in kullan�l�r.");
        AddItem("rosehip_tea", "Rosehip Tea", "A hot rosehip tea is not always available.", "Ku�burnu �ay�", "S�cak bir ku�burnu �ay� her zaman bulunmaz.");
        AddItem("salt", "Salt", "It can be added to some meals.", "Tuz", "Baz� yemeklere kat�labilir.");
        AddItem("scope", "Scope", "Required to craft a shotgun.", "D�rb�n", "Bir av t�fe�i �retmek i�in gerekli.");
        AddItem("scrap_metal", "Scrap Metal", "Freshly mined pieces of pure metal.", "Hurda Metal", "Yeni kaz�lm�� saf metal par�alar�.");
        AddItem("screw", "Screw", "It is used in production.", "Vida", "�retimde kullan�l�r.");
        AddItem("sewing_kit", "Sewing Kit", "Maybe you can repair your clothes.", "Diki� Kiti", "Belki k�yafetlerini onarabilirsin.");
        AddItem("shotgun", "Shotgun", "The most effective weapon at close range.", "Pompal� T�fek", "Yak�n mesafedeki en etkili silah.");
        AddItem("shotgun_shell", "Shotgun Shell", "Ammunition for a shotgun.", "Pompal� T�fek Mermisi", "Pompal� av t�fe�ine ait m�himmat.");
        AddItem("sleeping_bag", "Sleeping Bag", "Some sleep would be good.", "Uyku Tulumu", "Biraz uyku iyi gelir.");
        AddItem("soda", "Soda", "It's a delicious drink.", "Gazoz", "Leziz bir i�ecek.");
        AddItem("soil", "Soil", "Necessary to produce farming plots.", "Toprak", "Tarla ekim blo�u �retimi i�in gerekli.");
        AddItem("spear", "Spear", "Long range piercing tool.", "M�zrak", "Uzun menzilli delici alet.");
        AddItem("splint", "Splint", "Repairs your broken leg.", "Atel", "K�r�lan baca��n� onar�r.");
        AddItem("spice", "Spice", "You can add it to your meals.", "Baharat", "Yemeklerine katabilirsin.");
        AddItem("steak_with_vegetables", "Steak With Vegetables", "It is a first class food in terms of nutrition and taste.", "Sebzeli Biftek", "Besleyicilik ve lezzet a��s�ndan 1. s�n�f bir yiyecek.");
        AddItem("stick", "Stick", "It is produced by cutting wood. It is used in production.", "�ubuk", "Odun kesilerek �retilir. �retimde kullan�l�r.");
        AddItem("stone", "Stone", "Raw material obtained by breaking rocks.", "Ta�", "Kaya k�rarak elde edilen ham madde.");
        AddItem("stone_block", "Stone Block", "A block with medium durability.", "Ta� Blok", "Orta dayan�kl�l��a sahip bir blok.");
        AddItem("stone_stair", "Stone Stair", "Used to go higher.", "Ta� Basamak", "Daha y�kse�e ��kmak i�in kullan�l�r.");
        AddItem("sulfur", "Sulfur", "It is used in the production of powder.", "S�lf�r", "Barut �retiminde kullan�l�r.");
        AddItem("syringe", "Syringe", "It promotes health and contains some vitamins.", "��r�nga", "Sa�l��� y�kseltir, biraz da vitamin i�erir.");
        AddItem("sword", "Sword", "A sharp blade with high damage.", "K�l��", "Y�ksek hasarl� ve keskin bir b��ak.");
        AddItem("tape", "Tape", "It is used in the manufacture of certain items.", "Bant", "Baz� ��elerin �retiminde kullan�l�r.");
        AddItem("tomato", "Tomato", "Organic food that smells like musk.", "Domates", "Mis gibi kokan organik besin.");
        AddItem("tomato_seed", "Tomato Seed", "Seed to grow tomato.", "Domates Tohumu", "Domates yeti�tirmek i�in tohum.");
        AddItem("tomato_soup", "Tomato Soup", "Yummy and warm. It warms you up.", "Domates �orbas�", "Leziz ve s�cac�k. ��inizi �s�t�r.");
        AddItem("wood", "Wood", "Raw material obtained by cutting trees.", "Odun", "A�a� kesilerek elde edilen ham madde.");
        AddItem("wood_block", "Wood Block", "A block with low durability.", "Odun Blok", "D���k dayan�kl�l��a sahip bir blok.");
        AddItem("wood_door", "Wood Door", "It is used for entry and exit from the house.", "Tahta Kap�", "Eve giri� ��k�� i�in kullan�l�r.");
        AddItem("wood_frame", "Wood Frame", "It is the starting point of the buildings. It can be picked.", "Odun �er�eve", "Yap�lar�n ba�lang�� noktas�d�r. Geri al�nabilir.");
        AddItem("wood_spike", "Wood Spike", "Slows and kills enemies.", "Keskin Odun Tuzak", "D��manlar� yava�lat�r ve �ld�r�r.");
        AddItem("wood_stair", "Wood Stair", "Used to go higher.", "Odun Basamak", "Daha y�kse�e ��kmak i�in kullan�l�r.");
    }
    private void Add(string key, string en, string tr)
    {
        TranslationClass translationClass = new TranslationClass();
        translationClass.key = key;
        translationClass.en = en;
        translationClass.tr = tr;

        translations.Add(translationClass);
    }
    private void AddItem(string nickname, string nameEn, string descEn, string nameTr, string descTr)
    {
        TranslationItemClass translationItemClass = new TranslationItemClass();
        translationItemClass.nickname = nickname;
        translationItemClass.nameEn = nameEn;
        translationItemClass.descriptionEn = descEn;
        translationItemClass.nameTr = nameTr;
        translationItemClass.descriptionTr = descTr;

        translationItems.Add(translationItemClass);
    }
    public string Get(string key)
    {
        var result = translations.Find(x => x.key == key);
        if (result == null) return "";

        if (LanguageSettings.language == "en") return result.en;
        else if (LanguageSettings.language == "tr") return result.tr;

        return "";
    }
    public string GetItemName(string nickname)
    {
        var result = translationItems.Find(x => x.nickname == nickname);
        if (result == null) return "";

        if (LanguageSettings.language == "en") return result.nameEn;
        else if (LanguageSettings.language == "tr") return result.nameTr;

        return "";
    }
    public string GetItemDescription(string nickname)
    {
        var result = translationItems.Find(x => x.nickname == nickname);
        if (result == null) return "";

        if (LanguageSettings.language == "en") return result.descriptionEn;
        else if (LanguageSettings.language == "tr") return result.descriptionTr;

        return "";
    }
}

public class TranslationClass
{
    public string key;
    public string en;
    public string tr;
}
public class TranslationItemClass
{
    public string nickname;

    public string nameEn;
    public string descriptionEn;

    public string nameTr;
    public string descriptionTr;
}
