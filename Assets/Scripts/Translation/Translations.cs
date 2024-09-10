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
        Add("HealthValue", "Health Value : ", "Saðlýk Deðeri : ");
        Add("FoodValue", "Food Value : ", "Yiyecek Deðeri : ");
        Add("WaterValue", "Water Value : ", "Su Deðeri : ");

        // Car Canvas
        Add("CarSpeed", "Speed : ", "Hýz : ");
        Add("CarFuel", "Fuel : ", "Yakýt : ");
        Add("CarDurability", "Durability : ", "Dayanýklýlýk : ");

        // Car Controller
        Add("RadioTurnedOff", "The radio was turned off.", "Radyo kapatýldý.");

        // Console
        Add("WrongCommandUsage", "Wrong command usage !", "Yanlýþ komut kullanýmý !");
        Add("UnknownCommand", "Unknown command !", "Bilinmeyen komut !");
        Add("DayValueUpdated", "Day value updated !", "Gün deðeri güncellendi !");
        Add("MinuteValueUpdated", "Minute value updated !", "Dakika deðeri güncellendi !");
        Add("HourValueUpdated", "Hour value updated !", "Saat deðeri güncellendi !");
        Add("NewRunningSpeed", "New running speed : ", "Yeni koþu hýzý : ");
        Add("FpsCounterTurnedOff", "Fps counter turned off!", "Fps göstergesi kapatýldý!");
        Add("FpsCounterTurnedOn", "Fps counter turned on!", "Fps göstergesi açýldý!");
        Add("ItemAdded", "Item added!", "Eþya eklendi!");
        Add("EnergyValueChanged", "Energy value changed!", "Enerji deðeri deðiþtirildi!");
        Add("FoodValueChanged", "Food value changed!", "Yiyecek deðeri deðiþtirildi!");
        Add("WaterValueChanged", "Water value changed!", "Su deðeri deðiþtirildi!");
        Add("TimeSpeedChanged", "Time speed changed!", "Zaman hýzý deðiþtirildi!");
        Add("SuicideCommand", "Player died!", "Oyuncu öldü!");
        Add("RespawnCommand", "Player respawned!", "Oyuncu yeniden doðdu!");
        Add("MenuCommand", "Returning to the main menu..", "Ana menüye dönülüyor..");
        Add("AirdropComing", "Airdrop is on the way!", "Hava desteði yolda!");
        Add("AirdropFailed", "Wait for the previous airdrop to complete!", "Önceki hava desteðinin tamamlanmasýný bekleyin!");
        Add("SensitivityChanged", "Mouse sensitivity has been changed.", "Fare hassasiyeti deðiþtirildi.");
        Add("QuitCommand", "Exiting the game..", "Oyundan çýkýþ yapýlýyor..");

        // Player
        Add("GodModeActivated", "God mode activated !", "Tanrý modu açýldý !");
        Add("GodModeDeactivated", "God mode deactivated !", "Tanrý modu kapatýldý !");

        // Raycast
        Add("CollectMushroomDialog", "Press 'E' to collect mushroom.", "Mantarý toplamak için 'E' tuþuna basýn.");
        Add("CollectWoodPileDialog", "Press 'E' to collect wood.", "Odunu yerden almak için 'E' tuþuna basýn.");
        Add("CollectStoneDialog", "Press 'E' to collect stone.", "Taþý yerden almak için 'E' tuþuna basýn.");
        Add("OpenCampfireDialog", "Press 'E' to open campfire panel.", "Kamp ateþi panelini açmak için 'E' tuþuna basýn.");
        Add("OpenFurnaceDialog", "Press 'E' to open furnace panel.", "Fýrýn panelini açmak için 'E' tuþuna basýn.");
        Add("OpenBackpackDialog", "Press 'E' to open backpack.", "Çantayý açmak için 'E' tuþuna basýn.");
        Add("OpenChestDialog", "Press 'E' to open chest.", "Sandýðý açmak için 'E' tuþuna basýn.");
        Add("OpenMaterialBoxDialog", "Press 'E' to open material box.", "Malzeme kutusunu açmak için 'E' tuþuna basýn.");
        Add("Durability", "Durability : ", "Dayanýklýlýk : ");
        Add("PlantDialog", "Press 'E' to plant.", "Tarlayý ekmek için 'E' tuþuna basýn.");
        Add("PlantNeedWater", "Plant (Need Water)", "Ekin (Sulama Gerekli)");
        Add("PlantWaterDialog", "Press 'E' to water the plant", "Bitkiyi sulamak için 'E' tuþuna basýn.");
        Add("PlantGrowingDialog", "Plant growing.", "Ekin büyüyor.");
        Add("HarvestDialog", "Press 'E' to harvest", "Bitkiyi hasat etmek için 'E' tuþuna basýn.");
        Add("Refill", "Refill", "Doldur");
        Add("FuelAndCost", "% fuel.\n Cost : ", "% yakýt. \n Tüketir : ");
        Add("Gasoline", "Gasoline", "Benzin");
        Add("GasTankFull", "The gas tank is full!", "Yakýt tanký tamamen dolu!");
        Add("GetWoodFrame", "Press 'E' to get wood frame.", "Odun çerçeveyi almak için 'E' tuþuna basýn.");
        Add("FillWater", "Press 'E' to fill water.", "Su doldurmak için 'E' tuþuna basýn.");
        Add("Car", "Press 'E' to drive car. \n Press 'F' to open trunk.", "Arabayý sürmek için 'E' tuþuna basýn. \n Bagajý açmak için 'F' tuþuna basýn.");
        Add("OpenLootDialog", "Press 'E' to open.", "Açmak için 'E' tuþuna basýn.");

        // SavedSlots
        Add("SavedSlotSave", "Save", "Kayýt");
        Add("SavedSlotFull", "Full", "Dolu");
        Add("SavedSlotEmpty", "Empty", "Boþ");

        // TimeCanvas
        Add("Day", "Day : ", "Gün : ");

        // ConstTexts (Main Menu)
        Add("STARTGAME", "START GAME", "OYUNA BAÞLA");
        Add("SETTINGS", "SETTINGS", "AYARLAR");
        Add("KEYBOARD", "KEYBOARD", "KLAVYE");
        Add("NEWS", "NEWS", "HABERLER");
        Add("QUIT", "QUIT", "ÇIKIÞ");
        Add("NEW_GAME_HEADER", "- NEW GAME -", "- YENÝ OYUN -");
        Add("DeleteSelected", "Delete selected", "Seçili kaydý sil");
        Add("LoadSelected", "Load selected", "Seçili kaydý baþlat");
        Add("NewGame", "New game", "Yeni oyun");
        Add("Difficulty", "Difficulty :", "Zorluk :");
        Add("LootAbundance", "Loot Abundance :", "Ganimet Bolluðu :");
        Add("BlockDamage", "Block Damage :", "Blok Hasarý :");
        Add("StartGame", "Start Game", "Oyunu Baþlat");
        Add("SETTINGS_HEADER", "- SETTINGS -", "- AYARLAR -");
        Add("ScreenResolution", "Screen Resolution :", "Ekran Çözünürlüðü :");
        Add("ScreenMode", "Screen Mode :", "Ekran Modu :");
        Add("GraphicsQuality", "Graphics Quality :", "Grafik Kalitesi :");
        Add("FixedFrameRate", "Fixed Frame Rate :", "Sabit Kare Hýzý :");
        Add("Language", "Language :", "Dil :");
        Add("Sensitivity", "Sensitivity :", "Fare Hassasiyeti :");
        Add("FpsCounter", "Fps Counter :", "Fps Göstergesi :");
        Add("SaveChanges", "Save Changes", "Deðiþiklikleri Kaydet");
        Add("On", "On", "Açýk");
        Add("Off", "Off", "Kapalý");
        Add("KEYBOARD_HEADER", "- KEYBOARD -", "- KLAVYE -");
        Add("MoveForward", "Move forward", "Ýleriye hareket et");
        Add("MoveLeft", "Move left", "Sola hareket et");
        Add("MoveBack", "Move back", "Geriye hareket et");
        Add("MoveRight", "Move right", "Saða hareket et");
        Add("KeyR", "Reload / Change Radio Channel", "Þarjör deðiþtir / Radyo kanalýný deðiþtir");
        Add("Space", "Space", "Boþluk");
        Add("Jump", "Jump", "Zýpla");
        Add("Crouch", "Crouch", "Eðil");
        Add("MouseScroll", "Mouse Scroll", "Fare Tekerleði");
        Add("MouseScrollDesc", "Change the selected slot", "Seçili slotu deðiþtir");
        Add("InteractObjects", "Interact with objects", "Nesnelerle etkileþime geç");
        Add("ToggleCar", "Toggle car chest", "Araç sandýðýný aç/kapat");
        Add("ToggleMap", "Toggle map", "Haritayý aç/kapat");
        Add("ToggleInventory", "Toggle inventory", "Envanteri aç/kapat");
        Add("ToggleConsole", "Toggle console", "Konsolu aç/kapat");
        Add("LeftClick", "Left Click", "Sol Týk");
        Add("LeftClickDesc", "Use the primary function of the selected item", "Seçili öðenin birincil fonksiyonunu kullan");
        Add("RightClick", "Right Click", "Sað Týk");
        Add("RightClickDesc", "Fast shoot / Zoom / Use selected item", "Hýzlý atýþ / Yakýnlaþtýr / Seçili eþyayý kullan");
        Add("LOADING", "LOADING..", "YÜKLENÝYOR..");

        // ConstTexts (Game)
        Add("BACKPACK_HEADER", "BACKPACK", "SIRT ÇANTASI");
        Add("CHEST_HEADER", "CHEST", "SANDIK");
        Add("CRAFTING_PANEL_HEADER", "CRAFTING PANEL", "ÜRETÝM PANELÝ");
        Add("FURNACE_PANEL_FUEL_HEADER", "FUEL", "YAKIT");
        Add("FURNACE_PANEL_SMELTED_HEADER", "SMELTED", "ERÝTÝLDÝ");
        Add("FURNACE_PANEL_SMELT_HEADER", "SMELT", "ERÝTÝLECEK");
        Add("START_STOP_FIRE", "START/STOP FIRE", "ATEÞÝ BAÞLAT/DURDUR");
        Add("DESCRIPTION", "DESCRIPTION", "AÇIKLAMA");
        Add("CRAFT", "CRAFT", "ÜRET");
        Add("REQUIRED_MATERIALS", "REQUIRED MATERIALS", "GEREKLÝ MALZEMELER");
        Add("CRAFTING_PANEL_HEADER", "CRAFTING PANEL", "ÜRETÝM PANELÝ");
        Add("INVENTORY_HEADER", "INVENTORY", "ENVANTER");
        Add("LOOT_HEADER", "LOOT", "GANÝMET");
        Add("Resume", "Resume", "Oyuna Geri Dön");
        Add("Respawn", "Respawn", "Yeniden Doð");
        Add("RespawnRegisteredPoint", "Respawn Registered Point", "Kayýtlý Noktada Yeniden Doð");
        Add("MainMenu", "Main Menu", "Menüye Dön");
        Add("YOU_DIED", "YOU DIED.", "ÖLDÜN.");
    }
    private void PlaceItem()
    {
        AddItem("7.62mm_ammo", "7.62mm Ammo", "Assault and sniper rifle ammunition.", "7.62mm Mermi", "Taarruz ve niþancý tüfeðine ait mühimmat.");
        AddItem("9mm_ammo", "9mm Ammo", "Ammunition for pistol.", "9mm Mermi", "Tabanca için mühimmat.");
        AddItem("animal_fat", "Animal Fat", "Maybe you can produce some gasoline.", "Hayvan Yaðý", "Belki biraz benzin üretebilirsin.");
        AddItem("arrow", "Arrow", "Ammunition for bows.", "Ok", "Germeli silahlar için mühimmat.");
        AddItem("assault_rifle", "Assault Rifle", "A powerful and automatic weapon.", "Taarruz Tüfeði", "Güçlü ve otomatik bir silah.");
        AddItem("axe", "Axe", "Ideal hand tool for cutting trees.", "Balta", "Aðaç kesmek için ideal bir el aleti.");
        AddItem("bandage", "Bandage", "Stops bleeding, slightly increases health.", "Bandaj", "Kanamalarý durdurur, saðlýðý biraz artýrýr.");
        AddItem("baseball_bat", "Baseball Bat", "Medium damaged stick.", "Beyzbol Sopasý", "Orta hasarlý sopa.");
        AddItem("boiled_tomato_salad", "Boiled Tomato Salad", "Even though it doesn't taste good, it is nutritious.", "Haþlanmýþ Domates Salatasý", "Tadý güzel olmasa da besleyicidir.");
        AddItem("bottled_water", "Water Bottle", "Water is an essential source of survival.", "Þiþe Su", "Su temel bir hayatta kalma kaynaðýdýr.");
        AddItem("bow", "Bow", "A primitive but effective bow.", "Yay", "Ýlkel ama etkili bir yay.");
        AddItem("broken_bottle", "Broken Bottle", "A shattered bottle.", "Kýrýk Þiþe", "Parçalanmýþ halde bir þiþe.");
        AddItem("bullet_mold", "Bullet Mold", "It is the mold required for bullet production.", "Mermi Kalýbý", "Mermi üretimi için gerekli olan kalýptýr.");
        AddItem("bullet_tip", "Bullet Tip", "It is the tip required for bullet production.", "Mermi Ucu", "Mermi üretimi için gerekli olan uçtur.");
        AddItem("campfire", "Campfire", "Ideal for cooking and finding some peace.", "Kamp Ateþi", "Yemek piþirmek ve biraz huzur bulmak için ideal.");
        AddItem("canned_bean", "Canned Bean", "A nutritious preserve.", "Konserve Fasulye", "Besleyici bir konserve.");
        AddItem("canned_fish", "Canned Fish", "A nutritious preserve.", "Konserve Balýk", "Besleyici bir konserve.");
        AddItem("canned_pea", "Canned Pea", "A nutritious preserve.", "Konserve Bezelye", "Besleyici bir konserve.");
        AddItem("chemical", "Chemical", "Used in production.", "Kimyasal", "Üretimde kullanýlýr.");
        AddItem("chest", "Chest", "An area where you can store items.", "Sandýk", "Ýçerisine eþya depolayabileceðiniz bir alan.");
        AddItem("cloth", "Cloth", "A soft cloth.", "Kumaþ", "Yumuþacýk bir kumaþ.");
        AddItem("coal", "Coal", "Material used to make gunpowder.", "Kömür", "Barut yapmak için kullanýlan malzeme.");
        AddItem("coffee", "Coffee", "A delicious beverage made from coffee beans.", "Kahve", "Kahve çekirdeklerinden yapýlmýþ, enfes bir içecek.");
        AddItem("coffee_bean", "Coffee Bean", "Use some water to make coffee.", "Kahve Çekirdeði", "Kahve yapmak için biraz su kullanýn.");
        AddItem("coin", "Coin", "It is now only used for smelting in the furnace.", "Madeni Para", "Artýk sadece fýrýnda eritmek için kullanýlýyor.");
        AddItem("cola", "Cola", "It's a delicious drink.", "Kola", "Leziz bir içecek.");
        AddItem("corn", "Corn", "You can make a variety of dishes using some corn.", "Mýsýr", "Biraz mýsýr kullanarak çeþitli yemekler yapabilirsin.");
        AddItem("corn_bread", "Corn Bread", "Delicious bread made using some corn.", "Mýsýr Ekmeði", "Biraz mýsýr kullanýlarak yapýlan leziz ekmek.");
        AddItem("corn_chips", "Corn Chips", "Another delicious variety of chips.", "Mýsýr Cipsi", "Bir baþka leziz cips çeþidi.");
        AddItem("corn_seed", "Corn Seed", "It is used to grow corn.", "Mýsýr Tohumu", "Mýsýr yetiþtirmek için kullanýlýr.");
        AddItem("cotton", "Cotton", "It is used in cloth production.", "Pamuk", "Kumaþ üretiminde kullanýlýr.");
        AddItem("cotton_seed", "Cotton Seed", "It is used to grow cotton.", "Pamuk Tohumu", "Pamuk yetiþtirmek için kullanýlýr.");
        AddItem("cucumber", "Cucumber", "It is used in foods.", "Salatalýk", "Yemeklerde kullanýlýr.");
        AddItem("cucumber_extract", "Cucumber Extract", "It is used in making cream.", "Salatalýk Özü", "Krem yapýmýnda kullanýlýr.");
        AddItem("cucumber_seed", "Cucumber Seed", "It is used to grow cucumber.", "Salatalýk Tohumu", "Salatalýk yetiþtirmek için kullanýlýr.");
        AddItem("cream", "Cream", "It is used in first aid kit.", "Krem", "Ýlk yardým kitinde kullanýlýr.");
        AddItem("crossbow", "Crossbow", "It is slower than the regular bow, but it shoots more effectively.", "Kundaklý Yay", "Normal Yay'a göre daha yavaþtýr ama daha etkili atýþlar yapar.");
        AddItem("dirty_water", "Dirty Water", "Unreliable water. It must be boiled to drink.", "Kirli Su", "Güvenilmez bir su. Ýçmek için kaynatýlmalýdýr.");
        AddItem("dynamite", "Dynamite", "You can blow up doors and walls with this explosive.", "Dinamit", "Bu patlayýcý ile kapý ve duvarlarý patlatabilirsin.");
        AddItem("egg", "Egg", "It is nutritious but should be used in meals.", "Yumurta", "Besleyici ama yemeklerde kullanýlmalý.");
        AddItem("empty_bottle", "Empty Bottle", "An empty bottle. Maybe you can fill it with water.", "Boþ Þiþe", "Boþ bir þiþe. Belki su doldurabilirsin.");
        AddItem("farming_plot", "Farming Plot", "It is used to grow seeds.", "Tarým Arsasý", "Tohumlarý yetiþtirmek için kullanýlýr.");
        AddItem("feather", "Feather", "A common material used in arrow making.", "Kuþ Tüyü", "Ok yapýmýnda kullanýlan yaygýn bir malzeme.");
        AddItem("first_aid_kit", "First Aid Kit", "Stops bleeding, increases health at a high rate.", "Ýlk Yardým Kiti", "Kanamalarý durdurur, saðlýðý yüksek oranda artýrýr.");
        AddItem("flour", "Flour", "It is used in dough dishes.", "Un", "Hamurlu yemeklerde kullanýlýr.");
        AddItem("forged_iron", "Forged Iron", "It is used in production.", "Dövme Demir", "Üretimde kullanýlýr.");
        AddItem("forged_sulfur", "Forged Sulfur", "It is used in gunpowder production.", "Dövme Sülfür", "Barut üretiminde kullanýlýr.");
        AddItem("fried_potatoes", "Fried Potatoes", "Children's favorite food.", "Patates Kýzartmasý", "Çocuklarýn favori yiyeceði.");
        AddItem("fruit_juice", "Fruit Juice", "A delicious and vitamin-containing beverage.", "Meyve Suyu", "Leziz ve vitamin içeren bir içecek.");
        AddItem("furnace", "Furnace", "It is used to smelt raw materials.", "Fýrýn", "Ham malzemeleri eritmek için kullanýlýr.");
        AddItem("gasoline", "Gasoline", "Fuel source for car.", "Benzin", "Araba için yakýt kaynaðý.");
        AddItem("glass", "Glass", "It is indispensable for a home.", "Cam", "Bir evin olmazsa olmazýdýr.");
        AddItem("glue", "Glue", "It is used in the manufacture of certain items.", "Yapýþtýrýcý", "Bazý öðelerin üretiminde kullanýlýr.");
        AddItem("grilled_meat", "Grilled Meat", "It looks pretty nutritious.", "Izgara Et", "Oldukça besleyici görünüyor.");
        AddItem("hammer", "Hammer", "Security lock that can be added to the doors.", "Çekiç", "Yapýlarý taþ, metal gibi daha geliþmiþ maddelerle kaplar.");
        AddItem("hunting_rifle", "Hunting Rifle", "The indispensable power of long distances.", "Av Tüfeði", "Uzak mesafelerin vazgeçilmez gücü.");
        AddItem("iron_block", "Iron Block", "It was reinforced with iron.", "Demir Blok", "Demirin gücü ile taçlandýrýldý.");
        AddItem("iron_door", "Iron Door", "It is used for entry and exit from the house. It is made of iron which is very durable.", "Demir Kapý", "Eve giriþ çýkýþ için kullanýlýr. Çok dayanýklý olan demirden üretilmiþtir.");
        AddItem("iron_stair", "Iron Stair", "Used to go higher.", "Demir Basamak", "Daha yükseðe çýkmak için kullanýlýr.");
        AddItem("landmine", "Land Mine", "Military ammunition used to destroy creatures that step on it.", "Kara Mayýný", "Üzerine basan canlýlarý yok etmek için kullanýlan askeri mühimmat.");
        AddItem("ladder", "Ladder", "The best way to climb.", "Merdiven", "Týrmanmanýn en iyi yolu.");
        AddItem("leather", "Leather", "Maybe you can craft an armor using this.", "Deri", "Belki bunu kullanarak bir zýrh üretebilirsin.");
        AddItem("meat_skewer", "Meat Skewer", "Where is the barbecue?", "Et Þiþ", "Mangal nerede?");
        AddItem("meat_stew", "Meat Stew", "A healthy food to fill up on.", "Etli Güveç", "Týka basa doymak için saðlýklý bir besin.");
        AddItem("milk", "Milk", "Moo", "Süt", "Möö");
        AddItem("mushroom", "Mushroom", "You can eat it right away or use it to make pizza.", "Mantar", "Hemen yiyebilir veya pizza yapmak için kullanabilirsiniz.");
        AddItem("mushroom_onion_rings", "Mushroom Onion Rings", "Delicious onion rings.", "Mantarlý Soðan Halkasý", "Leziz soðan halkalarý.");
        AddItem("mushroom_seed", "Mushroom Seed", "It is used to grow mushroom.", "Mantar Tohumu", "Mantar yetiþtirmek için kullanýlýr.");
        AddItem("oil", "Oil", "It is used in production.", "Yað", "Üretimde kullanýlýr.");
        AddItem("onion", "Onion", "Spicy, but it is very healthy.", "Soðan", "Acý ama çok saðlýklý.");
        AddItem("onion_seed", "Onion Seed", "It is used to grow onion.", "Soðan Tohumu", "Soðan yetiþtirmek için kullanýlýr.");
        AddItem("painkiller", "Painkiller", "A powerful pain reliever.", "Aðrý Kesici", "Güçlü bir aðrý kesici.");
        AddItem("painted_block", "Painted Block", "It was decorated with paint.", "Boyama Blok", "Boyayla dekore edilmiþtir.");
        AddItem("pan", "Pan", "Maybe you can cook.", "Tava", "Belki yemek piþirebilirsin.");
        AddItem("pancake", "Pancake", "Some sugar.", "Gözleme", "Biraz da þeker.");
        AddItem("paper", "Paper", "You definitely won't write anything.", "Kaðýt", "Bir þeyler yazmayacaðýn kesin.");
        AddItem("pasta_with_sauce", "Pasta With Sauce", "A powerful source of carbohydrates.", "Soslu Makarna", "Güçlü bir karbonhidrat kaynaðý.");
        AddItem("pepper", "Pepper", "It adds spice to your meals.", "Biber", "Yemeklerine acý katar.");
        AddItem("pepper_seed", "Pepper Seed", "It is used to grow pepper.", "Biber Tohumu", "Biber yetiþtirmek için kullanýlýr.");
        AddItem("pickaxe", "Pickaxe", "Expensive hand tool used to break stones.", "Kazma", "Taþlarý kýrmak için kullanýlan, pahalý bir el aracý.");
        AddItem("pistol", "Pistol", "Perfect for scaring your enemies.", "Tabanca", "Düþmanlarýný korkutmak için birebir.");
        AddItem("pizza", "Pizza", "A unique combination of mushrooms and corn.", "Pizza", "Mantar ve mýsýrýn eþsiz uyumu.");
        AddItem("plastic", "Plastic", "It is used in production.", "Plastik", "Üretimde kullanýlýr.");
        AddItem("potato", "Potato", "Here is an all-around great food.", "Patates", "Ýþte her yönüyle muazzam bir yiyecek.");
        AddItem("potato_balls", "Potato Balls", "Potatoes are delicious in every form.", "Patates Toplarý", "Patatesin her hali leziz.");
        AddItem("potato_chips", "Potato Chips", "A delicious and salty chip.", "Patates Cipsi", "Bol tuzlu ve leziz bir cips.");
        AddItem("potato_seed", "Potato Seed", "It is used to grow potato.", "Patates Tohumu", "Patates yetiþtirmek için kullanýlýr.");
        AddItem("powder", "Powder", "It is used in bullet production.", "Barut", "Mermi üretiminde kullanýlýr.");
        AddItem("raw_meat", "Raw Meat", "Do not eat without cooking!", "Çið Et", "Piþirmeden sakýn yemeyin!");
        AddItem("rope", "Rope", "It is used in production.", "Ýp", "Üretimde kullanýlýr.");
        AddItem("rosehip", "Rosehip", "It is used to make rosehip tea.", "Kuþburnu", "Kuþburnu çayý yapýmýnda kullanýlýr.");
        AddItem("rosehip_seed", "Rosehip Seed", "It is used to grow rosehip.", "Kuþburnu Tohumu", "Kuþburnu yetiþtirmek için kullanýlýr.");
        AddItem("rosehip_tea", "Rosehip Tea", "A hot rosehip tea is not always available.", "Kuþburnu Çayý", "Sýcak bir kuþburnu çayý her zaman bulunmaz.");
        AddItem("salt", "Salt", "It can be added to some meals.", "Tuz", "Bazý yemeklere katýlabilir.");
        AddItem("scope", "Scope", "Required to craft a shotgun.", "Dürbün", "Bir av tüfeði üretmek için gerekli.");
        AddItem("scrap_metal", "Scrap Metal", "Freshly mined pieces of pure metal.", "Hurda Metal", "Yeni kazýlmýþ saf metal parçalarý.");
        AddItem("screw", "Screw", "It is used in production.", "Vida", "Üretimde kullanýlýr.");
        AddItem("sewing_kit", "Sewing Kit", "Maybe you can repair your clothes.", "Dikiþ Kiti", "Belki kýyafetlerini onarabilirsin.");
        AddItem("shotgun", "Shotgun", "The most effective weapon at close range.", "Pompalý Tüfek", "Yakýn mesafedeki en etkili silah.");
        AddItem("shotgun_shell", "Shotgun Shell", "Ammunition for a shotgun.", "Pompalý Tüfek Mermisi", "Pompalý av tüfeðine ait mühimmat.");
        AddItem("sleeping_bag", "Sleeping Bag", "Some sleep would be good.", "Uyku Tulumu", "Biraz uyku iyi gelir.");
        AddItem("soda", "Soda", "It's a delicious drink.", "Gazoz", "Leziz bir içecek.");
        AddItem("soil", "Soil", "Necessary to produce farming plots.", "Toprak", "Tarla ekim bloðu üretimi için gerekli.");
        AddItem("spear", "Spear", "Long range piercing tool.", "Mýzrak", "Uzun menzilli delici alet.");
        AddItem("splint", "Splint", "Repairs your broken leg.", "Atel", "Kýrýlan bacaðýný onarýr.");
        AddItem("spice", "Spice", "You can add it to your meals.", "Baharat", "Yemeklerine katabilirsin.");
        AddItem("steak_with_vegetables", "Steak With Vegetables", "It is a first class food in terms of nutrition and taste.", "Sebzeli Biftek", "Besleyicilik ve lezzet açýsýndan 1. sýnýf bir yiyecek.");
        AddItem("stick", "Stick", "It is produced by cutting wood. It is used in production.", "Çubuk", "Odun kesilerek üretilir. Üretimde kullanýlýr.");
        AddItem("stone", "Stone", "Raw material obtained by breaking rocks.", "Taþ", "Kaya kýrarak elde edilen ham madde.");
        AddItem("stone_block", "Stone Block", "A block with medium durability.", "Taþ Blok", "Orta dayanýklýlýða sahip bir blok.");
        AddItem("stone_stair", "Stone Stair", "Used to go higher.", "Taþ Basamak", "Daha yükseðe çýkmak için kullanýlýr.");
        AddItem("sulfur", "Sulfur", "It is used in the production of powder.", "Sülfür", "Barut üretiminde kullanýlýr.");
        AddItem("syringe", "Syringe", "It promotes health and contains some vitamins.", "Þýrýnga", "Saðlýðý yükseltir, biraz da vitamin içerir.");
        AddItem("sword", "Sword", "A sharp blade with high damage.", "Kýlýç", "Yüksek hasarlý ve keskin bir býçak.");
        AddItem("tape", "Tape", "It is used in the manufacture of certain items.", "Bant", "Bazý öðelerin üretiminde kullanýlýr.");
        AddItem("tomato", "Tomato", "Organic food that smells like musk.", "Domates", "Mis gibi kokan organik besin.");
        AddItem("tomato_seed", "Tomato Seed", "Seed to grow tomato.", "Domates Tohumu", "Domates yetiþtirmek için tohum.");
        AddItem("tomato_soup", "Tomato Soup", "Yummy and warm. It warms you up.", "Domates Çorbasý", "Leziz ve sýcacýk. Ýçinizi ýsýtýr.");
        AddItem("wood", "Wood", "Raw material obtained by cutting trees.", "Odun", "Aðaç kesilerek elde edilen ham madde.");
        AddItem("wood_block", "Wood Block", "A block with low durability.", "Odun Blok", "Düþük dayanýklýlýða sahip bir blok.");
        AddItem("wood_door", "Wood Door", "It is used for entry and exit from the house.", "Tahta Kapý", "Eve giriþ çýkýþ için kullanýlýr.");
        AddItem("wood_frame", "Wood Frame", "It is the starting point of the buildings. It can be picked.", "Odun Çerçeve", "Yapýlarýn baþlangýç noktasýdýr. Geri alýnabilir.");
        AddItem("wood_spike", "Wood Spike", "Slows and kills enemies.", "Keskin Odun Tuzak", "Düþmanlarý yavaþlatýr ve öldürür.");
        AddItem("wood_stair", "Wood Stair", "Used to go higher.", "Odun Basamak", "Daha yükseðe çýkmak için kullanýlýr.");
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
