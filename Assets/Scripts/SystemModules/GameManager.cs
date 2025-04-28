using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShuffleShowdown
{
    public class GameManager : MonoBehaviour
    {
        // 单例模式
        public static GameManager Instance { get; private set; }

        // 游戏状态枚举
        public enum GameState
        {
            Loading,
            MainMenu,
            InGame,
            Shopping,
            Paused,
            GameOver
        }

        // 当前游戏状态
        public GameState CurrentState { get; private set; }

        // 当前波次
        public int CurrentWave { get; private set; } = 0;

        // 玩家金币
        public int PlayerCoins { get; private set; } = 0;

        // 游戏事件委托
        public delegate void GameStateChangedHandler(GameState newState);
        public event GameStateChangedHandler OnGameStateChanged;

        public delegate void WaveChangedHandler(int newWave);
        public event WaveChangedHandler OnWaveChanged;

        public delegate void PlayerCoinsChangedHandler(int newCoins);
        public event PlayerCoinsChangedHandler OnPlayerCoinsChanged;

        private void Awake()
        {
            // 确保单例实例唯一性
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // 初始化游戏状态
            ChangeGameState(GameState.Loading);
        }

        private void Start()
        {
            // 游戏初始化完成后切换到主菜单
            ChangeGameState(GameState.MainMenu);
        }

        // 切换游戏状态
        public void ChangeGameState(GameState newState)
        {
            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
            
            // 根据新状态执行相应的逻辑
            switch (newState)
            {
                case GameState.InGame:
                    StartNewWave();
                    break;
                case GameState.GameOver:
                    // 游戏结束逻辑
                    break;
            }
        }

        // 开始新的一波
        public void StartNewWave()
        {
            CurrentWave++;
            OnWaveChanged?.Invoke(CurrentWave);
            
            // 通知波次管理器开始生成敌人
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.StartWave(CurrentWave);
            }
        }

        // 当前波次结束
        public void EndCurrentWave()
        {
            // 波次结束，切换到商店状态
            ChangeGameState(GameState.Shopping);
        }

        // 开始游戏
        public void StartGame()
        {
            // 重置游戏数据
            CurrentWave = 0;
            PlayerCoins = 0;
            OnPlayerCoinsChanged?.Invoke(PlayerCoins);
            
            // 切换到游戏状态
            ChangeGameState(GameState.InGame);
        }

        // 添加金币
        public void AddCoins(int amount)
        {
            PlayerCoins += amount;
            OnPlayerCoinsChanged?.Invoke(PlayerCoins);
        }

        // 使用金币
        public bool UseCoins(int amount)
        {
            if (PlayerCoins >= amount)
            {
                PlayerCoins -= amount;
                OnPlayerCoinsChanged?.Invoke(PlayerCoins);
                return true;
            }
            return false;
        }

        // 游戏暂停
        public void PauseGame()
        {
            if (CurrentState == GameState.InGame)
            {
                ChangeGameState(GameState.Paused);
                Time.timeScale = 0;
            }
        }

        // 恢复游戏
        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused)
            {
                ChangeGameState(GameState.InGame);
                Time.timeScale = 1;
            }
        }

        // 游戏结束
        public void GameOver()
        {
            Debug.Log("游戏结束");
            ChangeGameState(GameState.GameOver);
            
            // PlayerHealth 脚本会负责场景切换，这里只负责状态管理
            // 如果需要立即切换场景，可以取消下面的注释
            // SceneManager.LoadScene("GameOver");
        }
    }
}