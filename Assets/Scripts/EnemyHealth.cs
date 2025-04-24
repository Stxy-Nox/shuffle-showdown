using UnityEngine;
using System.Collections;

namespace ShuffleShowdown
{
    public class EnemyHealth : MonoBehaviour
    {
        [Header("生命值设置")]
        public float maxHealth = 10f;
        public float currentHealth;
        
        [Header("视觉反馈")]
        public bool flashOnDamage = true;
        public float flashDuration = 0.1f;
        public Color damageColor = Color.red;
        public Color defaultColor = Color.white;
        
        private Renderer[] renderers;
        private Enemy enemyComponent;
        
        private void Start()
        {
            // 初始化生命值
            currentHealth = maxHealth;
            
            // 获取敌人组件
            enemyComponent = GetComponent<Enemy>();
            
            // 获取所有渲染器组件（用于受伤闪烁效果）
            renderers = GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                // 如果没有在子对象中找到渲染器，则尝试获取当前对象的渲染器
                Renderer mainRenderer = GetComponent<Renderer>();
                if (mainRenderer != null)
                {
                    renderers = new Renderer[] { mainRenderer };
                }
            }
            
            Debug.Log("敌人健康组件初始化完成，找到 " + renderers.Length + " 个渲染器");
            
            // 设置默认颜色为白色
            SetDefaultColor();
        }
        
        // 设置默认颜色
        private void SetDefaultColor()
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null && renderer.material != null && renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = defaultColor;
                    Debug.Log("设置敌人默认颜色为: " + defaultColor);
                }
                else if (renderer != null)
                {
                    Debug.LogWarning("敌人渲染器没有_Color属性");
                }
            }
        }
        
        // 当敌人受到伤害时调用
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            Debug.Log("敌人健康组件受到 " + amount + " 点伤害，剩余血量: " + currentHealth);
            
            // 受伤视觉反馈
            if (flashOnDamage)
            {
                StartCoroutine(FlashDamage());
            }
            
            // 检查是否死亡
            if (currentHealth <= 0)
            {
                Die();
            }
        }
        
        private IEnumerator FlashDamage()
        {
            Debug.Log("敌人开始闪烁受击效果");
            // 改变所有材质颜色为伤害颜色
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null && renderer.material != null && renderer.material.HasProperty("_Color"))
                {
                    renderer.material.color = damageColor;
                    Debug.Log("设置敌人伤害颜色为: " + damageColor);
                }
            }
            
            // 等待闪烁时间
            yield return new WaitForSeconds(flashDuration);
            
            // 恢复原始颜色
            SetDefaultColor();
        }
        
        // 敌人死亡
        private void Die()
        {
            Debug.Log("敌人健康组件触发死亡");
            // 如果enemyComponent存在，调用其Die方法处理死亡逻辑
            if (enemyComponent != null && enemyComponent.health > 0)
            {
                enemyComponent.health = 0; // 确保Enemy脚本中的health也为0
                enemyComponent.Die(); // 直接调用Enemy中的Die方法
            }
            else
            {
                // 如果没有Enemy组件，直接销毁对象
                Destroy(gameObject);
            }
        }
    }
}