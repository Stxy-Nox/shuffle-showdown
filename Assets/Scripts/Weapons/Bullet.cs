using UnityEngine;

namespace ShuffleShowdown
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 direction;
        private float speed;
        private float damage;
        private float lifetime;
        private float startTime;
        
        public void Initialize(Vector3 dir, float spd, float dmg, float life)
        {
            direction = dir;
            speed = spd;
            damage = dmg;
            lifetime = life;
            startTime = Time.time;
            
            // 确保子弹不与玩家碰撞
            Physics.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
        }
        
        private void Update()
        {
            // 移动子弹
            transform.position += direction * speed * Time.deltaTime;
            
            // 检查生命周期
            if (Time.time - startTime >= lifetime)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // 检查是否击中敌人
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // 对敌人造成伤害
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                
                // 销毁子弹
                Destroy(gameObject);
            }
            // 如果击中其他障碍物（可选）
            else if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}