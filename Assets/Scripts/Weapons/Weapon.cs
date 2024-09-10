using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private AmmoText ammoText;
    private AmmoTrace ammoTrace;
    private AudioManager audioManager;
    private Crosshair crosshair;
    private Inventory inventory;
    private LockMovement lockMovement;
    private MuzzleFlash muzzleFlash;
    private ParticleManager particleManager;
    private PlayerItems playerItems;
    private Raycast raycast;
    private Recoil recoil;

    private AssaultRifle assaultRifle;
    private Bow bow;
    private Crossbow crossbow;
    private HuntingRifle huntingRifle;
    private Pistol pistol;
    private Shotgun shotgun;

    private AudioClip shootSound;
    private AudioClip reloadSound;

    private Animation anim;
    private AnimationClip shootAnimation;
    private AnimationClip reloadAnimation;

    private GameObject zoomCanvas;

    private Vector3 muzzleFlashPosition;
   
    private float recoilRate;
    private float reloadTime;
    private float range;
    private float fireRate;
    private int damage;

    private int particleCount;
    private bool playMuzzleEffect;
    private bool fastShootSupport;
    private bool zoomSupport;

    [SerializeField] AudioClip emptyClipSound;
    private bool emptyClipSoundPlayed;

    private Item weaponItem;

    void Awake()
    {
        ammoText = GetComponent<AmmoText>();
        ammoTrace = GetComponent<AmmoTrace>();
        audioManager = GetComponent<AudioManager>();
        crosshair = GetComponent<Crosshair>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        muzzleFlash = GetComponent<MuzzleFlash>();
        particleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParticleManager>();
        playerItems = GetComponent<PlayerItems>();
        raycast = GetComponent<Raycast>();
        recoil = GetComponent<Recoil>();

        assaultRifle = GetComponent<AssaultRifle>();
        bow = GetComponent<Bow>();
        crossbow = GetComponent<Crossbow>();
        huntingRifle = GetComponent<HuntingRifle>();
        pistol = GetComponent<Pistol>();
        shotgun = GetComponent<Shotgun>();
    }
    void Update()
    {
        CheckSelectedItem();
        if (weaponItem == null) return;

        CheckInputs();
        CheckAutoReload();
        CheckReloadInput();
    }
    private void CheckSelectedItem()
    {
        if (playerItems.selectedItem == null || playerItems.selectedItem.category != Categories.Weapon)
        {
            if (weaponItem == null) return;

            ResetSelectedItemVariables();
            return;
        }
        else
        {
            GetSelectedItemVariables();
            ammoText.UpdateText(weaponItem.ammoInside, weaponItem.ammoTypeNick);
        }
    }
    private void GetSelectedItemVariables()
    {
        if (weaponItem == playerItems.selectedItem) return;

        weaponItem = playerItems.selectedItem;

        if (weaponItem.nickName == "pistol") pistol.GetVariables();
        else if (weaponItem.nickName == "assault_rifle") assaultRifle.GetVariables();
        else if (weaponItem.nickName == "shotgun") shotgun.GetVariables();
        else if (weaponItem.nickName == "hunting_rifle") huntingRifle.GetVariables();
        else if (weaponItem.nickName == "crossbow") crossbow.GetVariables();
        else if (weaponItem.nickName == "bow") bow.GetVariables();
    }
    public void SetSelectedItemVariables(AudioClip shootSound, AudioClip reloadSound, Animation anim, AnimationClip shootAnimation,
        AnimationClip reloadAnimation, GameObject zoomCanvas, Vector3 muzzleFlashPosition, float recoilRate, float reloadTime,
        float range, float fireRate, int damage, int particleCount, bool playMuzzleEffect, bool fastShootSupport, bool zoomSupport)
    {
        this.shootSound = shootSound;
        this.reloadSound = reloadSound;
        this.anim = anim;
        this.shootAnimation = shootAnimation;
        this.reloadAnimation = reloadAnimation;
        this.zoomCanvas = zoomCanvas;
        this.muzzleFlashPosition = muzzleFlashPosition;
        this.recoilRate = recoilRate;
        this.reloadTime = reloadTime;
        this.range = range;
        this.fireRate = fireRate;
        this.damage = damage;
        this.particleCount = particleCount;
        this.playMuzzleEffect = playMuzzleEffect;
        this.fastShootSupport = fastShootSupport;
        this.zoomSupport = zoomSupport;
    }
    private void ResetSelectedItemVariables()
    {
        shootSound = null;
        reloadSound = null;
        anim = null;
        shootAnimation = null;
        reloadAnimation = null;
        muzzleFlashPosition = Vector3.zero;
        particleCount = 0;
        recoilRate = 0;
        reloadTime = 0;
        range = 0;
        fireRate = 0;
        damage = 0;
        fastShootSupport = false;
        weaponItem = null;
    }
    private void CheckInputs()
    {
        if (Input.GetMouseButton(0)) Shoot(false);
        if (Input.GetMouseButton(1))
        {
            if (fastShootSupport) Shoot(true);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomSupport) Zoom();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (emptyClipSoundPlayed) emptyClipSoundPlayed = false;
        }
    }
    private void CheckReloadInput()
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        Reload();
    }
    private void CheckAutoReload()
    {
        if (weaponItem.ammoInside > 0) return;
        Reload();
    }
    private void Shoot(bool fastShoot)
    {
        if (lockMovement.locked || lockMovement.playerBusy) return;
        if (weaponItem.ammoInside <= 0)
        {
            if(!emptyClipSoundPlayed)
            {
                audioManager.Play(emptyClipSound);
                emptyClipSoundPlayed = true;
            }
                
            return;
        }

        anim.Stop();
        anim.Play(shootAnimation.name);

        if (particleCount > 1)
        {
            for (int i = 0; i < particleCount; i++)
            {
                Vector3 randomAngle = new Vector3(Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f), Random.Range(-0.03f, 0.03f));
                Raycast(randomAngle);
            }
        }
        else Raycast(Vector3.zero);
        

        if (fastShoot)
        {
            lockMovement.KeepBusy(fireRate / 1.75f);
            recoil.IncreaseRecoil(recoilRate * 1.75f);
        }
        else
        {
            lockMovement.KeepBusy(fireRate);
            recoil.IncreaseRecoil(recoilRate);
        }

        crosshair.PlayAnimation();
        audioManager.Play(shootSound);

        if(playMuzzleEffect)
        muzzleFlash.PlayMuzzleEffect(muzzleFlashPosition);


        weaponItem.DecreaseDurability(1);
        weaponItem.DecreaseAmmoInside(1);
        inventory.UpdateSelectedSlot();
    }
    private void Zoom()
    {
        if(!zoomCanvas.activeInHierarchy)
        {
            zoomCanvas.SetActive(true);
            lockMovement.SetZoomValue(true);
        }
        else
        {
            zoomCanvas.SetActive(false);
            lockMovement.SetZoomValue(false);
        }
    }
    private void Reload()
    {
        if (lockMovement.playerBusy || weaponItem.ammoInside >= weaponItem.maxAmmoInside || lockMovement.locked) return;
        if (inventory.CalculateItemCount(weaponItem.ammoTypeNick) <= 0) return;

        anim.Play(reloadAnimation.name);
        audioManager.Play(reloadSound);
        lockMovement.KeepBusy(reloadTime);
        Invoke(nameof(FinishReload), reloadTime);
    }
    private void FinishReload()
    {
        int requiredAmmo = weaponItem.maxAmmoInside - weaponItem.ammoInside;
        int totalAmmo = inventory.CalculateItemCount(weaponItem.ammoTypeNick);

        if (totalAmmo >= requiredAmmo)
        {
            weaponItem.IncreaseAmmoInside(requiredAmmo);
            inventory.DecreaseItem(weaponItem.ammoTypeNick, requiredAmmo);
        }

        else
        {
            weaponItem.IncreaseAmmoInside(totalAmmo);
            inventory.DecreaseItem(weaponItem.ammoTypeNick, totalAmmo);
        }
    }
    private void Raycast(Vector3 difference)
    {
        RaycastHit? hit = raycast.CreateRaycast(range, difference);
        if (hit == null) return;

        switch (hit.Value.transform.root.tag)
        {
            case "Zombie": GiveDamageToZombie(hit.Value.transform.root.gameObject); particleManager.PlayHitZombieParticle(hit.Value.point); break;
            case "Building":DecreaseBuildingDurability(hit.Value.transform.root.gameObject); ammoTrace.CreateTrace(hit.Value); break;
            case "Animal": GiveDamageToAnimal(hit.Value.transform.root.root.gameObject); particleManager.PlayHitZombieParticle(hit.Value.point); break;
            default:
                if(hit.Value.transform.gameObject.layer != 3)
                ammoTrace.CreateTrace(hit.Value); 
                break;
        }
    }
    public void GiveDamageToZombie(GameObject zombieObject)
    {
        zombieObject.GetComponent<Zombie>().DecreaseHealth(gameObject, damage);
    }
    public void GiveDamageToAnimal(GameObject animalObject)
    {
        animalObject.GetComponent<Animal>().DecreaseHealth(gameObject, damage);
    }
    public void DecreaseBuildingDurability(GameObject building)
    {
        Block block = building.GetComponent<Block>();

        if (block.blockType == BlockTypes.Mine)
            building.GetComponent<Mine>().ExplodeWithShoot();
        else
        {
            block.DecreaseDurability(damage);
        }
    }
}
