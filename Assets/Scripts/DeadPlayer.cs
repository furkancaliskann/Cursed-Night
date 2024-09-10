using UnityEngine;
using UnityEngine.UI;

public class DeadPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private GameManager gameManager;
    private SaveSpawnPoint saveSpawnPoint;

    [SerializeField] private Animation anim;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private GameObject cameraObject;
    [SerializeField] private GameObject deathCanvas;
    [SerializeField] private GameObject respawnPanel;

    [SerializeField] private Button respawnRegisteredPointButton;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        saveSpawnPoint = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveSpawnPoint>();

        if (saveSpawnPoint.registeredSpawnPoint != Vector3.zero) respawnRegisteredPointButton.interactable = true;
    }
    public void SetVariables(GameObject deadPlayer, Vector3 cameraRotation)
    {
        Destroy(deadPlayer);
        cameraObject.transform.rotation = Quaternion.Euler(cameraRotation);
        cameraObject.SetActive(true);
        PlayDeathSound();
        OpenDeathCanvas();
        PlayCameraAnimation();
    }
    private void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    private void OpenDeathCanvas()
    {
        deathCanvas.SetActive(true);
    }
    private void PlayCameraAnimation()
    {
        anim.Play("Death");
        Invoke(nameof(OpenRespawnPanel), anim.clip.length + 0.5f);
    }
    private void OpenRespawnPanel()
    {
        respawnPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Respawn()
    {
        gameManager.SpawnPlayer(gameObject);
    }
    public void RespawnRegisteredPoint()
    {
        gameManager.SpawnWithRegisteredPoint(gameObject);
    }
}
