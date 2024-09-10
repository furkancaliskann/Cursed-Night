using UnityEngine;
using UnityEngine.UIElements;

public class BiomeSoundManager : MonoBehaviour
{
    private BiomeManager biomeManager;
    private AudioSource audioSource;

    [SerializeField] private AudioClip forestSound;
    [SerializeField] private AudioClip snowSound;
    [SerializeField] private AudioClip desertSound;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        biomeManager = GetComponent<BiomeManager>();
    }
    void Update()
    {
        CheckBiomeSound();
    }  
    private void CheckBiomeSound()
    {
        Biomes currentBiome = biomeManager.currentBiome;

        if (currentBiome == Biomes.Forest) PlaySound(forestSound);
        else if (currentBiome == Biomes.Snow) PlaySound(snowSound);
        else if (currentBiome == Biomes.Desert) PlaySound(desertSound);
    }
    private void PlaySound(AudioClip sound)
    {
        if (audioSource.isPlaying && audioSource.clip == sound) return;

        audioSource.clip = sound;
        audioSource.Play();
    }
}
