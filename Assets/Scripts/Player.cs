using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera cam;
    private CarCanvas carCanvas;
    private CharacterController characterController;
    private GameManager gameManager;
    private HealthNotification healthNotification;
    private LockMovement lockMovement;
    private MouseMovement mouseMovement;
    private OptimizationManager optimizationManager;
    private PlayerItems playerItems;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private SaveManager saveManager;
    private SpawnManager spawnManager;
    private Translations translations;

    [SerializeField] private CameraCarFollow cameraCarFollow;
    [SerializeField] private GameObject beltSlots;
    [SerializeField] private GameObject carPanel;
    [SerializeField] private GameObject body;
    private GameObject currentCar;
    private float counter = 0f;
    private float counterMax = 0.1f;

    public bool godMode {  get; private set; }

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        carCanvas = GetComponent<CarCanvas>();
        characterController = GetComponent<CharacterController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        healthNotification = GetComponent<HealthNotification>();
        lockMovement = GetComponent<LockMovement>();
        mouseMovement = GetComponent<MouseMovement>();
        optimizationManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<OptimizationManager>();
        playerItems = GetComponent<PlayerItems>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        saveManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveManager>();
        spawnManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnManager>();
        translations = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Translations>();

        optimizationManager.SetPlayer(gameObject);
        spawnManager.SetPlayer(gameObject);
    }
    void Update()
    {
        CheckTerrainError();
        CheckStopCarInput();
        CheckCounter();
    }
    private void CheckTerrainError()
    {
        if (transform.position.y < -50)
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        gameManager.SpawnPlayer(gameObject);
    }
    public void ReturnMainMenu()
    {
        saveManager.SaveAll();
        gameManager.ChangeScene("Main Menu");
    }
    public string ChangeGodMode()
    {
        if(!godMode)
        {
            godMode = true;
            healthNotification.DeleteAll();
            playerStats.MaximizeAllStats();
            playerMovement.ChangeRunSpeed(100);
            return translations.Get("GodModeActivated");
        }
        else
        {
            godMode = false;
            playerMovement.ResetRunSpeed();
            return translations.Get("GodModeDeactivated");
        }
    }
    private void CheckCounter()
    {
        if (counter > 0) counter -= Time.deltaTime;
    }
    public void DriveCar(GameObject car)
    {
        if (counter > 0 || lockMovement.locked || lockMovement.playerBusy) return;

        lockMovement.SetPlayerInCar(true);
        playerItems.ClearSelectedItemForCar();
        playerItems.CloseItems();
        beltSlots.SetActive(false);
        body.SetActive(false);
        CarController carController = car.GetComponent<CarController>();
        carCanvas.SetCarController(carController);
        carPanel.SetActive(true);
        characterController.enabled = false;
        cameraCarFollow.SetCarTarget(car.transform);
        cameraCarFollow.enabled = true;
        carController.SetPlayer(gameObject);
        carController.SetCarStartedDelay(true);
        currentCar = car;

        counter = counterMax;
    }
    public void StopCar()
    {
        if (currentCar == null || counter > 0 || lockMovement.locked || lockMovement.playerBusy) return;

        CarController carController = currentCar.GetComponent<CarController>();
        if (!carController.carStarted) return;
        lockMovement.SetPlayerInCar(false);
        carCanvas.SetCarController(null);
        carController.SetPlayer(null);
        carPanel.SetActive(false);
        carController.SetCarStartedDelay(false);
        cameraCarFollow.enabled = false;
        cameraCarFollow.SetCarTarget(null);
        transform.position = carController.landingPoint.position;
        transform.rotation = Quaternion.Euler(0, currentCar.transform.rotation.eulerAngles.y, 0);
        cam.transform.localPosition = playerMovement.originalCameraLocalPosition;
        currentCar = null;

        body.SetActive(true);
        beltSlots.SetActive(true);
        characterController.enabled = true;
        counter = counterMax;
    }
    private void CheckStopCarInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            StopCar();
        }
    }
    public void LoadPlayerPosition()
    {
        SavePlayerPosition savedPos = JsonUtility.FromJson<SavePlayerPosition>(saveManager.Load(SaveType.PlayerPosition));

        characterController.enabled = false;

        if (savedPos == null)
        {
            transform.position = gameManager.GetRandomSpawnPos();
            characterController.enabled = true;
            return;
        }

        
        transform.position = savedPos.position;
        transform.rotation = savedPos.rotation;
        mouseMovement.SetXRotation(savedPos.cameraXRotation);
        characterController.enabled = true;
    }
    public void SavePlayerPosition()
    {
        SavePlayerPosition save = new SavePlayerPosition(transform.position, transform.rotation, mouseMovement.xRotation);
        StartCoroutine(saveManager.Save(save, SaveType.PlayerPosition));
    }
}

[System.Serializable]
public class SavePlayerPosition
{
    public Vector3 position;
    public Quaternion rotation;
    public float cameraXRotation;

    public SavePlayerPosition(Vector3 position, Quaternion rotation, float cameraXRotation)
    {
        this.position = position;
        this.rotation = rotation;
        this.cameraXRotation = cameraXRotation;
    }
}