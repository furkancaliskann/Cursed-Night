using UnityEngine;
public class Rock : MonoBehaviour
{
    private SpawnManager spawnManager;
    public int health { get; private set; }
    public RockType rockType;
    public int prefabModel;

    void Update()
    {
        CheckHealth();
    }
    private void CheckHealth()
    {
        if (health > 0) return;

        spawnManager.RemoveRock(gameObject);
        Destroy(gameObject);
    }
    public void DecreaseHealth(int amount)
    {
        health -= amount;
    }
    public void SetVariables(SpawnManager spawnManager, int prefabModel, int health)
    {
        this.spawnManager = spawnManager;
        this.prefabModel = prefabModel;
        this.health = health;
    }
}

public enum RockType
{
    Stone,
    Coal,
    Sulfur,
    Iron
};
