using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRange : MonoBehaviour
{
    private AudioManager audioManager;
    private Crosshair crosshair;
    private Inventory inventory;
    private LockMovement lockMovement;
    private ParticleManager particleManager;
    private PlayerItems playerItems;
    private PlayerStats playerStats;
    private Raycast raycast;

    private Axe axe;
    private BaseballBat baseballBat;
    private Pickaxe pickaxe;
    private Spear spear;
    private Sword sword;

    [SerializeField] private AudioClip hitTreeAudio;
    [SerializeField] private AudioClip hitRockAudio;

    private Animation anim;
    private AnimationClip attackAnimation;

    private float range;
    private float speed;
    private int damage;
    private float damageLatency;
    private float energyCost;

    private Item weaponItem;

    void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        crosshair = GetComponent<Crosshair>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        particleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParticleManager>();
        playerItems = GetComponent<PlayerItems>();
        playerStats = GetComponent<PlayerStats>();
        raycast = GetComponent<Raycast>();

        axe = GetComponent<Axe>();
        baseballBat = GetComponent<BaseballBat>();
        pickaxe = GetComponent<Pickaxe>();
        spear = GetComponent<Spear>();
        sword = GetComponent<Sword>();
    }
    void Update()
    {
        CheckSelectedItem();
        if (weaponItem == null) return;
        CheckInputs();
    }
    private void CheckSelectedItem()
    {
        if (playerItems.selectedItem == null || playerItems.selectedItem.category != Categories.CloseRange)
        {
            if (weaponItem == null) return;

            ResetSelectedItemVariables();
            return;
        }
        else
        {
            GetSelectedItemVariables();
        }
    }
    private void GetSelectedItemVariables()
    {
        if (weaponItem == playerItems.selectedItem) return;

        weaponItem = playerItems.selectedItem;

        if (weaponItem.nickName == "axe") axe.GetVariables();
        else if (weaponItem.nickName == "baseball_bat") baseballBat.GetVariables();
        else if (weaponItem.nickName == "pickaxe") pickaxe.GetVariables();
        else if (weaponItem.nickName == "spear") spear.GetVariables();
        else if (weaponItem.nickName == "sword") sword.GetVariables();
    }
    public void SetSelectedItemVariables(Animation anim, AnimationClip attackAnimation, float range,
        float speed, int damage, float damageLatency, float energyCost)
    {
        this.anim = anim;
        this.attackAnimation = attackAnimation;
        this.range = range;
        this.speed = speed;
        this.damage = damage;
        this.damageLatency = damageLatency;
        this.energyCost = energyCost;
    }
    private void ResetSelectedItemVariables()
    {
        anim = null;
        attackAnimation = null;
        range = 0;
        speed = 0;
        damage = 0;
        damageLatency = 0;
        energyCost = 0;
        weaponItem = null;
    }
    private void CheckInputs()
    {
        if (!Input.GetMouseButton(0)) return;

        StartAttack();
    }
    private void StartAttack()
    {
        if (lockMovement.locked || lockMovement.playerBusy || playerStats.energy < energyCost) return;

        playerStats.DecreaseEnergy(energyCost);
        anim.Play(attackAnimation.name);
        Invoke(nameof(Raycast), damageLatency);

        lockMovement.KeepBusy(speed);
        
    }
    private void Raycast()
    {
        crosshair.PlayAnimation();

        RaycastHit? hit = raycast.CreateRaycast(range);
        if (hit == null) return;

        
        weaponItem.DecreaseDurability(1);
        inventory.UpdateSelectedSlot();

        switch (hit.Value.transform.root.tag)
        {
            case "Zombie": GiveDamageToZombie(hit.Value.transform.root.gameObject); particleManager.PlayHitZombieParticle(hit.Value.point); break;
            case "Animal": GiveDamageToAnimal(hit.Value.transform.root.gameObject); particleManager.PlayHitZombieParticle(hit.Value.point); break;
            case "Building": DecreaseBuildingDurability(hit.Value.transform.root.gameObject); break;
            case "Tree": GiveDamageToTree(hit.Value.transform.root.gameObject, hit.Value.point); particleManager.PlayHitTreeParticle(hit.Value.point); break;
            case "Rock": GiveDamageToRock(hit.Value.transform.root.gameObject); particleManager.PlayHitRockParticle(hit.Value.point); break;
        }
    }
    private void GiveDamageToZombie(GameObject zombieObject)
    {
        zombieObject.GetComponent<Zombie>().DecreaseHealth(gameObject, damage);
    }
    private void GiveDamageToAnimal(GameObject animalObject)
    {
        animalObject.GetComponent<Animal>().DecreaseHealth(gameObject, damage);
    }
    public void DecreaseBuildingDurability(GameObject building)
    {
        Block block = building.GetComponent<Block>();
        if (block.blockType == BlockTypes.Mine)
            building.GetComponent<Mine>().ExplodeWithShoot();
        else
            block.DecreaseDurability(damage);
    }
    private void GiveDamageToTree(GameObject tree, Vector3 hitPoint)
    {
        audioManager.Play(hitTreeAudio);
        tree.GetComponent<TreeCode>().DecreaseHealth(damage, transform.position, hitPoint);

        if (weaponItem.nickName == "axe")
            inventory.AddItem("wood", 15, true);

        else
            inventory.AddItem("wood", 5, true);
    }
    private void GiveDamageToRock(GameObject rockObject)
    {
        Rock rock = rockObject.GetComponent<Rock>();

        audioManager.Play(hitRockAudio);
        rock.DecreaseHealth(damage);

        int amount = 12;
        if (weaponItem.nickName != "pickaxe") amount = 4;

        if (rock.rockType == RockType.Stone) inventory.AddItem("stone", amount, true); // 40 hasar 25 vuruþ 300 item
        else if (rock.rockType == RockType.Coal) inventory.AddItem("coal", amount, true);
        else if (rock.rockType == RockType.Sulfur) inventory.AddItem("sulfur", amount, true);
        else if (rock.rockType == RockType.Iron) inventory.AddItem("scrap_metal", amount, true);
    }

}
