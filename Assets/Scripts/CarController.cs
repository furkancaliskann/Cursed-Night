using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController : MonoBehaviour
{
    private AudioSource audioSource;
    private RadioManager radioManager;
    private Rigidbody carRigidbody;
    private Translations translations;

    public float currentSpeed {  get; private set; }
    public float fuel { get; private set; }
    public int durability { get; private set; }

    private float maxSpeed = 30f;
    private float maxAcceleration = 30f;
    private float brakeAcceleration = 75f;
    private float brakeInputZero = 10f;

    private float turnSensitivity = 0.5f;
    private float maxSteerAngle = 30f;

    private float moveInput;
    private float steerInput;

    public bool carStarted;
    public bool carStoped;

    public bool radioOpen;
    public int selectedRadioChannel;

    private float soundCounter = 0.4f;
    private float soundCounterMax = 0.4f;

    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip carStartSound;
    [SerializeField] private AudioClip carStopSound;
    [SerializeField] private AudioClip fillFuelSound;
    [SerializeField] private AudioSource radioAudioSource;

    private Vector3 centerOfMass = Vector3.zero;
    public List<Wheel> wheels;

    private GameObject player;
    public Transform landingPoint;
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        radioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RadioManager>();
        translations = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Translations>();

        fuel = 100f;
        durability = 5000;
        carStoped = true;
    }
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = centerOfMass;
    }
    void Update()
    {
        CalculateSpeed();
        SetPlayerPosition();

        if (!CheckCarStarted()) return;

        GetInputs();
        AnimateWheels();
        CheckCounter();
        CheckRadioInput();
        CheckRadio();
    }
    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }
    private void GetInputs()
    {
        steerInput = Input.GetAxis("Horizontal");

        if (fuel <= 0) return;
        moveInput = Input.GetAxis("Vertical");
    }
    private void Move()
    {          
        if(currentSpeed >= maxSpeed) moveInput = 0;
        if (moveInput != 0 && fuel > 0)
        {
            if(currentSpeed < 29f)
            {
                //0.075f fena deðil
                fuel -= Time.deltaTime * 0.1f;
            }
            else
            {
                //0.4f fena deðil
                fuel -= Time.deltaTime * 0.5f;
            }
        }
            
        if (fuel < 0) fuel = 0;

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
        }
    }
    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    private void Brake()
    {
        if (!carStarted && currentSpeed > 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 1200 * brakeAcceleration * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else if (moveInput == 0)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeInputZero * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
    private void CalculateSpeed()
    {
        currentSpeed = carRigidbody.velocity.magnitude;
    }
    private bool CheckCarStarted()
    {
        if (!carStarted)
        {
            moveInput = 0;

            if (!carStoped)
            {
                audioSource.Stop();
                audioSource.pitch = 1f;
                audioSource.PlayOneShot(carStopSound);
                carStoped = true;
            }
            return false;
        }

        carStoped = false;
        return true;
    }
    private void CheckCounter()
    {
        if (soundCounter > 0) soundCounter -= Time.deltaTime;
        if (soundCounter <= 0)
        {
            audioSource.pitch = 0.35f + (0.015f * currentSpeed);
            audioSource.PlayOneShot(engineSound);
            soundCounter = soundCounterMax;
        }
    }
    private void CheckRadioInput()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            selectedRadioChannel++;
            if(selectedRadioChannel >= radioManager.radioChannels.Count)
            {
                selectedRadioChannel = -1;
                radioOpen = false;
            }
            else
            {
                StopRadio();
                radioOpen = true;
            }
        }
    }
    private void CheckRadio()
    {
        if (radioOpen && !radioAudioSource.isPlaying && Time.timeScale != 0) PlayRadio();
        else if (!radioOpen) StopRadio();
    }
    private void PlayRadio()
    {
        radioAudioSource.Stop();
        if (selectedRadioChannel == -1) return;
        radioAudioSource.clip = radioManager.radioChannels[selectedRadioChannel].selectedClip;
        radioAudioSource.time = radioManager.radioChannels[selectedRadioChannel].currentTime;
        player.GetComponent<CarCanvas>().OpenRadioPanel(radioManager.radioChannels[selectedRadioChannel].channelName);
        radioAudioSource.Play();
    }
    private void StopRadio()
    {
        if (!radioAudioSource.isPlaying) return;
        radioAudioSource.Stop();

        if(player != null)
        player.GetComponent<CarCanvas>().OpenRadioPanel(translations.Get("RadioTurnedOff"));
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    private void SetPlayerPosition()
    {
        if (player == null) return;

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
    }
    public void SetCarStartedDelay(bool value)
    {
        audioSource.PlayOneShot(carStartSound);

        if (value)
        {
            StartCoroutine(ChangeCarStartedNow(value));
        }
            
        else
        {
            carStarted = false;
            StopRadio();
        }
            
    }
    IEnumerator ChangeCarStartedNow(bool value)
    {
        yield return new WaitForSeconds(0.7f);
        carStarted = value;
        soundCounter = 0;
        radioOpen = true;
        PlayRadio();
    }
    public void DecreaseDurability(int value)
    {
        durability -= value;
    }
    public void IncreaseDurability(int value)
    {
        durability += value;
    }
    public int CalculateRequiredFuel()
    {
        int percent = 100 - Mathf.FloorToInt(fuel);
        return percent * 10;
    }
    public void FillFuel(float amount)
    {
        radioAudioSource.PlayOneShot(fillFuelSound);
        if (fuel + amount > 100) fuel = 100;
        else
            fuel += amount;
    }
    public void LoadCar(float fuel, int durability, bool carStarted, bool carStoped, int selectedRadioChannel)
    {
        this.carStarted = carStarted;
        this.fuel = fuel;
        this.durability = durability;
        this.carStoped = carStoped;
        this.selectedRadioChannel = selectedRadioChannel;

        if (carStarted)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.GetComponent<Player>().DriveCar(gameObject);
        }
    }
}

public enum Axel
{
    Front,
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject wheelModel;
    public WheelCollider wheelCollider;
    public Axel axel;
}