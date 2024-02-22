using Cinemachine;
using UnityEngine;

public class Castle : MonoBehaviour, IDamagable // By being I damagble, we allow other things to hurt us like bombs
{
    [field: SerializeField] public float MaxHealth { get; set; }
    [SerializeField] private AudioClip hitNoise;
    public float CurrentHealth { get; set; }
    public static Vector3 Position { get; private set; }

    private CinemachineImpulseSource _impulseSource;
    private AudioSource _audioSource;
    
    

    private void Start()
    {
        CurrentHealth = MaxHealth;
        Position = transform.GetChild(0).position;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _audioSource = GetComponent<AudioSource>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.attachedArticulationBody.TryGetComponent(out Enemy e) && !e.IsDead)
        {
            //Take damage
            //That's bizzare we can't ask ourselves to take damage?
            //CurrentHealth -= e.EnemyStats.Damage;
            (this as IDamagable).TakeDamage(e.EnemyStats.Damage);
            
            //Destroy enemy
            Destroy(e.gameObject);
            
           //if(CurrentHealth < MaxHealth) OnDie();
        }
    }


    public void OnDie()
    {
        GameManager.Instance.GameOver();
    }

    public void OnHit(float amount)
    {
        //Update health UI
        
        //Play impulse
        _impulseSource.GenerateImpulseWithForce(amount * 5);
        
        //Play sonud
        _audioSource.PlayOneShot(hitNoise);
        
    }

    public void SetHealth(float amount)
    {
        //Update health UI
        CurrentHealth = amount;
    }
}
