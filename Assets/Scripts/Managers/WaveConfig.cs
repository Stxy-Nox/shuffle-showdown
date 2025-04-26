using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveConfig
{
    public int waveNumber;
    public float waveDuration = 60f; // 波次持续时间(秒)
    public float enemySpawnRate = 1f; // 敌人生成速率(每秒)
    public int maxEnemiesInWave = 30; // 此波最大敌人数量
    public GameObject[] enemyPrefabs; // 此波可生成的敌人预制体
    public float enemyHealthMultiplier = 1f; // 敌人生命值倍率
    public float enemyDamageMultiplier = 1f; // 敌人伤害倍率
    public float enemySpeedMultiplier = 1f; // 敌人速度倍率
    public float bossSpawnTimePercent = 0.8f; // 在波次多少比例的时间点生成Boss(0-1)
    public GameObject bossPrefab; // Boss预制体
    public bool hasBoss = false; // 此波是否有Boss
}