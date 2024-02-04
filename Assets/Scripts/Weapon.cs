using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO add splines
public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] protected WeaponSO weaponStats;
    [SerializeField] private Projectile projectile;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clipPullBack;
    [SerializeField] private AudioClip clipReload;
    [SerializeField] private AudioClip clipRelease;
    
    private float _currentFireDuration; // Bind UI event to get the charge.
    private bool isFiring;
    
    private Animator _animator;
    private AudioSource _bowAudioSource;
    private GameObject _owner;
    private Projectile fake;

    public void Init(GameObject owner)
    {
        _owner = owner;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _bowAudioSource = GetComponent<AudioSource>();
        fake = Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
        fake.enabled = false;
        //p.transform.eulerAngles += new Vector3(0, -90, 0);
    }


    private IEnumerator FireTimer()
    {
        _currentFireDuration = 0;
        _bowAudioSource.clip = clipPullBack;
        _bowAudioSource.Play();
        //_bowAudioSource.loop = true;
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
        isFiring = true;
    }

    public void EndFire()
    {
        print("End Firing");
        StopAllCoroutines();
        TryFire();
        //_bowAudioSource.loop = false;
        isFiring = false;
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
       
        if (weaponStats.IsFullAuto && isFiring)
        {
            StartCoroutine(FireTimer());
        }
        else
        {
            _animator.SetBool(StaticUtilities.IsFiringID ,false);
        }

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
        }
        else
        {
            _animator.SetBool(StaticUtilities.IsFiringID ,false);
        }
    }

    public void SetProjectile(Projectile projectile1)
    {
        projectile = projectile1;
        
        fake = Instantiate(projectile, firePoint.position, firePoint.rotation, firePoint);
        fake.enabled = false;
    }
}
