using UnityEngine;

public class ConsumableItems : MonoBehaviour
{
    private HealthNotification healthNotification;
    private Inventory inventory;
    private LockMovement lockMovement;
    private PlayerItems playerItems;
    private PlayerStats playerStats;
    

    private AudioSource audioSource;
    [SerializeField] private AudioClip eatingSound;
    [SerializeField] private AudioClip drinkingSound;
    [SerializeField] private AudioClip bandageSound;
    [SerializeField] private AudioClip painkillerSound;
    [SerializeField] private AudioClip syringeSound;

    [SerializeField] private Animation foodAnim;
    [SerializeField] private AnimationClip eatingAnimation;

    [SerializeField] private Animation waterAnim;
    [SerializeField] private AnimationClip drinkingAnimation;

    [SerializeField] private Animation bandageAnim;
    [SerializeField] private AnimationClip bandageAnimation;

    [SerializeField] private Animation painkillerAnim;
    [SerializeField] private AnimationClip painkillerAnimation;

    [SerializeField] private Animation syringeAnim;
    [SerializeField] private AnimationClip syringeAnimation;

    [SerializeField] private Animation splintAnim;
    [SerializeField] private AnimationClip splintAnimation;

    private Item consumableItem;

    void Awake()
    {
        healthNotification = GetComponent<HealthNotification>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        playerItems = GetComponent<PlayerItems>();
        playerStats = GetComponent<PlayerStats>();    
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        CheckSelectedItem();
        if (consumableItem == null) return;

        CheckInputs();
    }
    private void CheckSelectedItem()
    {
        if (playerItems.selectedItem == null ||
            (playerItems.selectedItem.category != Categories.Food && playerItems.selectedItem.category != Categories.Health &&
            playerItems.selectedItem.category != Categories.Water))
        {
            consumableItem = null;
            return;
        }
        else
        {
            consumableItem = playerItems.selectedItem;
        }
    }
    private void CheckInputs()
    {
        if (!Input.GetMouseButton(1) || lockMovement.locked || lockMovement.playerBusy) return;

        if (consumableItem.category == Categories.Food) EatFood();
        if (consumableItem.category == Categories.Water) DrinkWater();
        else if (consumableItem.category == Categories.Health) UseHealth();

    }
    private void EatFood()
    {
        audioSource.PlayOneShot(eatingSound);
        foodAnim.Play(eatingAnimation.name);
        lockMovement.KeepBusy(2f);

        playerStats.IncreaseStats(consumableItem.foodValue, consumableItem.waterValue, consumableItem.healthValue);
        consumableItem.DecreaseAmount(1);
        inventory.UpdateSelectedSlot();
    }
    private void DrinkWater()
    {
        if (consumableItem.nickName == "bottled_water" || consumableItem.nickName == "dirty_water" || consumableItem.nickName == "rosehip_tea" ||
            consumableItem.nickName == "coffee")
            inventory.AddItem("empty_bottle", 1, true);

        audioSource.PlayOneShot(drinkingSound);
        waterAnim.Play(drinkingAnimation.name);
        lockMovement.KeepBusy(1.8f);

        playerStats.IncreaseStats(consumableItem.foodValue, consumableItem.waterValue, consumableItem.healthValue);
        consumableItem.DecreaseAmount(1);
        inventory.UpdateSelectedSlot();
    }
    private void UseHealth()
    {
        if(consumableItem.nickName == "bandage" || consumableItem.nickName == "first_aid_kit")
        {
            audioSource.PlayOneShot(bandageSound);
            bandageAnim.Play(bandageAnimation.name);
            lockMovement.KeepBusy(2.25f);
        }
        else if (consumableItem.nickName == "painkiller")
        {
            audioSource.PlayOneShot(painkillerSound);
            painkillerAnim.Play(painkillerAnimation.name);
            lockMovement.KeepBusy(1.75f);
        }
        else if (consumableItem.nickName == "syringe")
        {
            audioSource.PlayOneShot(syringeSound);
            syringeAnim.Play(syringeAnimation.name);
            lockMovement.KeepBusy(2.5f);
        }
        else if (consumableItem.nickName == "splint")
        {
            audioSource.PlayOneShot(bandageSound);
            splintAnim.Play(splintAnimation.name);
            lockMovement.KeepBusy(1.5f);
        }


        playerStats.IncreaseStats(consumableItem.foodValue, consumableItem.waterValue, consumableItem.healthValue);
        playerItems.selectedItem.DecreaseAmount(1);
        inventory.UpdateSelectedSlot();

        if (consumableItem.nickName == "splint") healthNotification.Delete(HealthEffects.Broken);
        else if (consumableItem.nickName == "bandage" || consumableItem.nickName == "first_aid_kit") 
            healthNotification.Delete(HealthEffects.Bleeding);
    }
}
