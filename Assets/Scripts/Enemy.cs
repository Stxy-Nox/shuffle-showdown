using UnityEngine;

namespace ShuffleShowdown
{
    public class Enemy : MonoBehaviour
    {
        [Header("移动设置")]
        public float moveSpeed = 3.0f;
        public bool faceMovementDirection = false; // 设为false，敌人不会旋转自身

        [Header("目标设置")]
        public Transform target; // 目标（通常是玩家）
        
        [Header("属性")]
        public float health = 10f;
        public int damage = 1;
        public int scoreValue = 10;
        
        [Header("调试")]
        public bool showDebug = false;
        
        private Rigidbody rb;
        private Vector3 moveDirection;
        private float damageInterval = 0.5f; // 造成伤害的时间间隔
        private float lastDamageTime = 0f;
        private EnemyHealth healthComponent;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            healthComponent = GetComponent<EnemyHealth>();
            
            // 如果没有找到EnemyHealth组件，尝试添加一个
            if (healthComponent == null)
            {
                healthComponent = gameObject.AddComponent<EnemyHealth>();
                Debug.Log("敌人添加了健康组件");
            }
        }
        
        private void Start()
        {
            // 尝试找到目标（如果未手动设置）
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }
            
            // 同步健康值
            if (healthComponent != null)
            {
                healthComponent.maxHealth = health;
                healthComponent.currentHealth = health;
            }
        }
        
        private void Update()
        {
            if (target != null)
            {
                // 计算朝向玩家的方向
                moveDirection = (target.position - transform.position).normalized;
                
                // 可视化（调试）
                if (showDebug)
                {
                    Debug.DrawRay(transform.position, moveDirection * 5f, Color.red);
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (target != null)
            {
                // 直接移动，不使用物理系统
                Vector3 velocity = moveDirection * moveSpeed;
                
                // 使用Transform直接移动，而不是通过刚体
                transform.position += velocity * Time.fixedDeltaTime;
                
                // 如果需要面向移动方向，则旋转敌人
                if (faceMovementDirection && moveDirection != Vector3.zero)
                {
                    transform.forward = moveDirection;
                }
            }
        }
        
        // 当敌人受到伤害时调用
        public void TakeDamage(float amount)
        {
            // 更新内部健康值
            health -= amount;
            
            Debug.Log("敌人受到 " + amount + " 点伤害，剩余生命值: " + health);
            
            // 如果有健康组件，则使用它来处理伤害
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(amount);
            }
            else if (health <= 0)
            {
                Die();
            }
        }
        
        // 敌人死亡
        public void Die()
        {
            // 加分或触发其他事件
            Debug.Log("敌人死亡");
            
            // 销毁敌人对象
            Destroy(gameObject);
        }
        
        // 使用触发器检测玩家
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && Time.time > lastDamageTime + damageInterval)
            {
                // 对玩家造成伤害
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("敌人开始对玩家造成 " + damage + " 点伤害");
                    playerHealth.TakeDamage(damage);
                    lastDamageTime = Time.time;
                }
                else
                {
                    Debug.LogError("找不到玩家的PlayerHealth组件!");
                }
            }
        }
    }
}