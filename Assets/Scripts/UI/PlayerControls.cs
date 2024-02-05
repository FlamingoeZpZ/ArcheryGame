using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-100)]
//Attached to the camera...
public class PlayerControls : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public static PlayerControls Instance { get; private set; }

    [NonSerialized] public Action<Vector2> MouseMoveDelta;
    [NonSerialized] public Action OnMousePressed;
    [NonSerialized] public Action OnMouseReleased;
    
    [NonSerialized] public Action<Weapon> OnSwapWeapon; 
    [NonSerialized] public Action<Projectile> OnSwapProjectile; 
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        MouseMoveDelta?.Invoke(eventData.delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMousePressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnMouseReleased?.Invoke();
    }
    
    public void SwapWeapon(Weapon wo)
    {
        OnSwapWeapon(wo);
    }
    
    public void SwapProjectile(Projectile wo)
    {
        OnSwapProjectile(wo);
    }
}
