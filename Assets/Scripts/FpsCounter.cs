using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    private LockMovement lockMovement;

    private float deltaTime = 0.0f;
    [SerializeField] private GameObject fpsPanel;
    [SerializeField] private Text fpsText;
    public List<string> closeReasons = new List<string>();
    public bool needOpen;
    public int lastFpsValue {  get; private set; }


    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
    }
    void Start()
    {
        RefreshPanel();
        InvokeRepeating(nameof(ShowFps), 0f, 0.5f);
    }
    void Update()
    {
        if (lockMovement.zoomOn)
            AddReason("zoom");
        else
            RemoveReason("zoom");

        CheckPanel();
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }
    public void RefreshPanel()
    {
        int value = PlayerPrefs.GetInt("FpsCounter", 0);

        if (value == 1) needOpen = true;
        else needOpen = false;
    }
    public void CheckPanel()
    {
        if(closeReasons.Count == 0 && !fpsPanel.activeInHierarchy && needOpen)
        {
            fpsPanel.SetActive(true);
        }
        else if ((fpsPanel.activeInHierarchy && closeReasons.Count > 0) || !needOpen)
        {
            fpsPanel.SetActive(false);
        }
    }
    private void ShowFps()
    {
        if (closeReasons.Count != 0) return;

        lastFpsValue = Mathf.RoundToInt(1.0f / deltaTime);

        if (lastFpsValue < 0)
        {
            lastFpsValue = 0;
            fpsText.color = Color.red;
        }
        else if (lastFpsValue >= 0 && lastFpsValue < 30) fpsText.color = Color.red;
        else if (lastFpsValue >= 30 && lastFpsValue < 60) fpsText.color = Color.yellow;
        else fpsText.color = Color.green;

        fpsText.text = lastFpsValue.ToString() + " FPS";
    }
    public void ChangePanelSetting(int value)
    {
        if (value == 0)
        {
            PlayerPrefs.SetInt("FpsCounter", 0);
            needOpen = false;
        }
        else if (value == 1)
        {
            PlayerPrefs.SetInt("FpsCounter", 1);
            needOpen = true;
        }
    }
    public void AddReason(string reason)
    {
        if (closeReasons.Contains(reason)) return;
        closeReasons.Add(reason);
    }
    public void RemoveReason(string reason)
    {
        if (!closeReasons.Contains(reason)) return;
        closeReasons.Remove(reason);
    }
}
