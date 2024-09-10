using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockMovement : MonoBehaviour
{
    public bool locked { get; private set; }
    public bool playerBusy { get; private set; }
    public bool playerInCar { get; private set; }
    public bool inventoryLocked { get; private set; }
    public bool zoomOn { get; private set; }

    void Start()
    {
        Unlock();
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        locked = true;
    }
    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        locked = false;
    }
    public void LockInventory()
    {
        inventoryLocked = true;
    }
    public void UnlockInventory()
    {
        inventoryLocked = false;
    }
    public void KeepBusy(float busyTime)
    {
        if (playerBusy) return;

        playerBusy = true;
        Invoke(nameof(Idle), busyTime);
    }
    private void Idle()
    {
        playerBusy = false;
    }
    public void SetPlayerInCar(bool value)
    {
        playerInCar = value;
    }
    public void SetZoomValue(bool value)
    {
        zoomOn = value;
    }
}
