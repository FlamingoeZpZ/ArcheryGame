using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO add splines
public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    [SerializeField] protected WeaponSO weaponStats;
    [SerializeField] private Projectile projectile;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clipPullBack;
    [SerializeField] private AudioClip clipReload;
    [SerializeField] private AudioClip clipRelease;
    
    private float _currentFireDuration; // Bind UI event to get the charge.
    private bool _isFiring;
    
    private Animator _animator;
    private AudioSource _bowAudioSource;
    private GameObject _owner;
    private Projectile _fake;
    public Projectile ProjectileType => projectile;
    public Action OnCanShoot { get; set; }
    public Action OnShoot { get; set; }

    public float CurrentFirePower() => Mathf.Lerp(projectile.Stats.MinSpeed, projectile.Stats.MaxSpeed,
        (_currentFireDuration / weaponStats.FullChargeTime));
    
   
    

    public void Init(GameObject owner)
    {
        _owner = owner;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bowAudioSource = GetComponent<AudioSource>();
        _fake = Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
        _fake.enabled = false;
        //p.transform.eulerAngles += new Vector3(0, -90, 0);
    }


    private IEnumerator FireTimer()
    {
        _currentFireDuration = 0;
        _bowAudioSource.clip = clipPullBack;
        _bowAudioSource.Play();
        //_bowAudioSource.loop = true;
        float val = _currentFireDuration / weaponStats.FullChargeTime;
        while (val < weaponStats.MinChargePercent)
        {
            _currentFireDuration += Time.deltaTime;
            val = _currentFireDuration / weaponStats.FullChargeTime; // A bit cheaper than doing it twice.
            _animator.SetFloat(StaticUtilities.ChargePercentID ,val);
            yield return null;
        }
        OnCanShoot?.Invoke();
        while (_currentFireDuration < weaponStats.FullChargeTime)
        {
            _currentFireDuration += Time.deltaTime;
            _animator.SetFloat(StaticUtilities.ChargePercentID ,_currentFireDuration / weaponStats.FullChargeTime);
            yield return null;
        }
        _animator.SetFloat(StaticUtilities.ChargePercentID  ,1);
        _currentFireDuration = weaponStats.FullChargeTime;
        yield return weaponStats.HoldTime;
        TryFire();
    }
    
    public void BeginFire()
    {
        print("Begin Firing");
        StartCoroutine(FireTimer());
        _animator.SetBool(StaticUtilities.IsFiringID ,true);
        _isFiring = true;
        
    }

    public void EndFire()
    {
        print("End Firing");
        StopAllCoroutines();
        TryFire();
        //_bowAudioSource.loop = false;
        _isFiring = false;
    }

    protected virtual void Fire(float percent)
    {
        Projectile p = Instantiate(projectile, firePoint.position, firePoint.rotation);
        p.Init(percent, _owner);
        _currentFireDuration = 0;
        
      
        _animator.SetTrigger(StaticUtilities.ReloadID);
        
        _bowAudioSource.PlayOneShot(clipRelease);

        _bowAudioSource.clip = clipReload;
        _bowAudioSource.loop = false;
        _bowAudioSource.PlayDelayed(0.05f);
       
        if (weaponStats.IsFullAuto && _isFiring)
        {
            StartCoroutine(FireTimer());
        }
        else
        {
            _animator.SetBool(StaticUtilities.IsFiringID ,false);
        }

    }

    public bool CanFire()
    {
        return CanFire(_currentFireDuration / weaponStats.FullChargeTime);
    }

    protected virtual bool CanFire(float percent)
    {
        return percent >= weaponStats.MinChargePercent;
    }

    private void TryFire()
    {
        float firePercent = _currentFireDuration / weaponStats.FullChargeTime; // Assume this is correct
        if (CanFire(firePercent))
        {
            Fire(firePercent);
            OnShoot.Invoke();
        }
        else
        {
            _animator.SetBool(StaticUtilities.IsFiringID ,false);
        }
    }

    public void SetProjectile(Projectile projectile1)
    {
        projectile = projectile1;
        
        _fake = Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
        _fake.enabled = false;
    }
}
