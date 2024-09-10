using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    private CloseRange closeRange;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip attackAnimation;

    private float range = 2.5f;
    private float speed = 1f;
    private int damage = 100;
    private float damageLatency = 0.45f;
    private float energyCost = 9f;

    void Awake()
    {
        closeRange = GetComponent<CloseRange>();
    }
    public void GetVariables()
    {
        closeRange.SetSelectedItemVariables(anim, attackAnimation, range, speed, damage, damageLatency, energyCost);
    }
}
