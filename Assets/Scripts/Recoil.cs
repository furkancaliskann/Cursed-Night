using UnityEngine;

public class Recoil : MonoBehaviour
{
    private MouseMovement mouseMovement;

    /*private float remainedRotation;
    private float rotationCounter = 0.01f;
    private float rotationCounterMax = 0.01f;*/

    void Awake()
    {
        mouseMovement = GetComponent<MouseMovement>();
    }
    /*void Update()
    {
        CheckRotationCounter();
    }
    private void CheckRotationCounter()
    {
        if (rotationCounter > 0) rotationCounter -= Time.deltaTime;

        if (remainedRotation < 0 && rotationCounter <= 0)
        {
            DecreaseRecoil(0.1f);
            remainedRotation += 0.1f;
            rotationCounter = rotationCounterMax;
        }
    }*/
    public void IncreaseRecoil(float amount/*, float remainedRotation*/)
    {
        transform.Rotate(0, Random.Range(-0.5f, 0.5f), 0);
        mouseMovement.DecreaseXRotation(amount);
        //this.remainedRotation = remainedRotation;
    }
    /*private void DecreaseRecoil(float amount)
    {
        mouseMovement.IncreaseXRotation(amount);
    }*/
}
