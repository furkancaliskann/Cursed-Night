using UnityEngine;

public class Airdrop : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private GameObject parachute; 
    private float velocityTimer = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        DecreaseVelocityTimer();
        CheckParachuteOnGround();      
    }
    private void DecreaseVelocityTimer()
    {
        if (velocityTimer > 0) velocityTimer -= Time.deltaTime;
    }
    private void CheckParachuteOnGround()
    {
        if (rb.velocity.y >= 0 && velocityTimer <= 0 && parachute.activeInHierarchy)
        {
            parachute.SetActive(false);
        }
    }
}
