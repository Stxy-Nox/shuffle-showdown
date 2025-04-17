using UnityEngine;

namespace ShuffleShowdown
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 5, -10);
        public float smoothSpeed = 5.0f;

        private void LateUpdate()
        {
            if (target == null)
                return;

            // 计算目标位置
            Vector3 desiredPosition = target.position + offset;
            
            // 平滑移动到目标位置
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            
            // 让相机看向玩家
            transform.LookAt(target);
        }
    }
}