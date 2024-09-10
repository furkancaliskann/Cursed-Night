using UnityEngine;

public class Block : MonoBehaviour
{
    private GameManager gameManager;
    private ItemList itemList;
    private ParticleManager particleManager;
    private SaveBuildings saveBuildings;
    private SaveSpawnPoint saveSpawnPoint;

    //private AudioSource audioSource;
    //[SerializeField] private AudioClip destroySound;
    [SerializeField] private GameObject destroyParticlePrefab;

    public string nickName { get; private set; }
    public float durability;
    public float maxDurability { get; private set; }
    public BlockTypes blockType;
    public bool upgradeable { get; private set; }
    public MaterialType materialType;

    void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        itemList = gameManager.GetComponent<ItemList>();
        particleManager = gameManager.GetComponent<ParticleManager>();
        saveBuildings = gameManager.GetComponent<SaveBuildings>();
        saveSpawnPoint = gameManager.GetComponent<SaveSpawnPoint>();
    }
    public void SetValues(string nickName, float durability)
    {
        this.nickName = nickName;

        Item thisItem = itemList.GetItem(nickName);

        this.durability = durability;
        maxDurability = thisItem.maxDurability;
        blockType = thisItem.blockType;
        upgradeable = thisItem.upgradeable;
        materialType = thisItem.materialType;
    }
    public void UpgradeBlock(MaterialType materialType)
    {
        saveBuildings.RemoveBlock(gameObject);
        Destroy(gameObject);

        Item item = itemList.GetUpgradeBlock(blockType, materialType);
        GameObject myObject = Instantiate(item.blockPrefab, transform.position, transform.rotation);
        myObject.GetComponent<Block>().SetValues(item.nickName, item.durability);

        saveBuildings.AddBlock(myObject);
    }
    public void DecreaseDurability(int amount)
    {
        if (durability <= 0) return;

        durability -= amount;
        if(durability <= 0)
        {
            particleManager.PlayBlockDestroyParticle(transform.position);

            //audioSource.PlayOneShot(destroySound);

            saveBuildings.RemoveBlock(gameObject);
            if (blockType == BlockTypes.SleepingBag && transform.position == saveSpawnPoint.registeredSpawnPoint &&
                transform.rotation.eulerAngles == saveSpawnPoint.registeredSpawnRotation) saveSpawnPoint.ClearPoint(); 
            Destroy(gameObject);
        } 
    }
    public void IncreaseDurability(int amount)
    {
        durability += amount;
        if (durability > maxDurability) durability = maxDurability;
    }
}
