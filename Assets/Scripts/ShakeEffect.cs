using TMPro;
using UnityEngine;
public class ShakeEffect : MonoBehaviour
{
    private LockMovement lockMovement;
    private PlayerMovement playerMovement;

    // For Mouse Movement
    private float sensitivity = 0.1f;
    private float slidingSpeed = 1.2f;
    private float returnSpeed = 4f;
    private float maxLeftRotation = -0.05f;
    private float maxRightRotation = 0.05f;

    private Vector3 originalPosition;
    private GameObject item;

    // For Input Movement
    private float swayAmountWalk = 0.03f;
    private float swayAmountRun = 0.12f;
    private float smoothFactor = 3f;

    // For Select Movement
    private float selectAnimationTime = 0.3f;
    private float selectAnimationTimeMax = 0.3f;
    private float selectSpeed = 5f;

    void Awake()
    {
        lockMovement = GetComponent<LockMovement>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (lockMovement.playerInCar) return;

        CheckInputs();
        CheckMousePosition();
        CheckSelectAnimationTime();
    }
    private void CheckInputs()
    {
        if (item == null || lockMovement.locked || lockMovement.playerBusy) return;
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 ||
            Input.GetMouseButton(0) || Input.GetMouseButton(1) || selectAnimationTime > 0) return;

        float swayValue;

        if (!playerMovement.isRunning) swayValue = swayAmountWalk;
        else swayValue = swayAmountRun;

        float moveX = Input.GetAxis("Horizontal") * (swayValue / 2);
        float moveY = Input.GetAxis("Vertical") * swayValue;

        if (moveY < -0.05f) moveY = -0.05f;

        Vector3 targetPosition = new Vector3(-moveX, -moveY, 0) + originalPosition;
        item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, targetPosition, Time.deltaTime * smoothFactor);
    }
    private void CheckMousePosition()
    {
        if (item == null || lockMovement.locked || lockMovement.playerBusy || selectAnimationTime > 0) return;

        float mouseXMovement = Input.GetAxis("Mouse X") * sensitivity;

        Vector3 newPosition = item.transform.localPosition + new Vector3(mouseXMovement, 0f, 0f) * slidingSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, originalPosition.x + maxLeftRotation, originalPosition.x + maxRightRotation);

        item.transform.localPosition = newPosition;

        if (Mathf.Abs(mouseXMovement) < 0.01f)
        {
            item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, originalPosition, returnSpeed * Time.deltaTime);
        }
    }  
    private void CheckSelectAnimationTime()
    {
        if (selectAnimationTime < 0) return;

        selectAnimationTime -= Time.deltaTime;

        if(item != null)
        item.transform.localPosition = Vector3.Lerp(item.transform.localPosition, originalPosition, selectSpeed * Time.deltaTime);

    }
    public void SetSelectedItem(GameObject item)
    {
        if(this.item != null)
            this.item.transform.localPosition = originalPosition;

        this.item = item;
        OriginalPosition code = item.GetComponent<OriginalPosition>();
        originalPosition = code.originalPosition;
        item.transform.localRotation = code.originalRotation;

        selectAnimationTime = selectAnimationTimeMax;
        item.transform.localPosition = originalPosition - new Vector3(0.15f, 0.2f, 0);
    }

}