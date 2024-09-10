using UnityEngine;

public class LootSpawnLocation : MonoBehaviour
{
    [Header("What types of loot can spawn in this transform? Leave blank for all.")]
    public Loots[] spawnableLootTypes;

    [Header("The array size must be the same as the loot types variable.")]
    public Quaternion[] lootsRotation;
}
