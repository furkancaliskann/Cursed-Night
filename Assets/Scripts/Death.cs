using UnityEngine;

public class Death : MonoBehaviour
{
    private Building building;
    private Camera cam;
    private Inventory inventory;
    private LockMovement lockMovement;
    private Player player;

    [SerializeField] private GameObject deadPlayerPrefab;
    [SerializeField] private GameObject backpackPrefab;

    void Awake()
    {
        building = GetComponent<Building>();
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
        player = GetComponent<Player>();
    }
    public void StartDeathProcess()
    {
        if(lockMovement.playerInCar)
        {
            player.StopCar();
        }
        GameObject backPack = Instantiate(backpackPrefab, transform.position, transform.rotation);
        backPack.GetComponent<Backpack>().TransferItems(inventory.slots);

        building.DestroyPreviewObject();

        GameObject deadPlayer = Instantiate(deadPlayerPrefab, transform.position, transform.rotation);
        deadPlayer.GetComponent<DeadPlayer>().SetVariables(gameObject, cam.transform.eulerAngles);
    }
}
