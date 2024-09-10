using UnityEngine;

public class FogManager : MonoBehaviour
{
    private Camera cam;

    private Color originalBackgroundColor = new Color(0.19f, 0.23f, 0.47f, 0.0f);

    private Color inWaterBackgroundColor = new Color(0.19f, 0.58f, 0.73f, 1.0f);
    private Color inWaterFogColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);

    private Color eventBackgroundColor = new Color(0.93f, 0.57f, 0.55f, 1.0f);
    private Color eventFogColor = new Color(0.89f, 0.54f, 0.54f, 0.50f);

    private bool inWaterFog;
    private bool eventFog;

    void Awake()
    {
        RenderSettings.fogMode = FogMode.Exponential;
    }
    void Update()
    {
        CheckFogState();
        CheckEventFogLerp();
    }
    private void CheckFogState()
    {
        if (eventFog && inWaterFog)
        {
            ActivateWaterAndEventFog();
        }
        else if (eventFog)
        {
            ActivateEventFog();
        }
        else if (inWaterFog)
        {
            ActivateWaterFog();
        }
        else
            DeactivateAll();
    }
    private void CheckEventFogLerp()
    {
        if (!eventFog || RenderSettings.fogDensity == 0.04f) return;

        if (RenderSettings.fogDensity < 0.04f)
        {
            RenderSettings.fogDensity += 0.0015f * Time.deltaTime;
        }
        else
            RenderSettings.fogDensity = 0.04f;
    }
    private void ActivateWaterFog()
    {
        if (!inWaterFog || cam == null) return;
        if (cam.backgroundColor == inWaterBackgroundColor) return;

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = inWaterBackgroundColor;

        RenderSettings.fogColor = inWaterFogColor;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogStartDistance = 0.0f;
        RenderSettings.fogEndDistance = 20f;
        RenderSettings.fog = true;
    }
    private void ActivateEventFog()
    {
        if (!eventFog || cam == null) return;
        if (cam.backgroundColor == eventBackgroundColor) return;

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = eventBackgroundColor;

        RenderSettings.fogColor = eventFogColor;
        RenderSettings.fogStartDistance = 0f;
        RenderSettings.fogEndDistance = 20f;
        RenderSettings.fog = true;
    }
    private void ActivateWaterAndEventFog()
    {
        if (!eventFog || !inWaterFog || cam == null) return;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = inWaterBackgroundColor;

        RenderSettings.fogColor = eventFogColor;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogStartDistance = 0.0f;
        RenderSettings.fogEndDistance = 20f;
        RenderSettings.fog = true;
    }
    private void DeactivateAll()
    {
        if (cam == null || cam.backgroundColor == originalBackgroundColor) return;

        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = originalBackgroundColor;
        RenderSettings.fogDensity = 0;
        RenderSettings.fog = false;

        inWaterFog = false;
        eventFog = false;
    }
    public void SetWaterFogState(bool value)
    {
        if (inWaterFog == value) return;

        inWaterFog = value;
    }
    public void SetEventFogState(bool value)
    {
        if (eventFog == value) return;

        eventFog = value;
    }
    public void LoadEventFogFast()
    {
        RenderSettings.fogDensity = 0.04f;
    }
    public void SetFogCamera(Camera cam)
    {
        this.cam = cam;
    }
}
