using UnityEngine;

namespace ShuffleShowdown
{
    public class InputTester : MonoBehaviour
    {
        public bool showDebug = true;
        public GUIStyle debugStyle;
        
        private string debugInfo = "";
        private float updateInterval = 0.5f;
        private float lastUpdateTime;
        
        private void Start()
        {
            if (debugStyle == null)
            {
                debugStyle = new GUIStyle();
                debugStyle.fontSize = 20;
                debugStyle.normal.textColor = Color.white;
            }
            
            Debug.Log("输入测试器已启动");
        }
        
        private void Update()
        {
            if (Time.time - lastUpdateTime > updateInterval)
            {
                // 获取输入
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                float horizontalRaw = Input.GetAxisRaw("Horizontal");
                float verticalRaw = Input.GetAxisRaw("Vertical");
                
                debugInfo = $"平滑输入: ({horizontal:F2}, {vertical:F2})\n" +
                            $"原始输入: ({horizontalRaw:F2}, {verticalRaw:F2})\n" +
                            $"按键状态:\n" +
                            $"W: {Input.GetKey(KeyCode.W)}, A: {Input.GetKey(KeyCode.A)}, S: {Input.GetKey(KeyCode.S)}, D: {Input.GetKey(KeyCode.D)}\n" +
                            $"↑: {Input.GetKey(KeyCode.UpArrow)}, ←: {Input.GetKey(KeyCode.LeftArrow)}, ↓: {Input.GetKey(KeyCode.DownArrow)}, →: {Input.GetKey(KeyCode.RightArrow)}";
                
                lastUpdateTime = Time.time;
            }
        }
        
        private void OnGUI()
        {
            if (showDebug)
            {
                GUI.Label(new Rect(10, 10, 500, 200), debugInfo, debugStyle);
            }
        }
    }
}