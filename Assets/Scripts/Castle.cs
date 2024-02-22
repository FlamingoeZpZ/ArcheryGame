using Cinemachine;
using UnityEngine;

public class Castle : MonoBehaviour, IDamagable // By being I damagble, we allow other things to hurt us like bombs
{
    [SerializeField] private CastleSO[] castleLevels;
    [SerializeField] private AudioClip hitNoise;
    
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public static Vector3 Position { get; private set; }

    private CinemachineImpulseSource _impulseSource;
    private AudioSource _audioSource;
    
    
    public int CastleLevel { get; private set; }
    public float CastleValueMultiplier => castleLevels[CastleLevel].CoinMultiplier;

    public void Upgrade()
    {
        CastleLevel++;
        MaxHealth = castleLevels[CastleLevel].MaxHealth;
        CurrentHealth = MaxHealth;
    }
    
    
    private void Start()
    {
        Position = transform.GetChild(0).position;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _audioSource = GetComponent<AudioSource>();
        
        MaxHealth = castleLevels[CastleLevel].MaxHealth;
        CurrentHealth = MaxHealth;
        
    }

    void OnTriggerEnter(Collider other)
    {
        Enemy e = other.transform.GetComponentInParent<Enemy>();
        if(e && !e.IsDead)
        {
            print("I've been hit: " + e.EnemyStats.Damage);
            (this as IDamagable).TakeDamage(e.EnemyStats.Damage);
            
            //Destroy enemy
            Destroy(e.gameObject);
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
