using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    private CloseRange closeRange;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip attackAnimation;

    private float range = 2.5f;
    private float speed = 1.25f;
    private int damage = 40;
    private float damageLatency = 0.4f;
    private float energyCost = 8f;

    void Awake()
    {
        closeRange = GetComponent<CloseRange>();
    }
    public void GetVariables()
    {
        closeRange.SetSelectedItemVariables(anim, attackAnimation, range, speed, damage, damageLatency, energyCost);
    }
}
