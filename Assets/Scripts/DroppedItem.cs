using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    public Item item;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        PlayDropSound();
        Destroy(gameObject, 120f);
    }
    private void PlayDropSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
