using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeCanvas : MonoBehaviour
{
    private LockMovement lockMovement;
    private TimeController timeController;
    private Translations translations;

    [SerializeField] private Text timeText;
    private DateTime zeroDateTime = new DateTime(0001, 1, 1, 0, 0, 0);

    [SerializeField] private GameObject parentObject;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
        timeController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeController>();
        translations = GetComponent<Translations>();
    }
    void Update()
    {
        if(lockMovement.zoomOn)
        {
            ActivateTimePanel(false);
            return;
        }

        ActivateTimePanel(true);
        UpdateText();
    }
    private void UpdateText()
    {
        if (timeText != null && timeController != null)
        {
            TimeSpan difference = timeController.GetTime() - zeroDateTime;

            timeText.text = translations.Get("Day") + (difference.Days + 1) + "  " + difference.Hours.ToString("00") + ":" + difference.Minutes.ToString("00");
        }
    }

    private void ActivateTimePanel(bool value)
    {
        if (value)
        {
            if (parentObject.activeInHierarchy) return;
            parentObject.SetActive(true);
        }
        else
        {
            if (!parentObject.activeInHierarchy) return;
            parentObject.SetActive(false);
        }
    }
}