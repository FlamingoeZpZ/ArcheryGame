using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    [SerializeField] private GameObject restartButton;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI score;

    private float scoreNum;
    private bool isDead;

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
        if(!isDead) MouseMoveDelta?.Invoke(eventData.delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isDead)OnMousePressed?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isDead)OnMouseReleased?.Invoke();
    }
    
    public void SwapWeapon(Weapon wo)
    {
        OnSwapWeapon(wo);
    }
    
    public void SwapProjectile(Projectile wo)
    {
        OnSwapProjectile(wo);
    }

    public void OnDeath()
    {
        SetHealth(0);
        restartButton.SetActive(true);
        isDead = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void SetHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
    }

    public void AddScore(float statsValue)
    {
        if (isDead) return;
        scoreNum += statsValue;
        score.text = scoreNum.ToString(CultureInfo.InvariantCulture);
    }
}
