using UnityEngine;

public class Minimap : MonoBehaviour
{
    private LockMovement lockMovement;

    [SerializeField] private GameObject minimapObject;
    private bool minimapOn = true;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
    }
    void Update()
    {
        if (lockMovement.zoomOn)
        {
            CloseForZoom();
            return;
        }

        if (minimapOn) Open();
        else Close();
    }
    public void Open()
    {
        if (minimapObject.activeInHierarchy) return;

        minimapOn = true;
        minimapObject.SetActive(true);
    }
    public void Close()
    {
        if (!minimapObject.activeInHierarchy) return;

        minimapOn = false;
        minimapObject.SetActive(false);
    }
    public void CloseForZoom()
    {
        minimapObject.SetActive(false);
    }
}
