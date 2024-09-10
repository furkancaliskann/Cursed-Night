using Unity.VisualScripting;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private BiomeManager biomeManager;
    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds; // 8 - 10

    // Biome
    [SerializeField] private AudioClip[] forestWalkingSounds;
    [SerializeField] private AudioClip[] forestRunningSounds;
    [SerializeField] private AudioClip[] snowWalkingSounds;
    [SerializeField] private AudioClip[] snowRunningSounds;
    [SerializeField] private AudioClip[] desertWalkingSounds;
    [SerializeField] private AudioClip[] desertRunningSounds;

    // Block
    [SerializeField] private AudioClip[] metalWalkingSounds;
    [SerializeField] private AudioClip[] metalRunningSounds;
    [SerializeField] private AudioClip[] stoneWalkingSounds;
    [SerializeField] private AudioClip[] stoneRunningSounds;
    [SerializeField] private AudioClip[] woodWalkingSounds;
    [SerializeField] private AudioClip[] woodRunningSounds;

    private int soundNumber;
    private bool playingRunSound;

    [SerializeField] private AudioSource waterAudioSource;
    [SerializeField] private AudioClip swimmingSound;
    [SerializeField] private AudioClip underWaterSound;
    [SerializeField] private AudioClip jumpSound;
    private AudioClip currentWaterClip;

    void Awake()
    {
        biomeManager = GetComponent<BiomeManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void PlayFootstepSounds(bool isRunning)
    {
        if ((audioSource.isPlaying && isRunning && playingRunSound) ||
            (audioSource.isPlaying && !isRunning && !playingRunSound) || !playerMovement.isGrounded) return;

        Biomes currentBiome = biomeManager.currentBiome;
        MaterialType materialType = playerMovement.groundObjectMaterialType;

        if (isRunning)
        {
            audioSource.pitch = 1.05f;
            if (soundNumber == 0)
            {
                if (materialType == MaterialType.Wood) audioSource.PlayOneShot(woodRunningSounds[1]);
                else if (materialType == MaterialType.Stone) audioSource.PlayOneShot(stoneRunningSounds[1]);
                else if (materialType == MaterialType.Iron) audioSource.PlayOneShot(metalRunningSounds[1]);
                else if (currentBiome == Biomes.Forest) audioSource.PlayOneShot(forestRunningSounds[1]);
                else if (currentBiome == Biomes.Snow) audioSource.PlayOneShot(snowRunningSounds[1]);
                else if (currentBiome == Biomes.Desert) audioSource.PlayOneShot(desertRunningSounds[1]);
            }
            else
            {
                if (materialType == MaterialType.Wood) audioSource.PlayOneShot(woodRunningSounds[0]);
                else if (materialType == MaterialType.Stone) audioSource.PlayOneShot(stoneRunningSounds[0]);
                else if (materialType == MaterialType.Iron) audioSource.PlayOneShot(metalRunningSounds[0]);
                else if (currentBiome == Biomes.Forest) audioSource.PlayOneShot(forestRunningSounds[0]);
                else if (currentBiome == Biomes.Snow) audioSource.PlayOneShot(snowRunningSounds[0]);
                else if (currentBiome == Biomes.Desert) audioSource.PlayOneShot(desertRunningSounds[0]);
            }
            playingRunSound = true;
        }
        else
        {
            audioSource.pitch = 1f;
            if (soundNumber == 0)
            {
                if (materialType == MaterialType.Wood) audioSource.PlayOneShot(woodWalkingSounds[1]);
                else if (materialType == MaterialType.Stone) audioSource.PlayOneShot(stoneWalkingSounds[1]);
                else if (materialType == MaterialType.Iron) audioSource.PlayOneShot(metalWalkingSounds[1]);
                else if (currentBiome == Biomes.Forest) audioSource.PlayOneShot(forestWalkingSounds[1]);
                else if (currentBiome == Biomes.Snow) audioSource.PlayOneShot(snowWalkingSounds[1]);
                else if (currentBiome == Biomes.Desert) audioSource.PlayOneShot(desertWalkingSounds[1]);
            }
            else
            {
                if (materialType == MaterialType.Wood) audioSource.PlayOneShot(woodWalkingSounds[0]);
                else if (materialType == MaterialType.Stone) audioSource.PlayOneShot(stoneWalkingSounds[0]);
                else if (materialType == MaterialType.Iron) audioSource.PlayOneShot(metalWalkingSounds[0]);
                else if (currentBiome == Biomes.Forest) audioSource.PlayOneShot(forestWalkingSounds[0]);
                else if (currentBiome == Biomes.Snow) audioSource.PlayOneShot(snowWalkingSounds[0]);
                else if (currentBiome == Biomes.Desert) audioSource.PlayOneShot(desertWalkingSounds[0]);
            }
            playingRunSound = false;
        }

        if (soundNumber == 0) soundNumber = 1;
        else soundNumber = 0;
    }
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    public void PlaySwimmingSound()
    {
        if (waterAudioSource.isPlaying && currentWaterClip == swimmingSound) return;
        if(waterAudioSource.isPlaying && currentWaterClip == underWaterSound)
        {
            StopWaterAudioSource();
        }

        waterAudioSource.PlayOneShot(swimmingSound);
        currentWaterClip = swimmingSound;
    }
    public void PlayUnderWaterSound()
    {
        if (waterAudioSource.isPlaying && currentWaterClip == underWaterSound) return;
        if (waterAudioSource.isPlaying && currentWaterClip == swimmingSound)
        {
            StopWaterAudioSource();
        }

        waterAudioSource.PlayOneShot(underWaterSound);
        currentWaterClip = underWaterSound;
    }
    public void StopWaterAudioSource()
    {
        if (!waterAudioSource.isPlaying) return;
        waterAudioSource.Stop();
    }
}
