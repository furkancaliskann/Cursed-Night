using UnityEngine;

public class Spear : MonoBehaviour
{
    private CloseRange closeRange;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip attackAnimation;

    private float range = 3.5f;
    private float speed = 1.2f;
    private int damage = 70;
    private float damageLatency = 0.45f;
    private float energyCost = 10f;

    void Awake()
    {
        closeRange = GetComponent<CloseRange>();
    }
    public void GetVariables()
    {
        closeRange.SetSelectedItemVariables(anim, attackAnimation, range, speed, damage, damageLatency, energyCost);
    }
}
