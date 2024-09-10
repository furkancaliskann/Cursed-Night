using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingRifle : MonoBehaviour
{
    private Weapon weapon;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip shootAnimation;
    [SerializeField] private AnimationClip reloadAnimation;

    [SerializeField] private GameObject zoomCanvas;

    private Vector3 muzzleFlashPosition = new Vector3(0.38f, 0f, 1.5f);

    private float recoilRate = 1.5f;
    private float reloadTime = 3.2f;
    private float range = 200f;
    private float fireRate = 2f;
    private int damage = 250;

    private int particleCount = 1;
    private bool playMuzzleEffect = true;
    private bool fastShootSupport = false;
    private bool zoomSupport = true;

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
