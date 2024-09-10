using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    private Inventory inventory;
    private LockMovement lockMovement;

    [SerializeField] private GameObject panel;
    [SerializeField] private Text text;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        lockMovement = GetComponent<LockMovement>();
    }
    private void OpenPanel()
    {
        if (panel.activeInHierarchy) return;
        panel.SetActive(true);
    }
    public void ClosePanel()
    {
        if (!panel.activeInHierarchy) return;
        panel.SetActive(false);
    }
    public void UpdateText(int ammoInside, string ammoTypeNick)
    {
        if(lockMovement.zoomOn)
        {
            ClosePanel();
            return;
        }

        OpenPanel();
        text.text = ammoInside + " / " + inventory.CalculateItemCount(ammoTypeNick); 
    }
}
