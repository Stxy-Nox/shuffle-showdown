using UnityEngine;

namespace ShuffleShowdown
{
    public class PlayerSetup : MonoBehaviour
    {
        [Header("组件引用")]
        public GameObject playerModel;
        
        [Header("预设值")]
        public Color playerColor = Color.blue;
        public float modelScale = 1.0f;
        
        private void Start()
        {
            SetupPlayerModel();
            SetupCamera();
            SetupWeaponSystem();
        }
        
        private void SetupPlayerModel()
        {
            if (playerModel == null)
            {
                // 如果没有指定模型，创建一个简单的圆盘作为临时模型
                playerModel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                playerModel.transform.SetParent(transform);
                playerModel.transform.localPosition = Vector3.zero;
                playerModel.transform.localScale = new Vector3(modelScale, 0.2f, modelScale);
                
                // 设置材质颜色
                Renderer renderer = playerModel.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material material = new Material(Shader.Find("Standard"));
                    material.color = playerColor;
                    renderer.material = material;
                }
            }
        }
        
        private void SetupCamera()
        {
            // 查找或创建主相机
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                // 创建相机
                GameObject cameraObj = new GameObject("Main Camera");
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.tag = "MainCamera";
                
                // 添加相机跟随脚本
                CameraFollow cameraFollow = cameraObj.AddComponent<CameraFollow>();
                cameraFollow.target = transform;
            }
            else
            {
                // 如果相机已存在，确保它有CameraFollow组件并设置目标
                CameraFollow cameraFollow = mainCamera.GetComponent<CameraFollow>();
                if (cameraFollow == null)
                {
                    cameraFollow = mainCamera.gameObject.AddComponent<CameraFollow>();
                }
                cameraFollow.target = transform;
            }
        }
        
        private void SetupWeaponSystem()
        {
            // 检查玩家是否已有武器管理器
            PlayerWeaponManager weaponManager = GetComponent<PlayerWeaponManager>();
            if (weaponManager == null)
            {
                // 添加武器管理器组件
                weaponManager = gameObject.AddComponent<PlayerWeaponManager>();
            }
            
            // 这里可以初始化武器管理器的其他设置
            // 比如设置武器挂载点，或者添加初始武器
        }
    }
}