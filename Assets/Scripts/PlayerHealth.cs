using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ShuffleShowdown
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("生命值设置")]
        public int maxHealth = 100;
        public int currentHealth;
        
        [Header("无敌时间")]
        public float invincibilityTime = 1.0f;
        public bool isInvincible = false;
        
        [Header("视觉反馈")]
        public bool flashOnDamage = true;
        public float flashDuration = 0.1f;
        public Color damageColor = Color.red;
        
        [Header("UI")]
        public Slider healthSlider;
        public Text healthText;
        
        private Renderer[] renderers;
        private Color[] originalColors;
        
        private void Start()
        {
            // 初始化生命值
            currentHealth = maxHealth;
            Debug.Log("初始化玩家血量: " + currentHealth);
            UpdateHealthUI();
            
            // 获取所有渲染器组件（用于受伤闪烁效果）
            renderers = GetComponentsInChildren<Renderer>();
            originalColors = new Color[renderers.Length];
            
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    originalColors[i] = renderers[i].material.color;
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            // 如果玩家处于无敌状态，不受伤害
            if (isInvincible)
            {
                Debug.Log("玩家处于无敌状态，不受到伤害");
                return;
            }
            
            currentHealth -= damage;
            Debug.Log("玩家受到 " + damage + " 点伤害，当前血量: " + currentHealth);
            
            // 限制最小生命值为0
            currentHealth = Mathf.Max(0, currentHealth);
            
            // 更新UI
            UpdateHealthUI();
            
            // 受伤视觉反馈
            if (flashOnDamage)
            {
                StartCoroutine(FlashDamage());
            }
            
            // 进入短暂无敌状态
            StartCoroutine(BecomeInvincible());
            
            // 检查是否死亡
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        public void Heal(int amount)
        {
            currentHealth += amount;
            
            // 限制最大生命值
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            
            // 更新UI
            UpdateHealthUI();
        }
        
        private void Die()
        {
            // 游戏结束逻辑
            Debug.Log("玩家死亡");
            
            // 禁用玩家控制
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            
            // 调用GameManager处理游戏结束
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
                Debug.Log("调用GameManager.GameOver()");
            }
            
            // 延迟加载游戏结束场景
            StartCoroutine(LoadGameOverScene());
        }
        
        private IEnumerator LoadGameOverScene()
        {
            // 等待短暂时间让死亡效果显示
            yield return new WaitForSeconds(1.5f);
            
            // 加载游戏结束场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            Debug.Log("加载GameOver场景");
        }
        
        private IEnumerator BecomeInvincible()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;
        }
        
        private IEnumerator FlashDamage()
        {
            // 改变所有材质颜色为伤害颜色
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].material != null && renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = damageColor;
                }
            }
            
            // 等待闪烁时间
            yield return new WaitForSeconds(flashDuration);
            
            // 恢复原始颜色
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].material != null && renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = originalColors[i];
                }
            }
        }
        
        private void UpdateHealthUI()
        {
            Debug.Log("更新血量UI，当前血量: " + currentHealth + "/" + maxHealth);
            
            // 使用旧版UI系统更新（如果有设置）
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
                Debug.Log("更新HealthSlider: " + currentHealth);
            }
            else
            {
                Debug.LogWarning("HealthSlider未设置");
            }
            
            if (healthText != null)
            {
                healthText.text = currentHealth + " / " + maxHealth;
                Debug.Log("更新HealthText: " + healthText.text);
            }
            else
            {
                Debug.LogWarning("HealthText未设置");
            }
            
            // 使用UIManager更新
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateHealthUI(currentHealth, maxHealth);
                Debug.Log("通过UIManager更新UI");
            }
            else
            {
                Debug.LogWarning("UIManager.Instance为空");
            }
        }
    }
}