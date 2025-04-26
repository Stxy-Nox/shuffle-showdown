using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShuffleShowdown
{
    public class WaveManager : MonoBehaviour
    {
        // 单例模式
        public static WaveManager Instance { get; private set; }

        [Header("波次设置")]
        public float waveStartDelay = 3f;         // 波次开始前的延迟
        public float enemySpawnInterval = 1.5f;   // 敌人生成间隔
        public float waveDuration = 60f;          // 每波持续时间
        public int baseEnemiesPerWave = 10;       // 基础每波敌人数量
        public float enemyCountScaling = 1.5f;    // 敌人数量随波次增长系数

        [Header("敌人设置")]
        public GameObject basicEnemyPrefab;       // 基础敌人预制体
        public GameObject eliteEnemyPrefab;       // 精英敌人预制体
        public EnemySpawner enemySpawner;         // 敌人生成器引用

        // 波次状态
        public enum WaveState
        {
            Preparing,
            Active,
            Complete
        }

        // 当前波次状态
        public WaveState CurrentWaveState { get; private set; } = WaveState.Preparing;

        // 波次事件委托
        public delegate void WaveStateChangedHandler(WaveState newState);
        public event WaveStateChangedHandler OnWaveStateChanged;

        // 当前波次信息
        private int currentWaveNumber = 0;
        private int enemiesRemaining = 0;
        private int enemiesSpawned = 0;
        private int totalEnemiesForWave = 0;
        private float waveTimer = 0f;
        private bool isWaveActive = false;

        private void Awake()
        {
            // 确保单例实例唯一性
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {
            if (isWaveActive)
            {
                waveTimer += Time.deltaTime;
                
                // 如果超过波次时间，结束当前波次
                if (waveTimer >= waveDuration)
                {
                    CompleteWave();
                }
            }
        }

        // 开始新的波次
        public void StartWave(int waveNumber)
        {
            currentWaveNumber = waveNumber;
            SetWaveState(WaveState.Preparing);
            
            // 计算本波次敌人总数
            totalEnemiesForWave = Mathf.RoundToInt(baseEnemiesPerWave * Mathf.Pow(enemyCountScaling, waveNumber - 1));
            enemiesRemaining = totalEnemiesForWave;
            enemiesSpawned = 0;
            
            // 延迟后开始生成敌人
            StartCoroutine(StartWaveAfterDelay());
        }

        // 延迟后开始波次
        private IEnumerator StartWaveAfterDelay()
        {
            yield return new WaitForSeconds(waveStartDelay);
            
            SetWaveState(WaveState.Active);
            isWaveActive = true;
            waveTimer = 0f;
            
            // 开始生成敌人
            StartCoroutine(SpawnEnemiesForWave());
        }

        // 生成波次敌人
        private IEnumerator SpawnEnemiesForWave()
        {
            while (enemiesSpawned < totalEnemiesForWave && isWaveActive)
            {
                SpawnEnemy();
                enemiesSpawned++;
                
                yield return new WaitForSeconds(enemySpawnInterval);
            }
        }

        // 生成单个敌人
        private void SpawnEnemy()
        {
            // 使用组件引用而不是静态实例
            if (enemySpawner != null)
            {
                // 根据波次难度决定是否生成精英敌人
                bool spawnElite = Random.value < (currentWaveNumber * 0.05f); // 5%几率每波递增
                
                GameObject enemyPrefab = spawnElite && eliteEnemyPrefab != null ? eliteEnemyPrefab : basicEnemyPrefab;
                
                if (enemyPrefab != null)
                {
                    // 使用组件方法而非静态实例方法
                    // enemySpawner.SpawnRandomEnemy(enemyPrefab, currentWaveNumber);
                    
                    // 临时解决方案：直接生成敌人
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
        
        // 获取随机生成位置的辅助方法
        private Vector3 GetRandomSpawnPosition()
        {
            // 获取玩家位置（假设场景中只有一个Player标签的对象）
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 playerPos = player != null ? player.transform.position : Vector3.zero;
            
            // 在玩家周围一定距离随机生成
            float spawnDistance = Random.Range(10f, 15f);
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            
            Vector3 spawnPos = playerPos + new Vector3(
                Mathf.Cos(angle) * spawnDistance,
                0f,
                Mathf.Sin(angle) * spawnDistance
            );
            
            return spawnPos;
        }

        // 敌人被击败
        public void EnemyDefeated()
        {
            enemiesRemaining--;
            
            // 检查是否所有敌人都被击败
            if (enemiesRemaining <= 0 && enemiesSpawned >= totalEnemiesForWave)
            {
                CompleteWave();
            }
        }

        // 完成当前波次
        private void CompleteWave()
        {
            if (isWaveActive)
            {
                isWaveActive = false;
                SetWaveState(WaveState.Complete);
                
                // 通知GameManager波次结束
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.EndCurrentWave();
                }
            }
        }

        // 设置波次状态
        private void SetWaveState(WaveState newState)
        {
            CurrentWaveState = newState;
            OnWaveStateChanged?.Invoke(newState);
        }

        // 获取当前波次信息
        public int GetCurrentWaveNumber()
        {
            return currentWaveNumber;
        }

        public int GetRemainingEnemies()
        {
            return enemiesRemaining;
        }

        public float GetWaveTimeRemaining()
        {
            return Mathf.Max(0, waveDuration - waveTimer);
        }
    }
}