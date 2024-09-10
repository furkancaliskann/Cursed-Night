using UnityEngine;

public class Map : MonoBehaviour
{
    private LockMovement lockMovement;
    [SerializeField] private GameObject panel;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
    }
    void Update()
    {
        CheckInputs();
    }
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!panel.activeInHierarchy)
                Open();
            else
                Close();
        }
    }
    private void Open()
    {
        if (lockMovement.locked) return;

        panel.SetActive(true);
        lockMovement.Lock();
    }
    private void Close()
    {
        if (!lockMovement.locked) return;
        panel.SetActive(false);
        lockMovement.Unlock();
    }
}
