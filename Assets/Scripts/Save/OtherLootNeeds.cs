using UnityEngine;

public class OtherLootNeeds : MonoBehaviour
{
    private Animation anim;
    [SerializeField] private AnimationClip dieAnimation;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }
    void Start()
    {
        Loots lootType = GetComponent<Loot>().lootType;

        if (lootType == Loots.Zombie || lootType == Loots.Deer)
            anim.Play(dieAnimation.name);
    }
}
