using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private float dayDuration;
    [SerializeField] private float spinSpeedMul;
    [SerializeField, Range(0,2)] private float maxExposure;
    [SerializeField, Range(0,2)] private float minExposure;
    
    [SerializeField] private Color dayTint;
    [SerializeField] private Color nightTint;
    [SerializeField] private AnimationCurve tintCurve;

    private float _currentTime = 0;
    private Light _sun;
    
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");
    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    public static Action OnDayEnd;

    public static bool IsActive = true;

    private static float dayProgression;
    public static float GetProgression() => dayProgression;

    private void Start()
    {
        _sun = GetComponent<Light>();
    }

    // Update is called once per frame
    void LateUpdate() // Graphical things are usually handled in LateUpdate
    {
       
        
        _currentTime += Time.deltaTime;
        skyboxMaterial.SetFloat(Rotation, _currentTime);
        
       
       if (!IsActive) return;
        UpdateSun();
        if (_currentTime > dayDuration)
        {
            EndDay();
        }
    }

    private void UpdateSun()
    {
        dayProgression = _currentTime / dayDuration;
        
        
        skyboxMaterial.SetFloat(Exposure, Mathf.Sin(dayProgression*Mathf.PI)*maxExposure + minExposure);
        
        //We want to rotate the sun between
        _sun.color = Color.Lerp(dayTint, nightTint, tintCurve.Evaluate(dayProgression));
        _sun.transform.rotation = Quaternion.AngleAxis(Mathf.Lerp(15,165,dayProgression),Vector3.right);
    }

    private void EndDay()
    {
        OnDayEnd?.Invoke();
        UpdateSun();
        _currentTime = 0;
        IsActive = false;
    }
}
