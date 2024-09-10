using UnityEngine;

public class TreeCode : MonoBehaviour
{
    private ParticleManager particleManager;
    private SpawnManager spawnManager;

    public int health { get; private set; }
    public TreeType type;
    public int prefabModel;

    [SerializeField] private MeshCollider[] meshColliders;
    [SerializeField] private Collider simpleCollider;
    private Rigidbody rb;
    [SerializeField] private AudioClip treeFallingSound;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (health > 0) return;

        spawnManager.RemoveTree(gameObject);
        gameObject.tag = "Untagged";
    }
    public void PlayTreeDownEffect(Vector3 playerPosition, Vector3 hitPoint)
    {
        for (int i = 0; i < meshColliders.Length; i++)
        {
            meshColliders[i].enabled = false;
        }

        simpleCollider.enabled = true;
        rb.isKinematic = false;

        Vector3 treePosition = transform.position;

        Vector3 playerToTree = (playerPosition - treePosition).normalized;
        Vector3 reverseTorque = Vector3.Cross(playerToTree, Vector3.up);
        rb.AddTorque(reverseTorque * 100f, ForceMode.Impulse);

        particleManager.PlayTreeDownParticle(hitPoint);
        GetComponent<AudioSource>().PlayOneShot(treeFallingSound);

        Destroy(gameObject, 4f);
    }
    public void DecreaseHealth(int amount, Vector3 playerPosition, Vector3 hitPoint)
    {
        health -= amount;
        if (health <= 0) PlayTreeDownEffect(playerPosition, hitPoint);
    }
    public void SetVariables(SpawnManager spawnManager, int prefabModel, int health)
    {
        this.spawnManager = spawnManager;
        this.prefabModel = prefabModel;
        this.health = health;
        particleManager = spawnManager.GetComponent<ParticleManager>();
    }
}

public enum TreeType
{
    Forest,
    SnowyForest,
    Desert,
}
