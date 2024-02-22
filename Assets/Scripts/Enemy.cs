using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(100)]
public class Enemy : MonoBehaviour, IDamagable
{
    public float MaxHealth
    {
        get => stats.MaxHealth;
        set { } // Do nothing.
    }

    public float CurrentHealth { get; set; }
    
    
    //public GameObject PreviousAttacker { get; set; }
    
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField] private EnemySo stats;

    private float _remainingAttackTime;
    
    private bool _isIdle;

    public bool IsDead { get; private set; }

    public static int EnemyCount {get; private set;}

    public static Action<Enemy> OnDeath;

    public EnemySo EnemyStats => stats;
   

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.SetDestination(Castle.Position);
        _animator.speed = stats.AnimationSpeed;
        _agent.speed *= stats.AnimationSpeed;
        EnemyCount++;
    }

    public void OnDie()
    {
        print("I've died");
        if (IsDead) return;
        //Ragdoll
        IsDead = true;
        _animator.enabled = false;
        _agent.enabled = false;
        OnDeath?.Invoke(this);
        StartCoroutine(GoThroughFloor());
       
    }

    public void OnHit(float amount)
    {
        _animator.SetTrigger(amount < 50 ? StaticUtilities.HitSmallID : StaticUtilities.HitBigID);
        AudioSource.PlayClipAtPoint(stats.HitNoise, transform.position, 10);
        StartCoroutine(Stun(amount));
    }

    private IEnumerator Stun(float amount)
    {
        float s = _agent.speed;
        _agent.speed = 0;
        //_animator.SetBool(StaticUtilities.IsIdleID, true);
        yield return new WaitForSeconds(amount * stats.StunTolerance);
        //_animator.SetBool(StaticUtilities.IsIdleID, _isIdle);
        _agent.speed = s;
    }
    private IEnumerator GoThroughFloor()
    {
        //This function can only ever run once, therefore it's not THAT expensive. Still not ideal.
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
        
        yield return new WaitForSeconds(stats.PhaseThroughFloorTime);

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }
        
        while (true)
        {
            transform.position += Vector3.down * (Time.deltaTime * stats.FallSpeed);
            if (transform.position.y < 5)
            {
                Destroy(gameObject);    
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        --EnemyCount;//Doesn't matter which enemy is dead, none of them will be
        if (EnemyCount <= 0 && GameManager.GameRunning) // Realistically, == is fine
        {
           GameManager.Instance.EndDay();
        }
    }

    private void OnDisable()
    {
        //Well an unintended side effect of this, is that they will all ragdoll xD
        //_animator.enabled = false;
        _animator.speed = 0;
        _agent.enabled = false;
    }
}
