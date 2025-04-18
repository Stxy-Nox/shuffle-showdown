using UnityEngine;

namespace ShuffleShowdown
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5.0f;
        public float acceleration = 15.0f;  // 加速度参数
        public float deceleration = 20.0f;  // 减速度参数
        
        [Header("Ground Check")]
        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer = -1;

        private Rigidbody rb;
        private Vector3 moveDirection;
        private Vector3 currentVelocity;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            rb.useGravity = false; // 在俯视角游戏中通常不需要重力
        }

        private void Update()
        {
            // 获取输入
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            // 计算移动方向
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            
            // 平滑应用速度变化
            if (moveDirection != Vector3.zero)
            {
                // 加速
                currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                // 减速至停止
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
            }
            
            // 应用计算出的速度
            rb.linearVelocity = currentVelocity;
            
            // 让玩家朝向移动方向 (只有在移动时才改变方向)
            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }
        }
    }
}