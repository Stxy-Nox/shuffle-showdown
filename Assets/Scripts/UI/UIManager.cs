using UnityEngine;
using UnityEngine.UI;

namespace ShuffleShowdown
{
    public class UIManager : MonoBehaviour
    {
        // 单例模式
        public static UIManager Instance { get; private set; }
        
        [Header("血条设置")]
        public Image healthBarFill;        // 血条填充图像
        public Text healthText;            // 血量文本

        private PlayerHealth playerHealth;  // 玩家健康组件引用

        private void Awake()
        {
            // 单例初始化
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("UIManager单例初始化");
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            // 查找血条UI元素
            if (healthBarFill == null)
            {
                GameObject fillObj = GameObject.Find("HealthBarFill");
                if (fillObj != null)
                {
                    healthBarFill = fillObj.GetComponent<Image>();
                    Debug.Log("自动查找到血条填充: " + (healthBarFill != null));
                }
            }
            
            if (healthText == null)
            {
                GameObject textObj = GameObject.Find("HealthText");
                if (textObj != null)
                {
                    healthText = textObj.GetComponent<Text>();
                    Debug.Log("自动查找到血条文本: " + (healthText != null));
                }
            }
            
            // 查找玩家健康组件
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    // 初始化血条
                    Debug.Log("找到玩家健康组件，初始血量: " + playerHealth.currentHealth);
                    UpdateHealthUI(playerHealth.currentHealth, playerHealth.maxHealth);
                }
                else
                {
                    Debug.LogError("找不到玩家的PlayerHealth组件!");
                }
            }
            else
            {
                Debug.LogError("找不到玩家对象!");
            }
        }

        private void Update()
        {
            // 如果有玩家健康组件，每帧更新UI
            if (playerHealth != null)
            {
                UpdateHealthUI(playerHealth.currentHealth, playerHealth.maxHealth);
            }
        }

        // 更新血条UI
        public void UpdateHealthUI(int currentHealth, int maxHealth)
        {
            Debug.Log("UIManager更新UI: " + currentHealth + "/" + maxHealth + ", UI元素存在: " + 
                    (healthBarFill != null) + ", " + (healthText != null));
                    
            if (healthBarFill != null)
            {
                // 更新血条填充比例
                float fillAmount = (float)currentHealth / maxHealth;
                healthBarFill.fillAmount = fillAmount;
                
                // 根据血量百分比改变血条颜色（可选）
                if (fillAmount <= 0.3f)
                {
                    // 低血量时变成橙色
                    healthBarFill.color = new Color(1f, 0.5f, 0f);
                }
                else
                {
                    // 正常血量时为绿色
                    healthBarFill.color = new Color(0.2f, 0.9f, 0.2f);
                }
            }
            
            if (healthText != null)
            {
                // 更新血量文本
                healthText.text = $"{currentHealth}/{maxHealth}";
            }
        }
    }
}