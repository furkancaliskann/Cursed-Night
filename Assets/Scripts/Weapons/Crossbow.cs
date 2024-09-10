using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    private Weapon weapon;

    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip shootAnimation;
    [SerializeField] private AnimationClip reloadAnimation;

    [SerializeField] private GameObject zoomCanvas;

    private Vector3 muzzleFlashPosition = Vector3.zero;

    private float recoilRate = 0.5f;
    private float reloadTime = 1f;
    private float range = 75f;
    private float fireRate = 0.5f;
    private int damage = 120;

    private int particleCount = 1;
    private bool playMuzzleEffect = false;
    private bool fastShootSupport = false;
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
