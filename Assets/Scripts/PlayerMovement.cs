using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CharacterController characterController;
    private FogManager fogManager;
    private Footsteps footsteps;
    private HealthNotification healthNotification;
    private LockMovement lockMovement;
    private PlayerStats playerStats;  

    private float walkSpeed = 4f;
    private float crouchedWalkSpeed = 2f;
    private float inWaterWalkSpeed = 1.75f;
    private float trappedWalkSpeed = 1.5f;

    private float runSpeed = 6.5f;
    private float runSpeedBase = 6.5f;
    private float inWaterRunSpeed = 2.5f;
    private float trappedRunSpeed = 2f;

    private float velocity = -15f;
    private float jumpHeight = 1.5f;

    public bool isWalking { get; private set; }
    public bool isRunning { get; private set; }
    public bool isGrounded { get; private set; }
    public bool isCrouched { get; private set; }
    public bool inWater { get; private set; }
    public bool onLadder { get; private set; }

    private GameObject waterObject;
    public MaterialType groundObjectMaterialType;
    [SerializeField] private GameObject groundObjectCheck;
    public Vector3 originalCameraLocalPosition {  get; private set; }
    private Vector3 fallVector;

    public bool isTrapped { get; private set; }

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        fogManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FogManager>();
        footsteps = GetComponent<Footsteps>();
        healthNotification = GetComponent<HealthNotification>();
        lockMovement = GetComponent<LockMovement>();
        playerStats = GetComponent<PlayerStats>();

        originalCameraLocalPosition = cam.transform.localPosition;

        SetFogCamera();
    }
    void Update()
    {
        if (lockMovement.playerInCar)
        {
            isWalking = isRunning = false;
            if (isCrouched) ChangeCrouch();
            return;
        }

        CheckGround();
        CalculateHeight();
        CheckMoveInputs();
        CheckCrouchInputs();
        CheckJumpInput();
        CheckSwimInput();
    }
    private void SetFogCamera()
    {
        fogManager.SetFogCamera(cam);
    }
    private void CheckGround()
    {
        isGrounded = characterController.isGrounded;
        if (Physics.Raycast(groundObjectCheck.transform.position, groundObjectCheck.transform.forward, out RaycastHit hit, 0.5f))
        {
            if(hit.transform.tag == "Terrain")
            {
                groundObjectMaterialType = MaterialType.Empty;
                return;
            }
            else
            {
                FootstepMaterial footstepMaterial = hit.transform.root.GetComponent<FootstepMaterial>();
                if(footstepMaterial == null)
                {
                    groundObjectMaterialType = MaterialType.Empty;
                    return;
                }
                else
                {
                    groundObjectMaterialType = footstepMaterial.type;
                }
            }
        }
        else
        {
            groundObjectMaterialType = MaterialType.Empty;
            return;
        }
    }
    private void CalculateHeight()
    {
        if (isGrounded || onLadder)
        {
            if (fallVector.y <= -15 && !onLadder)
            {
                playerStats.FallDamage(-fallVector.y - 15);
            }

            fallVector.y = -2f;
        }
    }
    private void CheckMoveInputs()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move != Vector3.zero && !lockMovement.locked)
        {
            if (Input.GetKey(KeyCode.LeftShift) && playerStats.energy > 0 &&
                !healthNotification.Check(HealthEffects.Broken) && playerStats.canRun && !isCrouched && !onLadder)
            {
                if(inWater)
                {
                    characterController.Move(move * inWaterRunSpeed * Time.deltaTime);
                }
                else if (isTrapped)
                {
                    characterController.Move(move * trappedRunSpeed * Time.deltaTime);
                }
                else
                {
                    characterController.Move(move * runSpeed * Time.deltaTime);
                    footsteps.PlayFootstepSounds(true);
                }
                
                isRunning = true;
                isWalking = false;  
                playerStats.DecreaseEnergyOverTime();
            }
            else
            {
                if (isCrouched)
                    characterController.Move(move * crouchedWalkSpeed * Time.deltaTime);
                else if (inWater)
                    characterController.Move(move * inWaterWalkSpeed * Time.deltaTime);
                else if (isTrapped)
                    characterController.Move(move * trappedWalkSpeed * Time.deltaTime);
                else if (onLadder)
                {
                    if(z > 0)
                    {
                        if (cam.transform.localRotation.x >= 0)
                        {
                            if(isGrounded)
                            {
                                characterController.Move(transform.forward * 2.5f * Time.deltaTime);
                                onLadder = false;
                            }
                            else
                                characterController.Move(-transform.up * 2.5f * Time.deltaTime);
                        }   
                        else if (cam.transform.localRotation.x < 0)
                            characterController.Move(transform.up * 2.5f * Time.deltaTime);
                    }
                    else if (x != 0)
                    {
                        characterController.Move(transform.right * x * walkSpeed / 2 * Time.deltaTime);
                    }
                } 
                else 
                    characterController.Move(move * walkSpeed * Time.deltaTime);

                isWalking = true;
                isRunning = false;
                if ((move.x > 0.75f || move.z > 0.75f || move.x < -0.75f || move.z < -0.75f) && !isCrouched && !inWater && !onLadder)
                    footsteps.PlayFootstepSounds(false);
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;
        }
    }
    private void CheckCrouchInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !inWater && !onLadder)
            ChangeCrouch();

    }
    private void CheckJumpInput()
    {
        if (isTrapped) return;
        if(onLadder && Input.GetButtonDown("Jump"))
        {
            fallVector.y = Mathf.Sqrt(jumpHeight * -2f * velocity);
            playerStats.DecreaseEnergy(2.5f);
            fallVector.y += velocity * Time.deltaTime;
            characterController.Move(fallVector * Time.deltaTime);
            return;
        }
        else if (Input.GetButtonDown("Jump") && isGrounded && !lockMovement.locked && !inWater && !onLadder)
        {
            footsteps.PlayJumpSound();
            fallVector.y = Mathf.Sqrt(jumpHeight * -2f * velocity);
            playerStats.DecreaseEnergy(2.5f);
        }
        else if (inWater)
        {
            if (Input.GetButton("Jump"))
            {
                fallVector.y = 0f;
                return;
            }
            else if(Input.GetKey(KeyCode.LeftControl))
            {
                fallVector.y = -2f;
            }
            else
                fallVector.y = -0.5f;
        }
        if (onLadder)
        {
            fallVector.y = 0f;
            return;
        }

        fallVector.y += velocity * Time.deltaTime;
        characterController.Move(fallVector * Time.deltaTime);
    }
    private void CheckSwimInput()
    {
        if (Input.GetButton("Jump") && !lockMovement.locked && inWater &&
            transform.position.y + cam.transform.localPosition.y <= waterObject.transform.position.y + 0.5f)
        {
            characterController.Move(Vector3.up * 1.2f * Time.deltaTime);
        }
    }
    private void ChangeCrouch()
    {
        if (lockMovement.locked) return;

        if (!isCrouched)
        {
            characterController.height /= 2f;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y / 2f,
                cam.transform.localPosition.z);
        }
            
        else
        {
            characterController.height *= 2f;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y * 2f,
                cam.transform.localPosition.z);
        }        

        isCrouched = !isCrouched;
    }
    public void SetTrapped(bool value)
    {
        if (isTrapped == value) return;
        isTrapped = value;
    }
    public void ChangeRunSpeed(float value)
    {
        runSpeed = value;
    }
    public void ResetRunSpeed()
    {
        runSpeed = runSpeedBase;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Building")
        {
            if (onLadder) return;
            Block block = other.gameObject.GetComponent<Block>();

            if (block != null && block.blockType == BlockTypes.Ladder)
            {
                onLadder = true;
            }
        }

        else if (other.tag == "Water")
        {
            if (waterObject == null)
            {
                waterObject = other.transform.root.gameObject;
            }
                

            inWater = true;

            if (isCrouched) ChangeCrouch();

            if(other.transform.root.position.y >= transform.position.y + cam.transform.localPosition.y)
            {
                fogManager.SetWaterFogState(true);
                footsteps.PlayUnderWaterSound();
            }   
            else
            {
                fogManager.SetWaterFogState(false);
                footsteps.PlaySwimmingSound();
            }
                
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Building")
        {
            Block block = other.gameObject.GetComponent<Block>();

            if (block != null && block.blockType == BlockTypes.Ladder)
            {
                onLadder = false;
            }
        }
        else if (other.tag == "Water")
        {
            waterObject = null;
            inWater = false;
            fogManager.SetWaterFogState(false);
            footsteps.StopWaterAudioSource();
        }
    }
}
