using UnityEngine;

public class Mine : MonoBehaviour
{
    private SaveBuildings saveBuildings;

    [SerializeField] private GameObject dynamiteAnimationObject;
    private float explotionRadius = 10f;

    void Awake()
    {
        saveBuildings = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SaveBuildings>();
    }
    private void StartExplosion(GameObject player, float damage, float distance)
    {
        GameObject myObject = Instantiate(dynamiteAnimationObject, transform.position, Quaternion.identity);
        myObject.GetComponent<DynamiteAnimation>().PlayAnimation();

        if(player != null)
        {
            Vector3 randomForce = new Vector3(Random.Range(90,180), 120f - distance * 10, Random.Range(-90, -180));
            player.GetComponent<CharacterController>().Move(randomForce * Time.deltaTime);
            player.GetComponent<PlayerStats>().DecreaseHealth(damage, true);
        }    

        saveBuildings.RemoveBlock(gameObject);
        Destroy(gameObject);
    }

    public void ExplodeWithShoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            StartExplosion(null, 0, 0);
            return;
        }

        var distance = CheckPlayerDistance(player);
        if (distance <= explotionRadius)
        {
            StartExplosion(player, 100 - distance * 10, distance);
        }
        else
            StartExplosion(null, 0, 0);
    }
    private float CheckPlayerDistance(GameObject player)
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            StartExplosion(other.gameObject, 100, 0);
        }
        else if (other.transform.root.tag == "Zombie")
        {
            ExplodeWithShoot();
            other.transform.root.GetComponent<Zombie>().DecreaseHealth(null, 500);
        }
        else if (other.transform.root.tag == "Animal")
        {
            ExplodeWithShoot();
            other.transform.root.GetComponent<Animal>().DecreaseHealth(null, 500);
        }
    }
}
