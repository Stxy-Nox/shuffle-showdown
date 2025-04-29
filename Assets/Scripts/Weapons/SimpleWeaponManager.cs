using UnityEngine;

namespace ShuffleShowdown
{
    public class SimpleWeaponManager : MonoBehaviour
    {
        [Header("子弹设置")]
        public GameObject bulletPrefab;
        public float fireRate = 2f;
        public float bulletSpeed = 15f;
        public float bulletDamage = 5f;
        public float bulletLifetime = 3f;
        public float attackRange = 10f;
        public Transform firePoint;

        [Header("对象池设置")]
        public int initialPoolSize = 20;  // 初始池大小

        private float nextFireTime = 0f;
        private bool poolInitialized = false;

        private void Start()
        {
            // 如果没有设置发射点，使用玩家的位置
            if (firePoint == null)
            {
                firePoint = transform;
            }

            // 初始化对象池
            InitializeObjectPool();
        }

        private void InitializeObjectPool()
        {
            // 确保场景中有对象池管理器
            if (SimpleObjectPool.Instance == null)
            {
                GameObject poolObj = new GameObject("SimpleObjectPool");
                poolObj.AddComponent<SimpleObjectPool>();
            }

            // 注册子弹预制件到对象池
            if (bulletPrefab != null)
            {
                SimpleObjectPool.Instance.RegisterPrefab("BulletPrefab", bulletPrefab);
                SimpleObjectPool.Instance.PreWarm("BulletPrefab", initialPoolSize);
                poolInitialized = true;
            }
            else
            {
                Debug.LogError("没有设置子弹预制件!");
            }
        }

        private void Update()
        {
            // 每帧尝试攻击
            if (poolInitialized)
            {
                TryAttack();
            }
            else if (SimpleObjectPool.Instance != null)
            {
                // 如果对象池后来才被创建，再次尝试初始化
                InitializeObjectPool();
            }
        }

        private void TryAttack()
        {
            if (Time.time > nextFireTime)
            {
                // 寻找最近的敌人
                Transform target = FindNearestEnemy();

                if (target != null)
                {
                    // 计算射击方向
                    Vector3 direction = (target.position - firePoint.position).normalized;
                    
                    // 创建子弹
                    CreateBullet(direction);
                    
                    // 设置下一次射击时间
                    nextFireTime = Time.time + (1f / fireRate);
                }
            }
        }

        private Transform FindNearestEnemy()
        {
            // 查找所有敌人
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            if (enemies.Length == 0)
            {
                return null;
            }
            
            Transform nearestEnemy = null;
            float nearestDistance = attackRange;
            
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
            
            return nearestEnemy;
        }

        private void CreateBullet(Vector3 direction)
        {
            // 子弹生成位置稍微抬高一点，避免与地面碰撞
            Vector3 spawnPos = firePoint.position + new Vector3(0, 0.3f, 0);
            
            // 从对象池获取子弹
            GameObject bulletObj = SimpleObjectPool.Instance.GetFromPool("BulletPrefab", spawnPos, Quaternion.LookRotation(direction));
            
            if (bulletObj != null)
            {
                // 设置子弹属性
                SimpleBullet bullet = bulletObj.GetComponent<SimpleBullet>();
                if (bullet != null)
                {
                    bullet.Initialize(direction, bulletSpeed, bulletDamage, bulletLifetime);
                    
                    // 为了可视化效果，给子弹添加一个材质颜色
                    Renderer bulletRenderer = bulletObj.GetComponent<Renderer>();
                    if (bulletRenderer != null)
                    {
                        bulletRenderer.material.color = Color.red;
                    }
                    
                    //Debug.Log("创建了一颗子弹，方向: " + direction);
                }
                else
                {
                    Debug.LogWarning("没有找到SimpleBullet组件!");
                }
            }
        }
    }
}