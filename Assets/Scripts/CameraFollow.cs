using UnityEngine;

namespace ShuffleShowdown
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("跟随设置")]
        public Transform target;
        public Vector3 offset = new Vector3(0, 15, 0); // 向上偏移，实现完全俯视效果
        public float smoothSpeed = 10.0f; // 更快的跟随速度
        
        [Header("视角设置")]
        public float cameraAngle = 90f; // 完全俯视角度
        public float minZoom = 6f;
        public float maxZoom = 12f;
        public float zoomSpeed = 2f;
        public float currentZoom = 8f; // 默认缩放值
        
        [Header("边界设置")]
        public bool useBoundaries = false;
        public float minX = -50f;
        public float maxX = 50f; 
        public float minZ = -50f;
        public float maxZ = 50f;
        
        [Header("视野设置")]
        public bool dynamicFieldOfView = true;
        public float minFOV = 50f;
        public float maxFOV = 70f;
        
        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                cam = Camera.main;
            }
        }

        private void Start()
        {
            // 设置完全俯视
            transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            // 处理缩放输入
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            currentZoom = Mathf.Clamp(currentZoom - scrollInput * zoomSpeed, minZoom, maxZoom);
            
            // 调整FOV来实现视野变化
            if (dynamicFieldOfView && cam != null)
            {
                float zoomRatio = Mathf.InverseLerp(minZoom, maxZoom, currentZoom);
                cam.fieldOfView = Mathf.Lerp(maxFOV, minFOV, zoomRatio);
            }
            
            // 计算目标位置
            Vector3 targetPosition = target.position;
            Vector3 desiredPosition = targetPosition + new Vector3(0, currentZoom, 0);
            
            // 应用平滑
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            
            // 如果使用边界，确保相机不会超出边界
            if (useBoundaries)
            {
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
                smoothedPosition.z = Mathf.Clamp(smoothedPosition.z, minZ, maxZ);
            }
            
            // 应用位置
            transform.position = smoothedPosition;
            
            // 保持俯视角
            transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
        }
    }
}