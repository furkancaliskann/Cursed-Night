using System.Collections.Generic;
using UnityEngine;

public class WoodSpike : MonoBehaviour
{
    private Block block;
    public List<GameObject> list = new List<GameObject>();

    private AudioSource audioSource;
    [SerializeField] private AudioClip slashSound;

    private float counter = 0f;
    private float counterMax = 1.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        block = GetComponent<Block>();
    }
    void Update()
    {
        if (Time.timeScale == 0f) return;
        CheckCounter();
        ApplyTrapEffects();
    }
    private void CheckCounter()
    {
        if (counter > 0) counter -= Time.deltaTime;
    }
    private void DecreaseDurability()
    {
        block.DecreaseDurability(50);
        if (block.durability <= 0) ClearList();
    }
    private void ApplyTrapEffects()
    {
        if (list.Count == 0 || counter > 0) return;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }

            if (list[i].transform.root.CompareTag("Player"))
            {
                PlayerStats playerStats = list[i].transform.root.GetComponent<PlayerStats>();
                playerStats.DecreaseHealth(8, true);

                PlayerMovement playerMovement = list[i].transform.root.GetComponent<PlayerMovement>();
                playerMovement.SetTrapped(true);

                audioSource.PlayOneShot(slashSound);
                DecreaseDurability();
                counter = counterMax;
            }
            else if (list[i].transform.root.CompareTag("Zombie"))
            {
                Zombie zombie = list[i].transform.root.GetComponent<Zombie>();
                zombie.DecreaseHealth(null, 50);
                zombie.SetTrapped(true);

                audioSource.PlayOneShot(slashSound);
                DecreaseDurability();
                counter = counterMax;
            }
            else if (list[i].transform.root.CompareTag("Animal"))
            {
                Animal animal = list[i].transform.root.GetComponent<Animal>();
                animal.DecreaseHealth(null, 50);
                animal.SetTrapped(true);

                audioSource.PlayOneShot(slashSound);
                DecreaseDurability();
                counter = counterMax;
            }

        }
    }
    private void ExitTrigger(GameObject exitObject)
    {
        if (exitObject.transform.root.CompareTag("Player"))
        {
            PlayerMovement playerMovement = exitObject.transform.root.GetComponent<PlayerMovement>();
            playerMovement.SetTrapped(false);
        }
        else if (exitObject.transform.root.CompareTag("Zombie"))
        {
            Zombie zombie = exitObject.transform.root.GetComponent<Zombie>();
            zombie.SetTrapped(false);
        }
        else if (exitObject.transform.root.CompareTag("Animal"))
        {
            Animal animal = exitObject.transform.root.GetComponent<Animal>();
            animal.SetTrapped(false);
        }

        if (list.Contains(exitObject.transform.root.gameObject)) list.Remove(exitObject.transform.root.gameObject);
    }
    private void ClearList()
    {
        for (int i = 0; i < list.Count; i++)
        {
            ExitTrigger(list[i]);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(!list.Contains(other.transform.root.gameObject)) list.Add(other.transform.root.gameObject); 
    }
    private void OnTriggerExit(Collider other)
    {
        ExitTrigger(other.transform.root.gameObject);
    }
}
