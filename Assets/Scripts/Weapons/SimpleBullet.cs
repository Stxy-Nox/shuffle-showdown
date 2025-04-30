using UnityEngine;

namespace ShuffleShowdown
{
    public class SimpleBullet : MonoBehaviour, IPoolable
    {
        private Vector3 direction;
        private float speed = 10f;
        private float damage = 10f; // 增加伤害值以便更容易看到效果
        private float lifetime = 3f;
        private float startTime;
        private bool isActive = false;

        public void Initialize(Vector3 dir, float spd, float dmg, float life)
        {
            direction = dir;
            speed = spd;
            damage = dmg;
            lifetime = life;
            startTime = Time.time;
            isActive = true;
        }

        private void Update()
        {
            if (!isActive) return;

            // 移动子弹
            transform.position += direction * speed * Time.deltaTime;

            // 检查生命周期
            if (Time.time - startTime >= lifetime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive) return;

            // 检查是否击中敌人
            if (other.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // 对敌人造成伤害
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Debug.Log("子弹击中敌人，造成 " + damage + " 点伤害");
                }
                else
                {
                    // 如果没有Enemy组件，尝试直接获取EnemyHealth组件
                    EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damage);
                        Debug.Log("子弹直接对敌人健康组件造成 " + damage + " 点伤害");
                    }
                    else
                    {
                        Debug.LogWarning("子弹击中了敌人，但找不到Enemy或EnemyHealth组件");
                    }
                }

                // 返回对象池
                ReturnToPool();
            }
        }

        // 将子弹返回对象池
        private void ReturnToPool()
        {
            if (SimpleObjectPool.Instance != null)
            {
                isActive = false;
                SimpleObjectPool.Instance.ReturnToPool(gameObject);
            }
            else
            {
                // 如果没有对象池，直接销毁
                Destroy(gameObject);
            }
        }

        // IPoolable接口实现
        public void OnObjectSpawn()
        {
            // 重置子弹状态
            isActive = true;
            startTime = Time.time;
        }

        public void OnObjectDespawn()
        {
            // 清理子弹状态
            isActive = false;
        }
        
        // 额外的IPoolable实现
        public void OnSpawn()
        {
            // 调用现有方法保持一致性
            OnObjectSpawn();
        }
        
        public void OnDespawn()
        {
            // 调用现有方法保持一致性
            OnObjectDespawn();
        }
    }
}