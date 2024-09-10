using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject hitTreeParticle;
    [SerializeField] private GameObject hitRockParticle;
    [SerializeField] private GameObject hitZombieParticle;
    [SerializeField] private GameObject fallingTreeParticle;
    [SerializeField] private GameObject blockDestroyParticle;

    public void PlayHitTreeParticle(Vector3 position)
    {
        GameObject particle = Instantiate(hitTreeParticle, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponentInChildren<ParticleSystem>();
        particleSystem.Emit(10);
        Destroy(particle, particleSystem.main.duration);
    }
    public void PlayHitRockParticle(Vector3 position)
    {
        GameObject particle = Instantiate(hitRockParticle, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponentInChildren<ParticleSystem>();
        particleSystem.Emit(20);
        Destroy(particle, particleSystem.main.duration);
    }

    public void PlayHitZombieParticle(Vector3 position)
    {
        GameObject particle = Instantiate(hitZombieParticle, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Emit(10);
        Destroy(particle, particleSystem.main.duration);
    }

    public void PlayTreeDownParticle(Vector3 position)
    {
        GameObject particle = Instantiate(fallingTreeParticle, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Emit(100);
        Destroy(particle, particleSystem.main.duration);
    }

    public void PlayBlockDestroyParticle(Vector3 position)
    {
        GameObject particle = Instantiate(blockDestroyParticle, position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        particleSystem.Emit(10);
        Destroy(particle, particleSystem.main.duration);
    }
}
