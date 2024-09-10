using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    private LockMovement lockMovement;

    private float sensitivity;
    public float originalSensitivity {  get; private set; }
    private Camera cam;
    public float xRotation {  get; private set; }

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
        cam = GetComponentInChildren<Camera>();
    }
    void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2.0f);
        originalSensitivity = sensitivity;
    }
    void Update()
    {
        if (lockMovement.locked) return;
        if (lockMovement.playerInCar)
        {
            xRotation = 0;
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89.9f, 89.9f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
    public void ChangeSensitivityWithConsole(float newSensitivity)
    {
        if (newSensitivity < 0.1f || newSensitivity > 10) return;

        PlayerPrefs.SetFloat("Sensitivity", newSensitivity);
        sensitivity = newSensitivity;

    }
    public void IncreaseXRotation(float amount)
    {
        xRotation += amount;
    }
    public void DecreaseXRotation(float amount)
    {
        xRotation -= amount;
    }
    public void SetXRotation(float value)
    {
        xRotation = value;
    }
    public void SetSensitivityForZoom(float value)
    {
        sensitivity = value;
    }
    public void ResetSensitivityAfterZoom()
    {
        sensitivity = originalSensitivity;
    }
}
