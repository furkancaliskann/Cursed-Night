using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
