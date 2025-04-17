using UnityEngine;

namespace ShuffleShowdown
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5.0f;
        public float jumpForce = 5.0f;
        
        [Header("Ground Check")]
        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer = -1; // Default to all layers

        private Rigidbody rb;
        private bool isGrounded;
        private Vector3 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            // 防止玩家翻倒
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void Update()
        {
            // 检测地面
            CheckGrounded();
            
            // 获取输入
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            // 计算移动方向（相对于相机）
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            
            // 跳跃
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            // 移动玩家
            Move();
        }

        private void Move()
        {
            if (moveDirection != Vector3.zero)
            {
                // 计算目标速度
                Vector3 targetVelocity = moveDirection * moveSpeed;
                
                // 保持Y轴速度不变（保留重力影响）
                targetVelocity.y = rb.linearVelocity.y;
                
                // 应用速度
                rb.linearVelocity = targetVelocity;
                
                // 让玩家朝向移动方向
                if (moveDirection != Vector3.zero)
                {
                    transform.forward = moveDirection;
                }
            }
        }

        private void Jump()
        {
            // 向上施加力
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void CheckGrounded()
        {
            // 从玩家位置向下发射射线检测地面
            isGrounded = Physics.Raycast(
                transform.position, 
                Vector3.down, 
                groundCheckDistance + 0.1f, // Capsule height/2 + 检测距离
                groundLayer
            );
        }
    }
}