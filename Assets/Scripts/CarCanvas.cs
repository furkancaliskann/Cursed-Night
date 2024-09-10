using UnityEngine;
using UnityEngine.UI;

public class CarCanvas : MonoBehaviour
{
    private CarController carController;
    private Translations translations;

    [SerializeField] private Text speedText;
    [SerializeField] private Text fuelText;
    [SerializeField] private Text durabilityText;

    [SerializeField] private GameObject radioPanel;
    [SerializeField] private Text radioText;

    void Awake()
    {
        translations = GetComponent<Translations>();
    }
    void Update()
    {
        UpdateCanvas();
    }
    private void UpdateCanvas()
    {
        if (carController == null) return;

        float speed = carController.currentSpeed;
        float fuel = carController.fuel;
        float durability = carController.durability;    

        speedText.text = translations.Get("CarSpeed") + Mathf.RoundToInt(speed).ToString() + " km/h";
        fuelText.text = translations.Get("CarFuel") + Mathf.RoundToInt(fuel).ToString("F0") + "%";
        durabilityText.text = translations.Get("CarDurability") + durability.ToString();
    }
    public void SetCarController(CarController carController)
    {
        this.carController = carController;
    }
    public void OpenRadioPanel(string channelName)
    {
        if(IsInvoking(nameof(CloseRadioPanel)))
            CancelInvoke(nameof(CloseRadioPanel));

        radioText.text = channelName;
        radioPanel.SetActive(true);

        Invoke(nameof(CloseRadioPanel), 2f);
    }
    private void CloseRadioPanel()
    {
        radioPanel.SetActive(false);
    }
}
