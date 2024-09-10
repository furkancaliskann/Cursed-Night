using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    private LockMovement lockMovement;
    private ParticleSystem particle;
    [SerializeField] private GameObject muzzleFlash;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
        particle = muzzleFlash.GetComponent<ParticleSystem>();
    }
    public void PlayMuzzleEffect(Vector3 muzzlePosition)
    {
        if (lockMovement.zoomOn) return;

        muzzleFlash.transform.localPosition = muzzlePosition;
        muzzleFlash.SetActive(true);
        particle.Emit(1);
        CancelInvoke(nameof(CloseMuzzleEffect));
        Invoke(nameof(CloseMuzzleEffect), 0.05f);
    }
    private void CloseMuzzleEffect()
    {
        muzzleFlash.SetActive(false);
        muzzleFlash.transform.localPosition = Vector3.zero;
        particle.Stop();
    }
}
