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
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            rb.isKinematic = true; // 设置为运动学，不受物理系统影响
            
            // 确保在Enemy层
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            
            // 修改碰撞体为触发器
            Collider[] colliders = GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.isTrigger = true;
            }
            
            // 如果没有指定目标，默认寻找玩家
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }
        
        private void Update()
        {
            if (target != null)
            {
                // 计算朝向玩家的方向
                moveDirection = (target.position - transform.position).normalized;
                moveDirection.y = 0; // 确保只在水平面上移动
                
                if (showDebug)
                {
                    Debug.DrawRay(transform.position, moveDirection * 2f, Color.red);
                }
            }
        }
        
        private void FixedUpdate()
        {
            Move();
        }
        
        private void Move()
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
            health -= amount;
            
            if (health <= 0)
            {
                Die();
            }
        }
        
        // 敌人死亡
        private void Die()
        {
            // 加分或触发其他事件
            
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
                    playerHealth.TakeDamage(damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}