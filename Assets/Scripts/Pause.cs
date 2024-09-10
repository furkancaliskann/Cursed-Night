using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    private LockMovement lockMovement;
    private Player player;   

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button respawnButton;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
        player = GetComponent<Player>();
    }
    void Start()
    {
        Time.timeScale = 1f;
    }
    void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Open();
        }
    }
    private void Open()
    {
        if (lockMovement.locked) return;

        if (player.godMode)
            respawnButton.interactable = true;
        else
            respawnButton.interactable = false;

        pausePanel.SetActive(true);
        lockMovement.Lock();
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }
    public void Close()
    {
        if (!lockMovement.locked) return;

        pausePanel.SetActive(false);
        lockMovement.Unlock();
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }
}
