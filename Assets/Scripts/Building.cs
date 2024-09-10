using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private AudioSource audioSource;
    private Camera cam;
    private Inventory inventory;
    private PlayerItems playerItems;
    private Raycast raycast;
    private SaveBuildings saveBuildings;
    private SaveSpawnPoint saveSpawnPoint;

    [SerializeField] private AudioClip placeSound;

    private float range = 5f;
    private List<float> rotationControl = new List<float>
    {
        0,90,180,270
    };
    private int rotationNumber = 0;
    private int customBuildIndex = -1;

    private List<CustomBuilds> customBuilds = new List<CustomBuilds>();

    private List<CustomBuild> doorCustomBuild = new List<CustomBuild>()
    {
        new CustomBuild{position = Vector3.zero, rotation = 90f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = Vector3.zero, rotation = 180f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = Vector3.zero, rotation = 270f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = new Vector3(0f, 0f, 0.45f), rotation = 0f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = new Vector3(0.45f, 0f, 0f), rotation = 90f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = new Vector3(0f, 0f, -0.45f), rotation = 0f, colliderCenter = new Vector3(0f, 1f, 0f)},
        new CustomBuild{position = new Vector3(-0.45f, 0f, 0f), rotation = 90f, colliderCenter = new Vector3(0f, 1f, 0f)},
    };
    private List<CustomBuild> ladderCustomBuild = new List<CustomBuild>()
    {
        new CustomBuild{position = new Vector3(0f, 0f, 0.45f), rotation = 0f, colliderCenter = new Vector3(0f, 0.5f, 0f) },
        new CustomBuild{position = new Vector3(0.45f, 0f, 0f), rotation = 90f, colliderCenter = new Vector3(0f, 0.5f, 0f) },
        new CustomBuild{position = new Vector3(0, 0f, -0.45f), rotation = 0f, colliderCenter = new Vector3(0f, 0.5f, 0) },
        new CustomBuild{position = new Vector3(-0.45f, 0f, 0f), rotation = 90f, colliderCenter = new Vector3(0f, 0.5f, 0f) },
    };

    private Item selectedItem;
    private GameObject previewObject;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        playerItems = GetComponent<PlayerItems>(); 
        raycast = GetComponent<Raycast>();
        saveBuildings = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveBuildings>();
        saveSpawnPoint = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveSpawnPoint>();

        customBuilds.Add(new CustomBuilds { blockType = BlockTypes.Door, customBuild = doorCustomBuild });
        customBuilds.Add(new CustomBuilds { blockType = BlockTypes.Ladder, customBuild = ladderCustomBuild });
    }
    void Update()
    {
        if (playerItems.selectedItem == null || playerItems.selectedItem.blockType == BlockTypes.Empty)
        {
            DestroyPreviewObject();
            return;
        }

        GetSelectedItem();
        CheckRaycast();
        CheckInputs();
    }
    private void GetSelectedItem()
    {
        if(selectedItem != playerItems.selectedItem)
        {
            customBuildIndex = -1;
            rotationNumber = 0;
            DestroyPreviewObject();
            selectedItem = playerItems.selectedItem;

            var isCustomBuild = customBuilds.FindIndex(x => x.blockType == selectedItem.blockType);
            if (isCustomBuild != -1) customBuildIndex = isCustomBuild;
        }
    }
    private void CheckInputs()
    {
        if (previewObject == null) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (previewObject.activeInHierarchy && previewObject.GetComponent<BuildingPreview>().number.Count == 0)
            {
                PlaceBuilding(previewObject.transform.position, previewObject.transform.eulerAngles);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotationNumber++;

            if(customBuildIndex == -1)
            {
                if (rotationNumber >= rotationControl.Count) rotationNumber = 0;
            }
            else
            {
                if (rotationNumber >= customBuilds[customBuildIndex].customBuild.Count) rotationNumber = 0;
            }
                
        }
    }
    private void CheckRaycast()
    {
        RaycastHit? result = raycast.CreateRaycast(range);

        if (result == null || !CheckPlaceableGround(result.Value.transform.root.tag))
        {
            DestroyPreviewObject();
            return;
        }

        RaycastHit hit = result.Value;
        Vector3 newPos = hit.point - cam.transform.forward * 0.01f;

        if(hit.transform.root.CompareTag("Building"))
        {
            Block block = hit.transform.root.GetComponent<Block>();
            
            if(block.blockType == BlockTypes.Frame || block.blockType == BlockTypes.Glass)
            {
                float valueX = hit.point.x - hit.transform.position.x;
                if (valueX == -0.5f) valueX = -1f;
                else if (valueX == 0.5f) valueX = 1f;
                else valueX = 0f;

                float valueY = hit.point.y - hit.transform.position.y;
                if (valueY >= 0.99f) valueY = 1f;
                else if (valueY == 0) valueY = -1f;
                else valueY = 0f;

                float valueZ = hit.point.z - hit.transform.position.z;
                if (valueZ == -0.5f) valueZ = -1f;
                else if (valueZ == 0.5f) valueZ = 1f;
                else valueZ = 0f;

                newPos = hit.transform.position + new Vector3(valueX, valueY, valueZ);
            }
        }

        if (selectedItem.blockPreview != null && previewObject == null)
            previewObject = Instantiate(selectedItem.blockPreview);
        SetPreviewObjectPosition(newPos);  
    }
    private bool CheckPlaceableGround(string raycastTag)
    {
        if (selectedItem == null) return false;

        if(selectedItem.placeableGrounds == PlaceableGrounds.Terrain && raycastTag == "Terrain") return true;
        if(selectedItem.placeableGrounds == PlaceableGrounds.Frames && raycastTag == "Building") return true;
        if (selectedItem.placeableGrounds == PlaceableGrounds.All && (raycastTag == "Terrain" || raycastTag == "Building")) return true;

        return false;

    }
    private void SetPreviewObjectPosition(Vector3 position)
    {
        if (previewObject == null) return;

        if (customBuildIndex != -1)
        {
            List<CustomBuild> build = customBuilds[customBuildIndex].customBuild;

            if (previewObject.transform.position == position + build[rotationNumber].position &&
               previewObject.transform.rotation == Quaternion.Euler(0, build[rotationNumber].rotation, 0)) return;

            previewObject.transform.position = position + build[rotationNumber].position;
            previewObject.transform.rotation = Quaternion.Euler(0, build[rotationNumber].rotation, 0);
            previewObject.GetComponent<BoxCollider>().center = build[rotationNumber].colliderCenter;
        }

        else
        {
            if (previewObject.transform.position == position &&
               previewObject.transform.rotation == Quaternion.Euler(0, rotationControl[rotationNumber], 0)) return;

            previewObject.transform.position = position;
            previewObject.transform.rotation = Quaternion.Euler(0, rotationControl[rotationNumber], 0);
        }
    }
    public void DestroyPreviewObject()
    {
        if (previewObject == null) return;

        Destroy(previewObject);
    }
    private void PlaceBuilding(Vector3 position, Vector3 rotation)
    {
        GameObject placedObject = Instantiate(selectedItem.blockPrefab, position, Quaternion.Euler(rotation));
        placedObject.GetComponent<Block>().SetValues(selectedItem.nickName, selectedItem.durability);

        if (selectedItem.blockType == BlockTypes.SleepingBag) saveSpawnPoint.ChangePoint(position, rotation);

        selectedItem.DecreaseAmount(1);
        inventory.UpdateSelectedSlot();
        saveBuildings.AddBlock(placedObject);
        audioSource.PlayOneShot(placeSound);
    }
    public void RemoveBuilding(GameObject myObject)
    {
        Block block = myObject.GetComponent<Block>();
        if (block == null) return;

        saveBuildings.RemoveBlock(myObject);
        inventory.AddItem(block.nickName, 1, true);
        Destroy(myObject);
    }
}

public struct CustomBuilds
{
    public BlockTypes blockType;
    public List<CustomBuild> customBuild; 
}

public struct CustomBuild
{
    public Vector3 position;
    public float rotation;
    public Vector3 colliderCenter;
}
