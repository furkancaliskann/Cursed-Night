using UnityEngine;

public class CameraCarFollow : MonoBehaviour
{
    private LockMovement lockMovement;
    private Transform carTarget;

    private float horizontalSpeed = 1.5f;
    private float verticalSpeed = 1.5f;
    private float followDistance = 5.5f;
    private float height = 2.0f;

    public float horizontalMove = 0f;
    public float verticalMove = 0f;

    void Awake()
    {
        lockMovement = GetComponentInParent<LockMovement>();
    }
    void Update()
    {
        if (lockMovement.locked) return;

        float moveX = Input.GetAxis("Mouse X");
        float moveY = Input.GetAxis("Mouse Y");

        horizontalMove += moveX * horizontalSpeed;
        verticalMove -= moveY * verticalSpeed;

        horizontalMove %= 360;
        verticalMove = Mathf.Clamp(verticalMove, -80, 80);

        transform.rotation = Quaternion.Euler(verticalMove, horizontalMove, 0);
        transform.position = carTarget.position - transform.forward * followDistance + Vector3.up * height;
    }
    public void SetCarTarget(Transform carTarget)
    {
        this.carTarget = carTarget;
        if (carTarget == null) return;

        horizontalMove = carTarget.transform.rotation.eulerAngles.y;
        verticalMove = 10f;
    }
}
