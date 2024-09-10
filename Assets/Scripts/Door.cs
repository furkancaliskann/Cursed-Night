using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip openSound;

    public bool isOpen {  get; private set; }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OpenOrCloseDoor()
    {
        if(!isOpen)
        {
            PlayDoorOpenSound();
            transform.position = transform.position + new Vector3(-0.50f, 0f, -0.50f);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y - 90f, 0f);          
            //anim.Play("DoorOpen");
        }
        else
        {
            PlayDoorCloseSound();
            transform.position = transform.position + new Vector3(0.50f, 0, 0.50f);
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + 90f, 0f);
            //anim.Play("DoorClose");
        }

        isOpen = !isOpen;
    }
    private void PlayDoorOpenSound()
    {
        audioSource.PlayOneShot(openSound);
    }
    private void PlayDoorCloseSound()
    {
        audioSource.PlayOneShot(closeSound);
    }
    public void LoadIsOpen(bool value)
    {
        isOpen = value;
    }
}
