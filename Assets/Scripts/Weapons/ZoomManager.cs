using UnityEngine;

public class ZoomManager : MonoBehaviour
{
    private Camera cam;
    private LockMovement lockMovement;
    private MouseMovement mouseMovement;

    private float originalFov;
    private int minFovMaxZoom = 10;
    private int maxFovMinZoom = 45;
    private int currentZoomValue;

    private int zoomSensitivity = 3;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        lockMovement = GetComponent<LockMovement>();
        mouseMovement = GetComponent<MouseMovement>();

        originalFov = cam.fieldOfView;
        currentZoomValue = maxFovMinZoom;
    }
    void Update()
    {
        if (lockMovement.zoomOn)
        {
            CheckInputs();
            StartZoom();
            return;
        }
        
        ResetZoom();
    }
    private void CheckInputs()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) IncreaseZoom();
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) DecreaseZoom();
    }
    private void StartZoom()
    {
        if (cam.fieldOfView != originalFov) return;

        cam.fieldOfView = currentZoomValue;
        mouseMovement.SetSensitivityForZoom(mouseMovement.originalSensitivity / originalFov * currentZoomValue);
    }
    private void IncreaseZoom()
    {
        currentZoomValue -= zoomSensitivity;

        if(currentZoomValue < minFovMaxZoom) currentZoomValue = minFovMaxZoom;

        cam.fieldOfView = currentZoomValue;
        mouseMovement.SetSensitivityForZoom(mouseMovement.originalSensitivity / originalFov * currentZoomValue);
    }
    private void DecreaseZoom()
    {
        currentZoomValue += zoomSensitivity;

        if (currentZoomValue > maxFovMinZoom) currentZoomValue = maxFovMinZoom;

        cam.fieldOfView = currentZoomValue;
        mouseMovement.SetSensitivityForZoom(mouseMovement.originalSensitivity / originalFov * currentZoomValue);
    }
    private void ResetZoom()
    {
        if (cam.fieldOfView == originalFov) return;

        cam.fieldOfView = originalFov;
        mouseMovement.ResetSensitivityAfterZoom();
    }
}
