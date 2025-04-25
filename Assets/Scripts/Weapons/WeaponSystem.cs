using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 添加命名空间
namespace ShuffleShowdown
{
    public class WeaponSystem : MonoBehaviour
    {
        // 单例模式
        public static WeaponSystem Instance { get; private set; }
        
        [Header("武器槽配置")]
        public int weaponSlotCount = 6;                      // 武器槽数量
        public CardWeaponBase[] startingCards;               // 初始卡牌
        
        [Header("子弹预制体")]
        public GameObject bulletPrefab;                      // 子弹预制体
        
        [Header("射击设置")]
        public Transform firePoint;                          // 射击点
        public float attackRange = 10f;                      // 攻击范围
        
        // 武器槽数组
        private CardWeaponBase[] weaponSlots;
        
        // 当前武器参数
        private WeaponParameters currentWeaponParams = new WeaponParameters();
        
        // 射击计时器
        private float shootTimer = 0f;
        
        // 武器系统事件
        public delegate void WeaponSystemEventHandler();
        public event WeaponSystemEventHandler OnWeaponParamsChanged;

        private void Awake()
        {
            // 确保单例实例唯一性
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // 初始化武器槽
            InitializeWeaponSlots();
        }
        
        private void Start()
        {
            // 计算初始武器参数
            RecalculateWeaponParameters();
        }
        
        private void Update()
        {
            // 寻找最近的敌人并自动射击
            if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.InGame)
            {
                AutoAttack();
            }
        }
        
        // 初始化武器槽
        private void InitializeWeaponSlots()
        {
            weaponSlots = new CardWeaponBase[weaponSlotCount];
            
            // 添加初始卡牌
            if (startingCards != null)
            {
                for (int i = 0; i < Mathf.Min(startingCards.Length, weaponSlotCount); i++)
                {
                    weaponSlots[i] = startingCards[i];
                }
            }
        }
        
        // 重新计算武器参数
        public void RecalculateWeaponParameters()
        {
            // 重置参数
            currentWeaponParams.Reset();
            
            // 应用所有卡牌效果
            if (weaponSlots != null)
            {
                foreach (CardWeaponBase card in weaponSlots)
                {
                    if (card != null)
                    {
                        card.ApplyCardEffect(currentWeaponParams);
                    }
                }
            }
            
            // 触发事件
            OnWeaponParamsChanged?.Invoke();
        }
        
        // 自动攻击逻辑
        private void AutoAttack()
        {
            // 射击计时器
            shootTimer += Time.deltaTime;
            
            // 检查是否可以射击
            if (shootTimer >= 1f / currentWeaponParams.fireRate)
            {
                // 寻找最近的敌人
                Transform target = FindNearestEnemy();
                
                // 如果有目标，射击
                if (target != null)
                {
                    Shoot(target.position);
                }
                
                // 重置计时器
                shootTimer = 0f;
            }
        }
        
        // 寻找最近的敌人
        private Transform FindNearestEnemy()
        {
            // 查找所有敌人
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            Transform nearestEnemy = null;
            float minDistance = attackRange;
            
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
            
            return nearestEnemy;
        }
        
        // 射击逻辑
        private void Shoot(Vector3 targetPosition)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                // 计算射击方向
                Vector3 direction = (targetPosition - firePoint.position).normalized;
                
                // 创建子弹实例
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
                
                if (bullet != null)
                {
                    // 使用完整命名空间
                    // 获取子弹组件
                    Bullet bulletComponent = bullet.GetComponent<Bullet>();
                    
                    if (bulletComponent != null)
                    {
                        // 简化参数调用，仅使用可用的参数
                        bulletComponent.Initialize(
                            direction, 
                            currentWeaponParams.bulletSpeed, 
                            currentWeaponParams.damage,
                            currentWeaponParams.range
                        );
                    }
                    else
                    {
                        // 如果找不到Bullet组件，则添加日志
                        Debug.LogWarning("没有找到Bullet组件!");
                    }
                }
            }
        }
        
        // 获取武器槽中的卡牌
        public CardWeaponBase GetCardInSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < weaponSlots.Length)
            {
                return weaponSlots[slotIndex];
            }
            return null;
        }
        
        // 设置武器槽中的卡牌
        public void SetCardInSlot(int slotIndex, CardWeaponBase card)
        {
            if (slotIndex >= 0 && slotIndex < weaponSlots.Length)
            {
                weaponSlots[slotIndex] = card;
                RecalculateWeaponParameters();
            }
        }
        
        // 交换两个卡牌的位置
        public void SwapCards(int slotIndex1, int slotIndex2)
        {
            if (slotIndex1 >= 0 && slotIndex1 < weaponSlots.Length &&
                slotIndex2 >= 0 && slotIndex2 < weaponSlots.Length)
            {
                CardWeaponBase temp = weaponSlots[slotIndex1];
                weaponSlots[slotIndex1] = weaponSlots[slotIndex2];
                weaponSlots[slotIndex2] = temp;
            }
        }
        
        // 获取当前武器参数
        public WeaponParameters GetCurrentWeaponParameters()
        {
            return currentWeaponParams;
        }
        
        // 获取武器槽数量
        public int GetWeaponSlotCount()
        {
            return weaponSlotCount;
        }
        
        // 清空所有武器槽
        public void ClearAllSlots()
        {
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                weaponSlots[i] = null;
            }
            RecalculateWeaponParameters();
        }
    }
}