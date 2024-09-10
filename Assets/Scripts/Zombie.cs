using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private LootManager lootManager;
    private NavMeshAgent zombieAgent;
    private GameObject target;
    [SerializeField] private ZombieCategory category;

    private float forgetTime = 10f;
    private float forgetTimeMax = 10f;

    private Animation anim;
    private AudioSource audioSource;

    [SerializeField] private AnimationClip walkAnimation;
    [SerializeField] private AnimationClip runAnimation;
    [SerializeField] private AnimationClip crawlAnimation;
    [SerializeField] private AnimationClip idleAnimation;
    [SerializeField] private AnimationClip crawlIdleAnimation;
    [SerializeField] private AnimationClip biteAnimation; 
    [SerializeField] private AnimationClip crawlAttackAnimation;
    [SerializeField] private AnimationClip dieAnimation; 

    [SerializeField] private AudioClip idleAudio;
    [SerializeField] private AudioClip growlAudio;
    [SerializeField] private AudioClip getDamageAudio1, getDamageAudio2;
    [SerializeField] private AudioClip biteAudio;
    [SerializeField] private AudioClip dieAudio;

    public bool searching;
    public bool attacking;
    public bool walking;

    public Vector3 walkingPosition;
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
        zombieAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animation>();
        audioSource = GetComponent<AudioSource>();

        originalSpeed = zombieAgent.speed;
    }
    void Update()
    {
        CheckHealth();
        CheckAnimation();
        CheckWalkingRefreshTime();
        CheckIdleTime();
        SearchPlayer();
        WalkToTarget();
        LookPlayer();
    }
    private void CheckHealth()
    {
        if (health > 0 || zombieAgent == null) return;

        walking = attacking = searching = false;
        PlayAnimation(true, dieAnimation);
        zombieAgent.destination = transform.position;
        zombieAgent = null;
        Invoke(nameof(DestroyAllComponents), destroyComponentTime);
    }
    private void CheckAnimation()
    {
        if(walking)
        {
            if (category == ZombieCategory.Normal) PlayAnimation(false, walkAnimation);
            else if (category == ZombieCategory.Crawl) PlayAnimation(false, crawlAnimation);
        }
    }
    private void CheckWalkingRefreshTime()
    {
        if (!walking || health <= 0 || !searching || attacking) return;

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
        if (walking || attacking || health <= 0) return;

        idleTime -= Time.deltaTime;

        if(category == ZombieCategory.Normal)
            PlayAnimation(false, idleAnimation);
        else if (category == ZombieCategory.Crawl)
            PlayAnimation(false, crawlIdleAnimation);

        if (idleTime <= 0)
        {
            WalkRandom();
            idleTime = Random.Range(idleTimeMin, idleTimeMax);
        }
    }
    private void LookPlayer()
    {
        if (target == null || health <= 0 || zombieAgent == null) return;

        Vector3 lookDirection = target.transform.position - transform.position;
        lookDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
    private void WalkRandom()
    {
        if (walking || attacking || health <= 0 || !searching || idleTime > 0) return;

        walking = true;
        PlayAudio(false, idleAudio);

        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        Terrain terrain = Terrain.activeTerrain;

        Vector3 scale = terrain.terrainData.heightmapScale;

        float firstY = terrain.terrainData.GetHeight(Mathf.RoundToInt(transform.position.x + x / scale.x), Mathf.RoundToInt(transform.position.z / scale.z));
        float y = terrain.SampleHeight(new Vector3(transform.position.x + x, firstY, transform.position.z + z));

        walkingPosition = new Vector3(transform.position.x + x, y, transform.position.z + z);

        zombieAgent.isStopped = false;
        zombieAgent.SetDestination(walkingPosition);
    }
    private void SearchPlayer()
    {
        if (target != null || health <= 0) return;

        searching = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        if (CalculateDistance(player) <= 10)
        {
            PlayAudio(true, growlAudio);
            target = player;
            searching = false;
        }
    }
    private void WalkToTarget()
    {
        if (zombieAgent == null || target == null || health <= 0 || attacking) return;

        float distance = CalculateDistance(target);

        if (distance <= 20) forgetTime = forgetTimeMax;

        else if (forgetTime > 0) forgetTime -= Time.deltaTime;

        if (forgetTime <= 0)
        {
            target = null;
            zombieAgent.isStopped = true;
            forgetTime = forgetTimeMax;
            walking = false;
            return;
        }

        if (distance > 2.0f)
        {
            zombieAgent.isStopped = false;
            zombieAgent.SetDestination(target.transform.position);
            walking = true;
        }
        else
        {
            zombieAgent.isStopped = true;
            AttackTarget();
        }
    }
    private void AttackTarget()
    {
        if (attacking) return;

        attacking = true;
        walking = false;

        if (category == ZombieCategory.Normal)
        {
            PlayAudio(true, biteAudio);
            PlayAnimation(true, biteAnimation);
            Invoke(nameof(DamageTime), 0.50f);
            Invoke(nameof(FinishAttacking), biteAnimation.length + 0.25f);
        }
            
        else if (category == ZombieCategory.Crawl)
        {
            PlayAnimation(true, crawlAttackAnimation);
            Invoke(nameof(DamageTime), 0.40f);
            Invoke(nameof(FinishAttacking), crawlAttackAnimation.length + 0.25f);
        }           
    }
    private void DamageTime()
    {
        if (CalculateDistance(target) > 2.5f || health <= 0) return;

        target.GetComponent<PlayerStats>().DecreaseHealth(10, true);
    }
    private void FinishAttacking()
    {
        attacking = false;
    }
    private void ResetWalkingVariables()
    {
        walking = false;
        zombieAgent.isStopped = true;
        walkingRefreshTime = walkingRefreshTimeMax;
    }
    private float CalculateDistance(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }
    public void DecreaseHealth(GameObject player, float amount)
    {
        if (health <= 0) return;

        audioSource.Stop();

        if (health - amount > 0)
        {
            int randomNumber = Random.Range(0, 2);
            if (randomNumber == 0) audioSource.PlayOneShot(getDamageAudio1);
            else audioSource.PlayOneShot(getDamageAudio2);
        }
        else audioSource.PlayOneShot(dieAudio);    

        health -= amount;

        if (target == null)
        {
            PlayAudio(true, growlAudio);
            target = player;
        }
    }
    private void PlayAudio(bool stopSounds, AudioClip sound)
    {
        if(stopSounds) audioSource.Stop();

        if(audioSource.clip != sound)
        audioSource.PlayOneShot(sound);
    }
    private void PlayAnimation(bool stopAnimations, AnimationClip anim)
    {
        if (stopAnimations) this.anim.Stop();

        if(!this.anim.IsPlaying(anim.name))
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
        destroyCounter.SetCounter(120f);

        Loot loot = transform.AddComponent<Loot>();
        loot.lootType = Loots.Zombie;
        loot.minimumItemCount = 0;
        loot.maximumItemCount = 3;
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

        if (zombieAgent == null) return;
        if (isTrapped) zombieAgent.speed = originalSpeed / 2;
        else zombieAgent.speed = originalSpeed;
    }
}

public enum ZombieCategory
{
    Normal,
    Crawl
}