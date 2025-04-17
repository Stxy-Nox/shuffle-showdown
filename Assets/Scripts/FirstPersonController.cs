using UnityEngine;

namespace ShuffleShowdown
{
    public class FirstPersonController : MonoBehaviour
    {
        [Header("移动设置")]
        public float moveSpeed = 5.0f;
        public float jumpForce = 5.0f;
        
        [Header("视角设置")]
        public float mouseSensitivity = 2.0f;
        public Transform cameraTransform;
        public float maxLookAngle = 80f;
        
        [Header("地面检测")]
        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer = -1; // 默认为所有层

        private Rigidbody rb;
        private bool isGrounded;
        private float rotationX = 0f;
        private Vector3 moveDirection;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            
            // 锁定并隐藏光标
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // 防止玩家翻倒
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            
            // 如果未指定相机，使用当前附加的相机
            if (cameraTransform == null)
            {
                Camera attachedCamera = GetComponentInChildren<Camera>();
                if (attachedCamera != null)
                    cameraTransform = attachedCamera.transform;
            }
        }

        private void OnDestroy()
        {
            // 解锁光标
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            // 检测地面
            CheckGrounded();
            
            // 处理鼠标视角控制
            HandleLook();
            
            // 获取移动输入
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            // 计算相对于相机的移动方向
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            
            // 保持移动方向在水平面上
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            moveDirection = (forward * verticalInput + right * horizontalInput).normalized;
            
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

        private void HandleLook()
        {
            if (cameraTransform == null) return;
            
            // 获取鼠标输入
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // 垂直旋转（相机上下）
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            
            // 水平旋转（整个玩家左右）
            transform.Rotate(Vector3.up * mouseX);
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
            }
            else
            {
                // 停止水平移动，保留垂直速度
                rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
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