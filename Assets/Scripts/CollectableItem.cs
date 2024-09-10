using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public string nickName;
    public string displayedName;
    public int amount;

    public CollectableType type;
}

public enum CollectableType
{
    Mushroom,
    Stone,
    WoodPile,
    BirdNest
}