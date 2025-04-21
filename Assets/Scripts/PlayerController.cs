using UnityEngine;

namespace ShuffleShowdown
{
    public class PlayerController : MonoBehaviour
    {
        [Header("移动设置")]
        public float moveSpeed = 8.0f;  // 增加默认速度，补偿去掉加速度后的感觉
        
        [Header("调试")]
        public bool showDebug = false;

        private Rigidbody rb;
        private Vector3 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false;
            
            // 确保在Player层
            gameObject.layer = LayerMask.NameToLayer("Player");
            
            if (showDebug)
                Debug.Log("PlayerController初始化完成");
        }

        private void Update()
        {
            // 获取输入
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            // 计算移动方向
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            
            // 显示调试信息
            if (showDebug && (horizontalInput != 0 || verticalInput != 0))
            {
                Debug.Log($"输入值: 水平={horizontalInput}, 垂直={verticalInput}, 方向={moveDirection}");
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            // 直接应用速度，不使用加速度/减速度
            if (moveDirection != Vector3.zero)
            {
                // 直接设置速度
                Vector3 velocity = moveDirection * moveSpeed;
                rb.linearVelocity = velocity;
                
                // 让玩家朝向移动方向
                transform.forward = moveDirection;
                
                if (showDebug)
                {
                    Debug.Log($"速度: {velocity}");
                }
            }
            else
            {
                // 当没有输入时立即停止
                rb.linearVelocity = Vector3.zero;
            }
        }
        
        // 防止其他物体推动玩家
        private void OnCollisionEnter(Collision collision)
        {
            // 如果碰撞到的是敌人，忽略物理反应
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }
}