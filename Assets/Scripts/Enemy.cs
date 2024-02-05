using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamagable
{
    public float MaxHealth
    {
        get => stats.MaxHealth;
        set { } // Do nothing.
    }

    public float CurrentHealth { get; set; }
    public GameObject PreviousAttacker { get; set; }
    
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField] private EnemySO stats;

    private float remainingAttackTime;
    
    private bool isIdle;

    private Rigidbody[] cringe;
    private bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.SetDestination(Player.PlayerTransform.position);
    }

    private void Update()
    {
        if (isDead) return;
        if (!isIdle)
        {
            Traversing();
        }
        else
        {
            InCombat();
        }
    }

    private void InCombat()
    {
        remainingAttackTime -= Time.deltaTime;
        if (remainingAttackTime < 0)
        {
            _animator.SetTrigger(StaticUtilities.PunchID);
        }
    }

    private void GenerateAttackTime()
    {
        remainingAttackTime = Random.Range(stats.MinAttackTime, stats.MaxAttackTime);
    }

    private void Traversing()
    {
        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            Debug.Log("We made it!");
            isIdle = true;
            _animator.SetTrigger(StaticUtilities.PoundID);
            _agent.isStopped = true;
        }
    }

    public void Slam()
    {
        Collider []c = Physics.OverlapSphere(transform.position, 5, StaticUtilities.PlayerLayer);

        foreach (Collider col in c)
        {
            if (col.transform.TryGetComponent(out IDamagable d))
            {
                d.TakeDamage(gameObject, 25);
            }
        }
        _animator.SetBool(StaticUtilities.IsIdleID, true);
    }

    public void Punch()
    {
        Collider []c = Physics.OverlapSphere(transform.position, 5, StaticUtilities.PlayerLayer);

        foreach (Collider col in c)
        {
            if (col.transform.TryGetComponent(out IDamagable d))
            {
                d.TakeDamage(gameObject, 15);
            }
        }
        GenerateAttackTime();
    }


    public GameObject GetSelf()
    {
        return gameObject;
    }

    public void OnDie()
    {
        
        //Ragdoll
        isDead = true;
        _animator.enabled = false;
        _agent.enabled = false;
       
        if (cringe == null)
        {
            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
            cringe = rbs;
            if (PreviousAttacker.TryGetComponent(out Player p))
            {
                p.AddScore(stats.Value);
                stats.OnDeath(p);
            }
        }
        else
        {
            //Reset the sink into ground timer
            StopAllCoroutines();
        }

        foreach (var rb in cringe)
        {
            rb.isKinematic = false;
        }

        StartCoroutine(GoThroughFloor());
       
    }

    public void OnHit(float amount)
    {
        _animator.SetTrigger(amount < 50 ? StaticUtilities.HitSmallID : StaticUtilities.HitBigID);
        StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        float s = _agent.speed;
        _agent.speed = 0;
        _animator.SetBool(StaticUtilities.IsIdleID, true);
        yield return new WaitForSeconds(1);
        _animator.SetBool(StaticUtilities.IsIdleID, isIdle);
        _agent.speed = s;
        GenerateAttackTime();
    }
    private IEnumerator GoThroughFloor()
    {
        yield return new WaitForSeconds(stats.PhaseThroughFloorTime);

        foreach (Rigidbody rb in cringe)
        {
            rb.isKinematic = true;
        }
        
        while (true)
        {
            transform.position += Vector3.down * (Time.deltaTime * stats.FallSpeed);
            if (transform.position.y < -5)
            {
                Destroy(gameObject);    
            }

            yield return null;
        }
    }

}
