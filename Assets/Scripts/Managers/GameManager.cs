using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Preparation,
    Battle,
    Shopping,
    GameOver,
    Victory
}

public class GameManager : MonoBehaviour
{
    // 单例模式
    public static GameManager Instance { get; private set; }
    
    // 当前游戏状态
    public GameState CurrentState { get; private set; }
    
    // 当前波次
    public int CurrentWave { get; private set; } = 0;
    
    // 玩家金币
    public int PlayerCoins { get; private set; } = 0;
    
    // 玩家生命值
    public int PlayerHealth { get; private set; } = 100;
    public int PlayerMaxHealth { get; private set; } = 100;
    
    // 游戏状态变化事件
    public delegate void GameStateChangedHandler(GameState newState);
    public event GameStateChangedHandler OnGameStateChanged;
    
    // 波次变化事件
    public delegate void WaveChangedHandler(int newWave);
    public event WaveChangedHandler OnWaveChanged;
    
    // 金币变化事件
    public delegate void CoinsChangedHandler(int newCoins);
    public event CoinsChangedHandler OnCoinsChanged;
    
    // 生命值变化事件
    public delegate void HealthChangedHandler(int currentHealth, int maxHealth);
    public event HealthChangedHandler OnHealthChanged;
    
    private void Awake()
    {
        // 确保只有一个GameManager实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 初始状态设为主菜单
        SetGameState(GameState.MainMenu);
    }
    
    // 改变游戏状态
    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
        
        // 根据新状态执行对应逻辑
        switch (newState)
        {
            case GameState.MainMenu:
                // 主菜单逻辑
                break;
            case GameState.Battle:
                // 战斗开始逻辑
                break;
            case GameState.Shopping:
                // 商店逻辑
                break;
            case GameState.GameOver:
                // 游戏结束逻辑
                break;
            case GameState.Preparation:
                // 准备阶段逻辑
                break;
            case GameState.Victory:
                // 胜利逻辑
                break;
        }
    }
    
    // 开始游戏
    public void StartGame()
    {
        CurrentWave = 0;
        PlayerCoins = 0;
        PlayerHealth = PlayerMaxHealth;
        
        // 通知生命值变化
        OnHealthChanged?.Invoke(PlayerHealth, PlayerMaxHealth);
        
        // 通知金币变化
        OnCoinsChanged?.Invoke(PlayerCoins);
        
        // 进入准备阶段
        SetGameState(GameState.Preparation);
        
        // 开始第一波
        StartNextWave();
    }
    
    // 开始下一波
    public void StartNextWave()
    {
        CurrentWave++;
        OnWaveChanged?.Invoke(CurrentWave);
        
        // 从准备阶段进入战斗
        SetGameState(GameState.Battle);
    }
    
    // 结束当前波次
    public void EndCurrentWave()
    {
        // 战斗结束，进入商店
        SetGameState(GameState.Shopping);
    }
    
    // 修改金币
    public void ModifyCoins(int amount)
    {
        PlayerCoins += amount;
        OnCoinsChanged?.Invoke(PlayerCoins);
    }
    
    // 修改生命值
    public void ModifyHealth(int amount)
    {
        PlayerHealth = Mathf.Clamp(PlayerHealth + amount, 0, PlayerMaxHealth);
        OnHealthChanged?.Invoke(PlayerHealth, PlayerMaxHealth);
        
        // 检查玩家是否死亡
        if (PlayerHealth <= 0)
        {
            SetGameState(GameState.GameOver);
        }
    }
    
    // 设置最大生命值
    public void SetMaxHealth(int newMaxHealth)
    {
        PlayerMaxHealth = newMaxHealth;
        PlayerHealth = Mathf.Min(PlayerHealth, PlayerMaxHealth);
        OnHealthChanged?.Invoke(PlayerHealth, PlayerMaxHealth);
    }
    
    // 游戏结束
    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }
}