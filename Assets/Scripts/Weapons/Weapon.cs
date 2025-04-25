using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShuffleShowdown
{
    public class Weapon : MonoBehaviour
    {
        [Header("武器设置")]
        public float attackRange = 10f;       // 攻击范围
        public float attackRate = 1f;         // 每秒攻击次数
        public float bulletSpeed = 10f;       // 子弹速度
        public float bulletDamage = 5f;       // 子弹伤害
        public float bulletLifetime = 3f;     // 子弹生存时间
        public GameObject bulletPrefab;       // 子弹预制件
        public Transform firePoint;           // 发射点
        
        [Header("目标检测")]
        public LayerMask enemyLayer;          // 敌人层
        public float targetSearchInterval = 0.2f; // 目标搜索间隔
        
        private float nextFireTime = 0f;
        private Transform currentTarget;
        private bool canFire = true;
        
        private void Start()
        {
            // 如果没有手动设置敌人层，自动设置
            if (enemyLayer == 0)
            {
                enemyLayer = LayerMask.GetMask("Enemy");
            }
            
            // 如果没有设置发射点，使用自身
            if (firePoint == null)
            {
                firePoint = transform;
            }
            
            // 开始周期性搜索目标
            StartCoroutine(SearchForTargets());
        }
        
        private void Update()
        {
            // 如果有目标且可以发射，则尝试攻击
            if (currentTarget != null && canFire && Time.time >= nextFireTime)
            {
                FireAtTarget();
            }
        }
        
        private IEnumerator SearchForTargets()
        {
            while (true)
            {
                FindClosestEnemy();
                yield return new WaitForSeconds(targetSearchInterval);
            }
        }
        
        private void FindClosestEnemy()
        {
            // 在攻击范围内查找所有敌人
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            
            if (hitColliders.Length > 0)
            {
                // 找到最近的敌人
                float closestDistance = float.MaxValue;
                Transform closestEnemy = null;
                
                foreach (var hitCollider in hitColliders)
                {
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = hitCollider.transform;
                    }
                }
                
                currentTarget = closestEnemy;
            }
            else
            {
                currentTarget = null;
            }
        }
        
        private void FireAtTarget()
        {
            // 设置下一次发射时间
            nextFireTime = Time.time + (1f / attackRate);
            
            // 创建子弹
            if (bulletPrefab != null)
            {
                // 创建子弹实例
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                
                // 计算朝向目标的方向
                Vector3 direction = (currentTarget.position - firePoint.position).normalized;
                
                // 设置子弹朝向
                bullet.transform.forward = direction;
                
                // 添加子弹脚本并初始化
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                if (bulletComponent == null)
                {
                    bulletComponent = bullet.AddComponent<Bullet>();
                }
                
                bulletComponent.Initialize(direction, bulletSpeed, bulletDamage, bulletLifetime);
            }
        }
        
        // 辅助方法：在场景中可视化攻击范围
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            
            if (firePoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(firePoint.position, 0.2f);
            }
        }
    }
}