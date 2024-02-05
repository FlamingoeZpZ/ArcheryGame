using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamagable
{
    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public GameObject PreviousAttacker { get; set; }
    
    private NavMeshAgent _agent;
    private Animator _animator;

    [SerializeField] private float phaseThroughFloorTime;
    [SerializeField] private float fallSpeed;
    [SerializeField] private float minAttackTime;
    [SerializeField] private float maxAttackTime;

    private float remainingAttackTime;
    
    private bool isIdle;

    private List<Rigidbody> cringe = new();

    
    
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
        remainingAttackTime = Random.Range(minAttackTime, maxAttackTime);
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

    public void GroundPound()
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
        _animator.enabled = false;
        _agent.enabled = false;
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            cringe.Add(rb);
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
        yield return new WaitForSeconds(phaseThroughFloorTime);

        foreach (Rigidbody rb in cringe)
        {
            rb.isKinematic = true;
        }
        
        while (true)
        {
            transform.position += Vector3.down * (Time.deltaTime * fallSpeed);
            if (transform.position.y < -5)
            {
                Destroy(gameObject);    
            }

            yield return null;
        }
    }

}
