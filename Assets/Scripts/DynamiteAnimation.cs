using UnityEngine;

public class DynamiteAnimation : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particle;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        particle = GetComponent<ParticleSystem>();
    }
    void Start()
    {
        Destroy(gameObject, 2.5f);
    }
    public void PlayAnimation()
    {
        particle.Emit(1000);
        audioSource.Play();
    }
}
