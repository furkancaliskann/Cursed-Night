using UnityEngine;

public class Injury : MonoBehaviour
{
    private Animation anim;
    private AudioSource audioSource;

    [SerializeField] private GameObject injuryEffect;
    [SerializeField] private AnimationClip injuryClip;
    [SerializeField] private AudioClip injurySound;

    void Awake()
    {
        anim = injuryEffect.GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();
    }
    public void StartInjuryEffect()
    {
        if (anim.isPlaying)
        {
            anim.Stop();
            CancelInvoke(nameof(StopInjuryEffect));
        }

        injuryEffect.SetActive(true);
        anim.Play(injuryClip.name);
        audioSource.PlayOneShot(injurySound);

        Invoke(nameof(StopInjuryEffect), anim.clip.length);
    }
    private void StopInjuryEffect()
    {
        injuryEffect.SetActive(false);
    }
}
