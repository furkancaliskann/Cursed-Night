using UnityEngine;
public class Plane : MonoBehaviour
{
    private LootManager lootManager;
    private SpawnManager spawnManager;
    private Terrain terrain;
    [SerializeField] private GameObject airdropPrefab;

    public bool flyStarted {  get; private set; }
    private Vector3 targetVector;
    private float speed = 50f;

    void Awake()
    {
        lootManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootManager>();
        spawnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>();
        terrain = Terrain.activeTerrain;
    }
    void Update()
    {
        FlyWithTargetVector();
    }
    public void SetTarget(Vector3 targetPosition)
    {
        transform.LookAt(targetPosition);
        targetVector = (targetPosition - transform.position).normalized;

        float dropTime = CreateDropTime();
        if(dropTime > 0)
        {
            Invoke(nameof(DropPackage), dropTime);
            flyStarted = true;
            Invoke(nameof(StopFlying), 30f);
        }
        else
        {
            StopFlying();
            spawnManager.SpawnPlane();
        } 
    }
    private void FlyWithTargetVector()
    {
        if (targetVector != null && targetVector != Vector3.zero && flyStarted)
        {
            transform.Translate(targetVector * speed * Time.deltaTime, Space.World);
        }
    }
    private float CreateDropTime()
    {
        int counter = 0;

        while(true)
        {
            if(counter >= 20)
            {
                return -1;
            }
            float randomNumber = Random.Range(3f, 20f);
            Vector3 point = transform.position + (targetVector * speed * randomNumber);
            counter++;

            if (point.x > 0 && point.x < terrain.terrainData.size.x - 20 && point.z > 0 && point.z < terrain.terrainData.size.z - 20)
                return randomNumber;
        }
    }
    private void DropPackage()
    {
        GameObject drop = Instantiate(airdropPrefab,transform.position + new Vector3(0, -5f, 0), Quaternion.Euler(-90,0, transform.rotation.eulerAngles.y));
        drop.GetComponent<Loot>().FillItems();
        lootManager.AddOtherLoot(drop);
    }
    private void StopFlying()
    {
        transform.position = Vector3.zero;
        targetVector = Vector3.zero;
        flyStarted = false;
        gameObject.SetActive(false);
    }
}
