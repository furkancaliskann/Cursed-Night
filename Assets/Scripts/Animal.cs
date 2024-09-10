using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    private LootManager lootManager;
    private NavMeshAgent agent;
    private GameObject player;
    private Vector3 targetPoint;
    [SerializeField] private AnimalType animalType;

    private float forgetTime = 7.5f;
    private float forgetTimeMax = 7.5f;

    private Animation anim;
    private AudioSource audioSource;

    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip runAnimation;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip dieAnimation;

    [SerializeField] private AudioClip walkAudio;
    [SerializeField] private AudioClip runAudio1, runAudio2;
    [SerializeField] private AudioClip getDamageAudio;
    [SerializeField] private AudioClip dieAudio;

    private bool running;
    private bool walking;

    private Vector3 walkingPosition;
    private float walkingRefreshTime = 10f;
    private float walkingRefreshTimeMax = 10f;

    private float idleTimeMin = 2f;
    private float idleTimeMax = 8f;
    private float idleTime;

    private float destroyComponentTime = 1f;

    private float health = 200f;

    private bool isTrapped;
    private float originalSpeed;

    void Awake()
    {
        lootManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LootManager>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

        originalSpeed = agent.speed;
    }
    void Update()
    {
        CheckHealth();
        if (health <= 0) return;
        CheckAnimation();
        CheckAudio();
        CheckWalkingRefreshTime();
        CheckIdleTime();
        SearchPlayer();
        WalkToTarget();
    }
    private void CheckHealth()
    {
        if (health > 0 || agent == null) return;

        agent.destination = transform.position;
        agent = null;
        player = null;
        targetPoint = Vector3.zero;
        walking = running = false;
        PlayAnimation(true, dieAnimation);
        audioSource.PlayOneShot(dieAudio);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Invoke(nameof(DestroyAllComponents), destroyComponentTime);
    }
    private void CheckAnimation()
    {
        if (walking)
        {
            PlayAnimation(false, walkAnimation);
        }
        else if (running)
        {
            PlayAnimation(false, runAnimation);
        }
    }
    private void CheckAudio()
    {
        if (walking) PlayWalkingSound();
        else if (running) PlayRunningSound();        
    }
    private void CheckWalkingRefreshTime()
    {
        if (!walking || health <= 0 || running) return;

        if (walkingRefreshTime > 0) walkingRefreshTime -= Time.deltaTime;

        if (walkingRefreshTime <= 0)
        {
            ResetWalkingVariables();
        }
        else if (Vector3.Distance(walkingPosition, transform.position) <= 0.5f)
        {
            ResetWalkingVariables();
        }
    }
    private void CheckIdleTime()
    {
        if (walking || running || health <= 0) return;

        idleTime -= Time.deltaTime;

        PlayAnimation(false, idleAnimation);

        if (idleTime <= 0)
        {
            WalkRandom();
            idleTime = Random.Range(idleTimeMin, idleTimeMax);
        }
    }
    private void WalkRandom()
    {
        if (walking || running || health <= 0 || idleTime > 0) return;
        
        walking = true;

        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        Terrain terrain = Terrain.activeTerrain;

        Vector3 scale = terrain.terrainData.heightmapScale;

        float firstY = terrain.terrainData.GetHeight(Mathf.RoundToInt(transform.position.x + x / scale.x), Mathf.RoundToInt(transform.position.z / scale.z));
        float y = terrain.SampleHeight(new Vector3(transform.position.x + x, firstY, transform.position.z + z));

        walkingPosition = new Vector3(transform.position.x + x, y, transform.position.z + z);

        agent.isStopped = false;
        agent.SetDestination(walkingPosition);
    }
    private void SearchPlayer()
    {
        if (health <= 0) return;
        if(player == null)
        {
            GameObject result = GameObject.FindGameObjectWithTag("Player");
            if (result == null) return;

            if (CalculateDistance(result.transform.position) <= 17.5f)
            {
                targetPoint = (transform.position - result.transform.position).normalized; ;
                player = result;
            }
        }
        else
        {
            targetPoint = (transform.position - player.transform.position).normalized; ;
        }
    }
    private void WalkToTarget()
    {
        if (agent == null || player == null || health <= 0) return;

        float distance = CalculateDistance(player.transform.position);

        if (distance <= 20) forgetTime = forgetTimeMax;

        else if (forgetTime > 0) forgetTime -= Time.deltaTime;

        if (forgetTime <= 0)
        {
            agent.isStopped = true;
            forgetTime = forgetTimeMax;
            walking = false;
            running = false;
            player = null;
            if(!isTrapped) agent.speed = 3.5f;
            else agent.speed = 1.75f;
            return;
        }

        if (distance > 2.0f)
        {
            if (!isTrapped) agent.speed = 8f;
            else agent.speed = 4f;
            agent.isStopped = false;
            agent.SetDestination(transform.position + targetPoint);
            running = true;
            walking = false;
        }
    }
    private void ResetWalkingVariables()
    {
        walking = false;
        agent.isStopped = true;
        walkingRefreshTime = walkingRefreshTimeMax;
    }
    private float CalculateDistance(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }
    public void DecreaseHealth(GameObject player, float amount)
    {
        if (health <= 0) return;

        audioSource.Stop();

        health -= amount;
        if (this.player == null) this.player = player;
        audioSource.PlayOneShot(getDamageAudio);
    }
    private void PlayWalkingSound()
    {
        if (audioSource.isPlaying && audioSource.clip == walkAudio) return;

        audioSource.clip = walkAudio;
        audioSource.Play();
    }
    private void PlayRunningSound()
    {
        if (audioSource.isPlaying && (audioSource.clip == runAudio1 || audioSource.clip == runAudio2)) return;

        if (audioSource.clip == runAudio1)
        {
            audioSource.clip = runAudio2;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = runAudio1;
            audioSource.Play();
        }
    }
    private void PlayAnimation(bool stopAnimations, AnimationClip anim)
    {
        if (stopAnimations) this.anim.Stop();

        if (!this.anim.IsPlaying(anim.name))
            this.anim.Play(anim.name);
    }
    private void DestroyAllComponents()
    {
        Component[] allComponents = GetComponents<Component>();

        for (int i = 1; i < allComponents.Length; i++)
        {
            if (allComponents[i].GetType().Name == "Rigidbody" || allComponents[i].GetType().Name == "FootstepMaterial" ||
                allComponents[i].GetType().Name == "Animation") continue;

            Destroy(allComponents[i]);
        }

        OtherLootDestroyCounter destroyCounter = transform.AddComponent<OtherLootDestroyCounter>();
        destroyCounter.SetCounter(240f);

        Loot loot = transform.AddComponent<Loot>();
        loot.lootType = Loots.Deer;
        loot.minimumItemCount = 3;
        loot.maximumItemCount = 7;
        loot.FillItems();

        lootManager.AddOtherLoot(gameObject);

        tag = "Loot";
        SetLayer(transform);
    }
    private void SetLayer(Transform parent)
    {
        parent.gameObject.layer = 3;

        foreach (Transform child in parent)
        {
            SetLayer(child);
        }
    }
    public void SetTrapped(bool value)
    {
        if (isTrapped == value) return;
        isTrapped = value;

        if (agent == null) return;
        if (isTrapped) agent.speed = originalSpeed / 2;
        else agent.speed = originalSpeed;
    }
}

public enum AnimalType
{
    Deer
}
