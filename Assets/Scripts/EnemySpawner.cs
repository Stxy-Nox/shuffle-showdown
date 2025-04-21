using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ShuffleShowdown
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("生成设置")]
        public float spawnInterval = 1.0f;
        public int maxEnemies = 50;
        public float minSpawnDistance = 10f; // 距离玩家的最小生成距离
        public float maxSpawnDistance = 20f; // 距离玩家的最大生成距离
        
        [Header("地图边界")]
        public float mapWidth = 50f;
        public float mapHeight = 50f;
        
        [Header("敌人预制件")]
        public GameObject enemyPrefab;
        public List<GameObject> enemyVariants = new List<GameObject>(); // 可选的敌人变种
        
        [Header("难度设置")]
        public float difficultyScalingFactor = 0.95f; // 小于1意味着随着时间推移生成间隔会缩短
        public float gameDuration = 300f; // 游戏持续时间（秒）
        public float initialDifficulty = 1.0f;
        public float maxDifficulty = 3.0f;
        
        private Transform playerTransform;
        private float currentSpawnInterval;
        private float gameTimer = 0f;
        private List<GameObject> activeEnemies = new List<GameObject>();
        private int enemyLayer;
        
        private void Start()
        {
            // 获取Enemy层索引
            enemyLayer = LayerMask.NameToLayer("Enemy");
            
            // 查找玩家
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("找不到玩家对象！请确保玩家已标记为'Player'标签。");
                enabled = false;
                return;
            }
            
            // 检查是否有敌人预制件
            if (enemyPrefab == null && (enemyVariants == null || enemyVariants.Count == 0))
            {
                Debug.LogError("没有设置敌人预制件！请在Inspector中分配敌人预制件。");
                enabled = false;
                return;
            }
            
            // 初始化生成间隔
            currentSpawnInterval = spawnInterval;
            
            // 开始生成敌人
            StartCoroutine(SpawnEnemies());
        }
        
        private void Update()
        {
            // 更新游戏计时器
            gameTimer += Time.deltaTime;
            
            // 计算当前难度（从初始难度到最大难度的线性插值）
            float normalizedTime = Mathf.Clamp01(gameTimer / gameDuration);
            float currentDifficulty = Mathf.Lerp(initialDifficulty, maxDifficulty, normalizedTime);
            
            // 调整生成间隔
            currentSpawnInterval = spawnInterval / currentDifficulty;
            
            // 移除已销毁的敌人
            activeEnemies.RemoveAll(enemy => enemy == null);
        }
        
        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                // 检查是否达到敌人数量上限
                if (activeEnemies.Count < maxEnemies)
                {
                    SpawnEnemy();
                }
                
                // 等待下一次生成
                yield return new WaitForSeconds(currentSpawnInterval);
            }
        }
        
        private void SpawnEnemy()
        {
            if (playerTransform == null)
                return;
            
            // 在玩家周围的随机位置生成敌人（环形区域）
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
            
            // 计算生成位置
            Vector3 spawnPosition = playerTransform.position + new Vector3(
                Mathf.Cos(angle) * distance,
                0f,
                Mathf.Sin(angle) * distance
            );
            
            // 限制在地图边界内
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -mapWidth/2, mapWidth/2);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, -mapHeight/2, mapHeight/2);
            
            // 选择一个敌人预制件
            GameObject prefabToSpawn;
            if (enemyVariants.Count > 0 && Random.Range(0, 100) < 30) // 30%几率生成变种敌人
            {
                prefabToSpawn = enemyVariants[Random.Range(0, enemyVariants.Count)];
            }
            else
            {
                prefabToSpawn = enemyPrefab;
            }
            
            // 创建敌人
            GameObject enemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            
            // 确保敌人在正确的层上
            enemy.layer = enemyLayer;
            
            // 确保所有子对象都在Enemy层上
            foreach (Transform child in enemy.transform)
            {
                child.gameObject.layer = enemyLayer;
            }
            
            // 确保碰撞体是触发器
            Collider[] colliders = enemy.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                col.isTrigger = true;
            }
            
            // 确保刚体是运动学的
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            
            // 设置敌人目标
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.target = playerTransform;
            }
            
            // 添加到活跃敌人列表
            activeEnemies.Add(enemy);
        }
        
        // 在编辑器中可视化生成区域（辅助调试）
        private void OnDrawGizmos()
        {
            if (playerTransform != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(playerTransform.position, minSpawnDistance);
                
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(playerTransform.position, maxSpawnDistance);
            }
            
            // 绘制地图边界
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapWidth, 1, mapHeight));
        }
    }
}