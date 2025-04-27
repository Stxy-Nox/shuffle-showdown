using UnityEngine;
using System.Collections.Generic;

namespace ShuffleShowdown
{
    public class PlayerWeaponManager : MonoBehaviour
    {
        [Header("武器设置")]
        public List<GameObject> weaponPrefabs = new List<GameObject>();  // 武器预制件列表
        public Transform weaponHolder;  // 武器挂载点
        
        private List<Weapon> activeWeapons = new List<Weapon>();  // 激活的武器列表
        
        private void Start()
        {
            // 如果没有指定武器挂载点，使用玩家对象
            if (weaponHolder == null)
            {
                weaponHolder = transform;
            }
            
            // 初始化武器
            InitializeWeapons();
        }
        
        private void InitializeWeapons()
        {
            // 清空当前武器
            foreach (Weapon weapon in activeWeapons)
            {
                if (weapon != null)
                {
                    Destroy(weapon.gameObject);
                }
            }
            activeWeapons.Clear();
            
            // 添加初始武器
            foreach (GameObject weaponPrefab in weaponPrefabs)
            {
                AddWeapon(weaponPrefab);
            }
            
            // 如果没有初始武器，添加默认武器
            if (activeWeapons.Count == 0)
            {
                CreateDefaultWeapon();
            }
        }
        
        public void AddWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab != null)
            {
                // 实例化武器并挂载
                GameObject weaponObj = Instantiate(weaponPrefab, weaponHolder);
                Weapon weapon = weaponObj.GetComponent<Weapon>();
                
                if (weapon != null)
                {
                    activeWeapons.Add(weapon);
                }
            }
        }
        
        private void CreateDefaultWeapon()
        {
            // 创建一个空物体作为默认武器
            GameObject defaultWeapon = new GameObject("DefaultWeapon");
            defaultWeapon.transform.SetParent(weaponHolder);
            defaultWeapon.transform.localPosition = Vector3.zero;
            
            // 添加武器组件
            Weapon weapon = defaultWeapon.AddComponent<Weapon>();
            
            // 设置默认参数
            weapon.attackRange = 8f;
            weapon.attackRate = 2f;
            weapon.bulletSpeed = 15f;
            weapon.bulletDamage = 5f;
            weapon.bulletLifetime = 2f;
            
            // 创建一个简单的子弹预制件
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.name = "DefaultBullet";
            bullet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            
            // 添加必要的组件
            bullet.AddComponent<Bullet>();
            
            // 设置物理属性
            bullet.GetComponent<Collider>().isTrigger = true;
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            
            // 设置为不可见（只在场景视图中可见）
            bullet.SetActive(false);
            
            // 把这个临时预制件赋值给武器
            weapon.bulletPrefab = bullet;
            
            // 设置发射点
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(defaultWeapon.transform);
            firePointObj.transform.localPosition = new Vector3(0, 0, 0.5f);
            weapon.firePoint = firePointObj.transform;
            
            // 添加到活跃武器列表
            activeWeapons.Add(weapon);
        }
    }
}