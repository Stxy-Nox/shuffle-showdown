using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // 单例模式
    public static EventManager Instance { get; private set; }
    
    // 事件字典，用于存储事件名称和对应的委托
    private Dictionary<string, Action<object>> eventDictionary;
    
    private void Awake()
    {
        // 确保只有一个EventManager实例
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 初始化事件字典
        eventDictionary = new Dictionary<string, Action<object>>();
    }
    
    // 订阅事件
    public void Subscribe(string eventName, Action<object> listener)
    {
        // 检查事件是否已存在
        if (eventDictionary.ContainsKey(eventName))
        {
            // 添加监听器到已存在的事件
            eventDictionary[eventName] += listener;
        }
        else
        {
            // 创建新事件并添加监听器
            eventDictionary.Add(eventName, listener);
        }
    }
    
    // 取消订阅事件
    public void Unsubscribe(string eventName, Action<object> listener)
    {
        // 检查事件是否存在
        if (eventDictionary.ContainsKey(eventName))
        {
            // 移除监听器
            eventDictionary[eventName] -= listener;
            
            // 如果没有更多监听器，移除整个事件
            if (eventDictionary[eventName] == null)
            {
                eventDictionary.Remove(eventName);
            }
        }
    }
    
    // 触发事件
    public void TriggerEvent(string eventName, object data = null)
    {
        // 检查事件是否存在
        if (eventDictionary.ContainsKey(eventName))
        {
            // 调用所有注册的监听器
            eventDictionary[eventName]?.Invoke(data);
        }
    }
    
    // 常用事件名称常量
    public static class EventNames
    {
        public const string EnemyKilled = "EnemyKilled";
        public const string PlayerDamaged = "PlayerDamaged";
        public const string CoinCollected = "CoinCollected";
        public const string WaveStarted = "WaveStarted";
        public const string WaveCompleted = "WaveCompleted";
        public const string GameStarted = "GameStarted";
        public const string GameOver = "GameOver";
        public const string WeaponPurchased = "WeaponPurchased";
        public const string CardEquipped = "CardEquipped";
    }
}