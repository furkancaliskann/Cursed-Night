using UnityEngine;

public class Pistol : MonoBehaviour
{
    private Weapon weapon;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip shootAnimation;
    [SerializeField] private AnimationClip reloadAnimation;

    [SerializeField] private GameObject zoomCanvas;

    private Vector3 muzzleFlashPosition = new Vector3(0.35f, -0.27f, 1f);
    
    private float recoilRate = 0.8f;
    private float reloadTime = 1.75f;
    private float range = 50f;
    private float fireRate = 0.35f;
    private int damage = 40;

    private int particleCount = 1;
    private bool playMuzzleEffect = true;
    private bool fastShootSupport = true;
    private bool zoomSupport = false;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
    public void GetVariables()
    {
        weapon.SetSelectedItemVariables(shootSound, reloadSound, anim, shootAnimation, reloadAnimation, zoomCanvas,
            muzzleFlashPosition, recoilRate, reloadTime, range, fireRate, damage, particleCount, playMuzzleEffect,
            fastShootSupport, zoomSupport);
    }
}


