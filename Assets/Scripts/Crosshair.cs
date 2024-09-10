using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private LockMovement lockMovement;
    [SerializeField] private GameObject parentObject;

    [SerializeField] private Animation anim;
    [SerializeField] private AnimationClip crosshairAnimation;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
    }
    void Update()
    {
        if(lockMovement.playerInCar || lockMovement.zoomOn) Close();
        else Open();
    }
    private void Open()
    {
        if (parentObject.activeInHierarchy) return;

        parentObject.SetActive(true);
    }
    private void Close()
    {
        if (!parentObject.activeInHierarchy) return;

        parentObject.SetActive(false);
    }
    public void PlayAnimation()
    {
        if(anim.isPlaying) anim.Stop();

        anim.Play(crosshairAnimation.name);
    }
}
