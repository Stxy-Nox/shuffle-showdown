using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
                return;
            
            currentHealth -= damage;
            
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
            
            // 可以触发游戏结束画面，禁用玩家控制等
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            
            // 例如，你可以调用游戏管理器处理游戏结束
            // GameManager.Instance.GameOver();
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
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = damageColor;
                }
            }
            
            // 等待闪烁时间
            yield return new WaitForSeconds(flashDuration);
            
            // 恢复原始颜色
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    renderers[i].material.color = originalColors[i];
                }
            }
        }
        
        private void UpdateHealthUI()
        {
            // 如果有血条UI，更新它
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
            
            // 如果有血量文本UI，更新它
            if (healthText != null)
            {
                healthText.text = currentHealth + " / " + maxHealth;
            }
        }
    }
}