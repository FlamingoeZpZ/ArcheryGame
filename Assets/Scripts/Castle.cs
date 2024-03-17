using Cinemachine;
using UnityEngine;

public class Castle : MonoBehaviour, IDamagable // By being I damagble, we allow other things to hurt us like bombs
{
    private CastleSO castleLevel;
    
    //[SerializeField] private CastleSO[] castleLevels;
    [SerializeField] private AudioClip hitNoise;
    
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public static Vector3 Position { get; private set; }

    private CinemachineImpulseSource _impulseSource;
    private AudioSource _audioSource;
    public float CastleValueMultiplier => castleLevel.CoinMultiplier;

    public static Castle Instance { get; private set; }

    
    private void OnEnable()
    {
        Position = transform.GetChild(0).position;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _audioSource = GetComponent<AudioSource>();
        
        Instance = this;

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
        GameManager.healthBar.value = CurrentHealth / MaxHealth;
        
        //Play impulse
        _impulseSource.GenerateImpulseWithForce(1);
        
        //Play sonud
        _audioSource.PlayOneShot(hitNoise);
        
    }

    public void SetHealth(float amount)
    {
        //Update health UI
        CurrentHealth = amount;
        GameManager.healthBar.value = CurrentHealth / MaxHealth;
    }
    public void SetStats(CastleSO statsReplace)
    {
        castleLevel = statsReplace;
        MaxHealth = castleLevel.MaxHealth;//castleLevels[CastleLevel].MaxHealth;
        SetHealth(MaxHealth);
    }
}
