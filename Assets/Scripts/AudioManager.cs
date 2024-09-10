using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource weaponAudioSource;


    public void Play(AudioClip clip)
    {
        weaponAudioSource.PlayOneShot(clip);
    }
}
