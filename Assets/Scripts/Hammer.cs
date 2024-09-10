using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Inventory inventory;
    private LockMovement lockMovement;
    private ParticleManager particleManager;
    private PlayerItems playerItems;
    private Raycast raycast;

    private AudioSource audioSource;

    [SerializeField] private AudioClip upgradeSound;
    [SerializeField] private AudioClip repairSound;

    private Item hammer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        particleManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ParticleManager>();
        playerItems = GetComponent<PlayerItems>();
        raycast = GetComponent<Raycast>();
    }
    void Update()
    {
        CheckSelectedItem();
        if (hammer == null) return;

        CheckInputs();
    }
    private void CheckSelectedItem()
    {
        if (playerItems.selectedItem == null || playerItems.selectedItem.nickName != "hammer")
        {
            if(hammer != null)
                hammer = null;
        }
        else
        {
            if(hammer != playerItems.selectedItem)
                hammer = playerItems.selectedItem;
        }
    }
    private void CheckInputs()
    {
        if (Input.GetMouseButton(1) && !lockMovement.locked && !lockMovement.playerBusy)
        {
            lockMovement.KeepBusy(0.5f);
            CheckRaycast();
        }
    }
    private void CheckRaycast()
    {
        RaycastHit? hit = raycast.CreateRaycast(4f);
        if (hit == null || !hit.Value.transform.root.CompareTag("Building")) return;
        
        Block block = hit.Value.transform.root.GetComponent<Block>();
        if (block == null) return;

        if (block.durability < block.maxDurability)
        {
            Repair(block);
        }
        else
        {
            TryUpgrade(block, hit.Value.point);
        }
    }
    private void Repair(Block block)
    {
        if (block.materialType == MaterialType.Wood || block.materialType == MaterialType.Frame)
        {
            if (inventory.CalculateItemCount("wood") < 10) return;
            block.IncreaseDurability(100);
            DecreaseFromInventory("wood", 10);
            audioSource.PlayOneShot(repairSound);
        }
        else if (block.materialType == MaterialType.Stone)
        {
            if (inventory.CalculateItemCount("stone") < 10) return;
            block.IncreaseDurability(100);
            DecreaseFromInventory("stone", 10);
            audioSource.PlayOneShot(repairSound);
        }
        else if (block.materialType == MaterialType.Iron)
        {
            if (inventory.CalculateItemCount("forged_iron") < 10) return;
            block.IncreaseDurability(100);
            DecreaseFromInventory("forged_iron", 10);
            audioSource.PlayOneShot(repairSound);
        }
    }
    public void TryUpgrade(Block block, Vector3 hitPoint)
    {
        if (!block.upgradeable || block.materialType == MaterialType.Iron) return;

        if(block.blockType == BlockTypes.Door)
        {
            if(block.materialType == MaterialType.Wood)
            {
                if (inventory.CalculateItemCount("forged_iron") < 250) return;
                else
                {
                    DecreaseFromInventory("forged_iron", 250);
                    block.UpgradeBlock(MaterialType.Iron);
                    particleManager.PlayHitTreeParticle(hitPoint);
                    audioSource.PlayOneShot(upgradeSound);
                }
                    
            }
        }
        else
        {
            if (block.materialType == MaterialType.Frame)
            {
                if (inventory.CalculateItemCount("wood") < 50) return;
                else
                {
                    DecreaseFromInventory("wood", 50);
                    block.UpgradeBlock(MaterialType.Wood);
                    particleManager.PlayHitTreeParticle(hitPoint);
                    audioSource.PlayOneShot(upgradeSound);
                    return;
                }
            }
            else if (block.materialType == MaterialType.Wood)
            {
                if (inventory.CalculateItemCount("stone") < 50) return;
                else
                {
                    DecreaseFromInventory("stone", 50);
                    block.UpgradeBlock(MaterialType.Stone);
                    particleManager.PlayHitTreeParticle(hitPoint);
                    audioSource.PlayOneShot(upgradeSound);
                    return;
                }                  
            }
            else if (block.materialType == MaterialType.Stone)
            {
                if (inventory.CalculateItemCount("forged_iron") < 50) return;
                else
                {
                    DecreaseFromInventory("forged_iron", 50);
                    block.UpgradeBlock(MaterialType.Iron);
                    particleManager.PlayHitTreeParticle(hitPoint);
                    audioSource.PlayOneShot(upgradeSound);
                    return;
                }
            }
        }
    }
    private void DecreaseFromInventory(string nickName, int amount)
    {
        playerItems.selectedItem.DecreaseDurability(1);
        inventory.UpdateSelectedSlot();
        inventory.DecreaseItem(nickName, amount);
    }
}