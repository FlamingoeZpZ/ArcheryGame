using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    //OLD (for equipping new weapons and buttons)
    public class WeaponToUI : MonoBehaviour
    {

        [SerializeField] private RectTransform parent;
        [SerializeField] private Button prefab;
        
        private Weapon currentWeapon;
        public static WeaponToUI instance;

        private Dictionary<Projectile, TextMeshProUGUI> ammoValues = new();
        
        private void Awake()
        {
            if (instance && instance != this)
            {
                Destroy(instance.gameObject);
            }
            instance = this;
            //PlayerControls.Instance.OnSwapWeapon += SetWeapon;
        }

        private void AddProjectileType(Projectile projectile, int amount)
        {
            Button btn = Instantiate(prefab, parent);
            //btn.onClick.AddListener(() => PlayerControls.Instance.OnSwapProjectile(projectile));
            //btn.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = projectile.Stats.Icon;
            TextMeshProUGUI tmp = btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tmp.text = amount.ToString();
            ammoValues.Add(projectile, tmp);
        }
        

        //Each weapon stores a series of projectiles
        public void SetWeapon(Weapon wep)
        {
            currentWeapon = wep;

            //Clear previous
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Destroy(parent.GetChild(i));
            }
            
            ammoValues.Clear();
            
            //
            /*foreach (Projectile p in wep.Projectiles)
            {
                Button btn = Instantiate(prefab, parent);
                btn.onClick.AddListener(() => PlayerControls.Instance.OnSwapProjectile(p));
                btn.transform.GetChild(0).GetComponent<Image>().sprite = p.stats.icon;
                TextMeshProUGUI tmp = btn.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                tmp.text = wep.Projectiles
                ammoValues.Add(p.name, tmp);
            }*/
        }

        /*
        public void UpdateAmmo(string id, int value)
        {
            
        }
        */


    }
}