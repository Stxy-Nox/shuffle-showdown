//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WaveManager : MonoBehaviour
//{
//    // 单例模式
//    public static WaveManager Instance { get; private set; }
    
//    // 波次配置列表
//    [SerializeField] private List<WaveConfig> waveConfigs = new List<WaveConfig>();
    
//    // 当前波次配置
//    private WaveConfig currentWaveConfig;
    
//    // 当前波次计时器
//    private float waveTimer = 0f;
    
//    // 当前波次已生成敌人数量
//    private int enemiesSpawned = 0;
    
//    // 当前波次仍活跃的敌人数量
//    private int activeEnemies = 0;
    
//    // 生成区域(相对于玩家的距离)
//    [SerializeField] private float spawnRadius = 15f;
    
//    // 生成位置(四周)
//    private Vector3[] spawnDirections = new Vector3[]
//    {
//        new Vector3(1, 0, 0), // 右
//        new Vector3(-1, 0, 0), // 左
//        new Vector3(0, 0, 1), // 上
//        new Vector3(0, 0, -1) // 下
//    };
    
//    // 当前是否在波次中
//    private bool isWaveActive = false;
    
//    // 当前波次是否已完成
//    private bool isWaveCompleted = false;
    
//    // Boss是否已生成
//    private bool bossSpawned = false;
    
//    private void Awake()
//    {
//        // 确保只有一个WaveManager实例
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
        
//        Instance = this;
//    }
    
//    private void Start()
//    {
//        // 注册游戏状态变化事件
//        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
//        GameManager.Instance.OnWaveChanged += OnWaveChanged;
        
//        // 注册敌人事件
//        EventManager.Instance.Subscribe(EventManager.EventNames.EnemyKilled, OnEnemyKilled);
//    }
    
//    private void OnDestroy()
//    {
//        // 取消注册事件
//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
//            GameManager.Instance.OnWaveChanged -= OnWaveChanged;
//        }
        
//        if (EventManager.Instance != null)
//        {
//            EventManager.Instance.Unsubscribe(EventManager.EventNames.EnemyKilled, OnEnemyKilled);
//        }
//    }
    
//    private void Update()
//    {
//        if (isWaveActive && !isWaveCompleted)
//        {
//            // 更新波次计时器
//            waveTimer += Time.deltaTime;
            
//            // 检查是否应该生成敌人
//            CheckEnemySpawn();
            
//            // 检查是否应该生成Boss
//            CheckBossSpawn();
            
//            // 检查波次是否结束
//            CheckWaveEnd();
//        }
//    }
    
//    private void OnGameStateChanged(GameState newState)
//    {
//        // 当游戏进入战斗状态时，激活波次
//        if (newState == GameState.Battle)
//        {
//            isWaveActive = true;
//            isWaveCompleted = false;
            
//            // 触发波次开始事件
//            EventManager.Instance.TriggerEvent(EventManager.EventNames.WaveStarted, GameManager.Instance.CurrentWave);
//        }
//        else
//        {
//            isWaveActive = false;
//        }
//    }
    
//    private void OnWaveChanged(int newWave)
//    {
//        // 重置波次数据
//        ResetWaveData();
        
//        // 获取当前波次配置
//        if (newWave <= waveConfigs.Count)
//        {
//            currentWaveConfig = waveConfigs[newWave - 1];
//        }
//        else
//        {
//            // 如果没有配置，使用最后一个配置并增加难度
//            currentWaveConfig = new WaveConfig();
//            if (waveConfigs.Count > 0)
//            {
//                WaveConfig lastConfig = waveConfigs[waveConfigs.Count - 1];
//                currentWaveConfig = lastConfig;
//                currentWaveConfig.waveNumber = newWave;
//                currentWaveConfig.enemyHealthMultiplier *= 1.2f; 
//                currentWaveConfig.enemyDamageMultiplier *= 1.1f;
//                currentWaveConfig.enemySpeedMultiplier *= 1.05f;
//                currentWaveConfig.maxEnemiesInWave += 5;
//            }
//        }
//    }
    
//    private void ResetWaveData()
//    {
//        waveTimer = 0f;
//        enemiesSpawned = 0;
//        activeEnemies = 0;
//        isWaveCompleted = false;
//        bossSpawned = false;
//    }
    
//    private void CheckEnemySpawn()
//    {
//        // 如果已达到最大敌人数量，不再生成
//        if (enemiesSpawned >= currentWaveConfig.maxEnemiesInWave)
//        {
//            return;
//        }
        
//        // 计算本帧是否应该生成敌人
//        float spawnProbability = currentWaveConfig.enemySpawnRate * Time.deltaTime;
        
//        if (Random.value < spawnProbability)
//        {
//            SpawnEnemy();
//        }
//    }
    
//    private void SpawnEnemy()
//    {
//        // 确保有敌人预制体可用
//        if (currentWaveConfig.enemyPrefabs == null || currentWaveConfig.enemyPrefabs.Length == 0)
//        {
//            Debug.LogWarning("没有设置敌人预制体!");
//            return;
//        }
        
