//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class StoreManager : MonoBehaviour
//{
//    // 单例模式
//    public static StoreManager Instance { get; private set; }
    
//    [Header("商店设置")]
//    public int itemsPerRefresh = 6;                  // 每次刷新的商品数量
//    public List<CardWeaponBase> availableCards;      // 可用卡牌池
//    public CardWeaponBase[] currentStoreItems;       // 当前商店物品
    
//    [Header("刷新设置")]
//    public int refreshCost = 5;                      // 刷新商店的花费
    
//    // 商店相关事件
//    public delegate void StoreEventHandler();
//    public event StoreEventHandler OnStoreOpened;
//    public event StoreEventHandler OnStoreClosed;
//    public event StoreEventHandler OnStoreRefreshed;
    
//    // UI 管理器引用
//    private StoreUI storeUI;
    
//    private void Awake()
//    {
//        // 确保单例实例唯一性
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }
//    }
    
//    private void Start()
//    {
//        // 初始化商店
//        InitializeStore();
        
//        // 查找UI管理器
//        storeUI = FindObjectOfType<StoreUI>();
//    }
    
//    // 初始化商店
//    private void InitializeStore()
//    {
//        // 初始化当前商店物品数组
//        currentStoreItems = new CardWeaponBase[itemsPerRefresh];
        
//        // 初始刷新商店
//        RefreshStore();
//    }
    
//    // 打开商店
//    public void OpenStore()
//    {
//        // 通知UI显示商店
//        if (storeUI != null)
//        {
//            storeUI.ShowStoreUI();
//        }
        
//        // 触发事件
//        OnStoreOpened?.Invoke();
//    }
    
//    // 关闭商店
//    public void CloseStore()
//    {
//        // 通知UI隐藏商店
//        if (storeUI != null)
//        {
//            storeUI.HideStoreUI();
//        }
        
//        // 触发事件
//        OnStoreClosed?.Invoke();
        
//        // 通知GameManager进入下一波
//        GameManager.Instance.ChangeGameState(GameManager.GameState.InGame);
//    }
    
//    // 刷新商店
//    public void RefreshStore()
//    {
//        // 随机选择卡牌填充商店
//        for (int i = 0; i < currentStoreItems.Length; i++)
//        {
//            // 如果可用卡牌池非空
//            if (availableCards.Count > 0)
//            {
//                // 随机选择一张卡牌
//                int randomIndex = Random.Range(0, availableCards.Count);
//                currentStoreItems[i] = availableCards[randomIndex];
//            }
//            else
//            {
//                currentStoreItems[i] = null;
//            }
//        }
        
//        // 通知UI更新
//        if (storeUI != null)
//        {
//            storeUI.UpdateStoreUI();
//        }
        
//        // 触发事件
//        OnStoreRefreshed?.Invoke();
//    }
    
//    // 尝试刷新商店（需要花费）
//    public bool TryRefreshStore()
//    {
//        // 检查玩家是否有足够的金币
//        if (GameManager.Instance.UseCoins(refreshCost))
//        {
//            RefreshStore();
//            return true;
//        }
//        return false;
//    }
    
//    // 购买商店物品
//    public bool PurchaseItem(int itemIndex)
//    {
//        // 检查索引是否有效
//        if (itemIndex < 0 || itemIndex >= currentStoreItems.Length)
//        {
//            Debug.LogWarning("无效的商店物品索引: " + itemIndex);
//            return false;
//        }
        
//        // 获取商品
//        CardWeaponBase item = currentStoreItems[itemIndex];
        
//        // 检查商品是否存在
//        if (item == null)
//        {
//            Debug.LogWarning("尝试购买空商品");
//            return false;
//        }
        
//        // 检查玩家是否有足够的金币
//        if (GameManager.Instance.UseCoins(item.price))
//        {
//            // 玩家获得物品
//            // 这里我们只是把卡牌加入玩家的可用卡牌列表
//            // 实际上可能需要调用WeaponSystem的某个方法
            
//            // 移除商店中的该物品
//            currentStoreItems[itemIndex] = null;
            
//            // 更新UI
//            if (storeUI != null)
//            {
//                storeUI.UpdateStoreUI();
//            }
            
//            return true;
//        }
        
//        return false;
//    }
    
//    // 获取当前商店物品列表
//    public CardWeaponBase[] GetCurrentStoreItems()
//    {
//        return currentStoreItems;
//    }
    
//    // 获取商店刷新价格
//    public int GetRefreshCost()
//    {
//        return refreshCost;
//    }
//}