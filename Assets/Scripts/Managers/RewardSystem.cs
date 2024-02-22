using UnityEngine;

public class RewardSystem : MonoBehaviour
{

    private ParticleSystem particles;
    
    
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        Enemy.OnDeath += (x) => SpawnParticle(x.EnemyStats.Value , x.transform.position); //* Castle.value
    }

    private void SpawnParticle(float enemyStatsValue, Vector3 location)
    {
        particles.emission.SetBurst(0, new ParticleSystem.Burst(0, (short)enemyStatsValue)); //Important to use the correct constructor.
        particles.transform.position = location;
        particles.Play();
    }
}