//        // 获取随机敌人预制体
//        GameObject enemyPrefab = currentWaveConfig.enemyPrefabs[Random.Range(0, currentWaveConfig.enemyPrefabs.Length)];
        
//        // 注册预制体到对象池
//        ObjectPoolManager.Instance.RegisterPrefab(enemyPrefab.name, enemyPrefab);
        
//        // 获取玩家位置(假设有个Player标签)
//        GameObject player = GameObject.FindGameObjectWithTag("Player");
//        if (player == null)
//        {
//            Debug.LogWarning("没有找到玩家!");
//            return;
//        }
        
//        // 计算生成位置(玩家周围)
//        Vector3 playerPos = player.transform.position;
//        Vector3 spawnDir = spawnDirections[Random.Range(0, spawnDirections.Length)];
//        Vector3 spawnPos = playerPos + spawnDir * spawnRadius;
        
//        // 添加随机偏移
//        spawnPos += new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        
//        // 从对象池获取敌人
//        GameObject enemy = ObjectPoolManager.Instance.GetObjectFromPool(enemyPrefab.name, spawnPos, Quaternion.identity);
        
//        // 设置敌人属性(根据波次)
//        Enemy enemyComponent = enemy.GetComponent<Enemy>();
//        if (enemyComponent != null)
//        {
//            enemyComponent.SetMultipliers(
//                currentWaveConfig.enemyHealthMultiplier,
//                currentWaveConfig.enemyDamageMultiplier,
//                currentWaveConfig.enemySpeedMultiplier
//            );
//        }
        
//        // 增加计数
//        enemiesSpawned++;
//        activeEnemies++;
//    }
    
//    private void CheckBossSpawn()
//    {
//        // 如果已经生成Boss或此波没有Boss，返回
//        if (bossSpawned || !currentWaveConfig.hasBoss || currentWaveConfig.bossPrefab == null)
//        {
//            return;
//        }
        
//        // 检查是否达到Boss生成时间点
//        if (waveTimer >= currentWaveConfig.waveDuration * currentWaveConfig.bossSpawnTimePercent)
//        {
//            SpawnBoss();
//        }
//    }

//    private void SpawnBoss()
//    {
//        // 获取玩家位置
//        GameObject player = GameObject.FindGameObjectWithTag("Player");
//        if (player == null)
//        {
//            Debug.LogWarning("没有找到玩家!");
//            return;
//        }
        
//        // 计算Boss生成位置(玩家前方)
//        Vector3 playerPos = player.transform.position;
//        Vector3 spawnDir = spawnDirections[Random.Range(0, spawnDirections.Length)];
//        Vector3 spawnPos = playerPos + spawnDir * spawnRadius;
        
//        // 注册Boss预制体到对象池
//        ObjectPoolManager.Instance.RegisterPrefab(currentWaveConfig.bossPrefab.name, currentWaveConfig.bossPrefab);
        
//        // 生成Boss
//        GameObject boss = ObjectPoolManager.Instance.GetObjectFromPool(currentWaveConfig.bossPrefab.name, spawnPos, Quaternion.identity);
        
//        // 设置Boss属性
//        Enemy bossComponent = boss.GetComponent<Enemy>();
//        if (bossComponent != null)
//        {
//            bossComponent.SetMultipliers(
//                currentWaveConfig.enemyHealthMultiplier * 2f, // Boss生命值加倍
//                currentWaveConfig.enemyDamageMultiplier * 1.5f, // Boss伤害提高50%
//                currentWaveConfig.enemySpeedMultiplier * 0.8f  // Boss速度降低20%(一般Boss较慢但血厚)
//            );
//        }
        
//        // 标记Boss已生成
//        bossSpawned = true;
//        activeEnemies++;
//    }
    
//    private void CheckWaveEnd()
//    {
//        // 检查波次是否结束(时间到或敌人全部消灭)
//        bool timeUp = waveTimer >= currentWaveConfig.waveDuration;
//        bool allEnemiesSpawned = enemiesSpawned >= currentWaveConfig.maxEnemiesInWave;
//        bool allEnemiesKilled = allEnemiesSpawned && activeEnemies <= 0;
        
//        if ((timeUp && allEnemiesKilled) || (!currentWaveConfig.hasBoss && timeUp))
//        {
//            CompleteWave();
//        }
//        else if (currentWaveConfig.hasBoss && bossSpawned && activeEnemies <= 0)
//        {
//            // 如果有Boss且Boss已被击败(没有活跃敌人)
//            CompleteWave();
//        }
//    }
    
//    private void CompleteWave()
//    {
//        if (!isWaveCompleted)
//        {
//            isWaveCompleted = true;
            
//            // 触发波次完成事件
//            EventManager.Instance.TriggerEvent(EventManager.EventNames.WaveCompleted, GameManager.Instance.CurrentWave);
            
//            // 通知GameManager波次结束
//            GameManager.Instance.EndCurrentWave();
//        }
//    }
    
//    private void OnEnemyKilled(object data)
//    {
//        // 减少活跃敌人计数
//        activeEnemies = Mathf.Max(0, activeEnemies - 1);
//    }
//}