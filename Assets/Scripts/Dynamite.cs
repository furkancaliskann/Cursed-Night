using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    RaycastHit hit;
    public GameObject throwedStructure;
    public GameObject animationObject;
    public ParticleSystem particle;
    public AudioSource audioSource;

    public List<GameObject> whichPlayersTakingDamage = new List<GameObject>();


    void Start()
    {
        PlayWickEffect();

        Invoke(nameof(CreateAnimationObject), 5f);
    }

    void CreateAnimationObject()
    {
        GameObject myObject = Instantiate(animationObject, transform.position, Quaternion.identity);

        myObject.GetComponent<DynamiteAnimation>().PlayAnimation();
        GetComponent<Collider>().enabled = true;

        Invoke(nameof(GiveDamage), 0.1f);

        if (throwedStructure != null)
            throwedStructure.GetComponent<Block>().DecreaseDurability(200);
    }

    void GiveDamage()
    {
        foreach(GameObject player in whichPlayersTakingDamage)
        {
            player.layer = 0;

            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 8f))
            {
                if (hit.transform.tag == "Player")
                {
                    float distance = Vector3.Distance(player.transform.position, transform.position);

                    player.GetComponent<PlayerStats>().DecreaseHealth(80 - distance * 5, true);
                }
            }

            player.layer = 2;
        }

        Destroy(gameObject);
    }

    void PlayWickEffect()
    {
        particle.Emit(1);
        audioSource.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(!whichPlayersTakingDamage.Contains(other.gameObject))
                whichPlayersTakingDamage.Add(other.gameObject);
        }
    }
}
