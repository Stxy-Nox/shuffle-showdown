//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class StoreUI : MonoBehaviour
//{
//    [Header("UI引用")]
//    public GameObject storePanel;                        // 商店面板
//    public Button closeButton;                           // 关闭按钮
//    public Button refreshButton;                         // 刷新按钮
//    public TextMeshProUGUI playerCoinsText;              // 玩家金币文本
//    public TextMeshProUGUI refreshCostText;              // 刷新花费文本
    
//    [Header("商品UI")]
//    public Transform itemsContainer;                      // 商品容器
//    public GameObject itemPrefab;                         // 商品预制体
    
//    [Header("武器槽UI")]
//    public Transform weaponSlotsContainer;                // 武器槽容器
//    public GameObject weaponSlotPrefab;                   // 武器槽预制体
    
//    // 商品UI项列表
//    private List<StoreItemUI> storeItemUIs = new List<StoreItemUI>();
    
//    // 武器槽UI项列表
//    private List<WeaponSlotUI> weaponSlotUIs = new List<WeaponSlotUI>();
    
//    // 选中的卡牌和槽位
//    private CardWeaponBase selectedCard;
//    private int selectedSlotIndex = -1;
    
//    private void Start()
//    {
//        // 初始化UI
//        InitializeUI();
        
//        // 隐藏商店UI
//        HideStoreUI();
//    }
    
//    // 初始化UI
//    private void InitializeUI()
//    {
//        // 设置关闭按钮事件
//        if (closeButton != null)
//        {
//            closeButton.onClick.AddListener(OnCloseButtonClicked);
//        }
        
//        // 设置刷新按钮事件
//        if (refreshButton != null)
//        {
//            refreshButton.onClick.AddListener(OnRefreshButtonClicked);
//        }
        
//        // 创建武器槽UI
//        CreateWeaponSlotUIs();
        
//        // 创建商品UI
//        CreateStoreItemUIs();
        
//        // 更新刷新花费文本
//        UpdateRefreshCostText();
//    }
    
//    // 创建武器槽UI
//    private void CreateWeaponSlotUIs()
//    {
//        if (weaponSlotsContainer == null || weaponSlotPrefab == null) return;
        
//        // 清空容器
//        foreach (Transform child in weaponSlotsContainer)
//        {
//            Destroy(child.gameObject);
//        }
//        weaponSlotUIs.Clear();
        
//        // 获取武器系统
//        WeaponSystem weaponSystem = WeaponSystem.Instance;
//        if (weaponSystem == null) return;
        
//        // 创建武器槽UI
//        for (int i = 0; i < weaponSystem.GetWeaponSlotCount(); i++)
//        {
//            GameObject slotObj = Instantiate(weaponSlotPrefab, weaponSlotsContainer);
//            WeaponSlotUI slotUI = slotObj.GetComponent<WeaponSlotUI>();
            
//            if (slotUI != null)
//            {
//                // 设置槽位索引
//                slotUI.SetSlotIndex(i);
                
//                // 设置卡牌数据
//                CardWeaponBase card = weaponSystem.GetCardInSlot(i);
//                slotUI.SetCardData(card);
                
//                // 添加点击事件
//                int index = i; // 为闭包捕获变量
//                slotUI.OnSlotClicked = () => OnWeaponSlotClicked(index);
                
//                // 添加到列表
//                weaponSlotUIs.Add(slotUI);
//            }
//        }
//    }
    
//    // 创建商品UI
//    private void CreateStoreItemUIs()
//    {
//        if (itemsContainer == null || itemPrefab == null) return;
        
//        // 清空容器
//        foreach (Transform child in itemsContainer)
//        {
//            Destroy(child.gameObject);
//        }
//        storeItemUIs.Clear();
        
//        // 获取商店管理器
//        StoreManager storeManager = StoreManager.Instance;
//        if (storeManager == null) return;
        
//        // 获取当前商店物品
//        CardWeaponBase[] storeItems = storeManager.GetCurrentStoreItems();
        
//        // 创建商品UI
//        for (int i = 0; i < storeItems.Length; i++)
//        {
//            GameObject itemObj = Instantiate(itemPrefab, itemsContainer);
//            StoreItemUI itemUI = itemObj.GetComponent<StoreItemUI>();
            
//            if (itemUI != null)
//            {
//                // 设置商品索引
//                itemUI.SetItemIndex(i);
                
//                // 设置卡牌数据
//                itemUI.SetCardData(storeItems[i]);
                
//                // 添加点击事件
//                int index = i; // 为闭包捕获变量
//                itemUI.OnItemClicked = () => OnStoreItemClicked(index);
                
//                // 添加到列表
//                storeItemUIs.Add(itemUI);
//            }
//        }
//    }
    
//    // 更新商店UI
//    public void UpdateStoreUI()
//    {
//        // 更新玩家金币文本
//        UpdatePlayerCoinsText();
        
//        // 更新商品UI
//        UpdateStoreItemUIs();
        
//        // 更新武器槽UI
//        UpdateWeaponSlotUIs();
//    }
    
//    // 更新商品UI
//    private void UpdateStoreItemUIs()
//    {
//        StoreManager storeManager = StoreManager.Instance;
//        if (storeManager == null) return;
        
//        // 获取当前商店物品
//        CardWeaponBase[] storeItems = storeManager.GetCurrentStoreItems();
        
//        // 更新商品UI
//        for (int i = 0; i < storeItemUIs.Count && i < storeItems.Length; i++)
//        {
//            storeItemUIs[i].SetCardData(storeItems[i]);
//        }
//    }
    
//    // 更新武器槽UI
//    private void UpdateWeaponSlotUIs()
//    {
//        WeaponSystem weaponSystem = WeaponSystem.Instance;
//        if (weaponSystem == null) return;
        
//        // 更新武器槽UI
//        for (int i = 0; i < weaponSlotUIs.Count; i++)
//        {
//            CardWeaponBase card = weaponSystem.GetCardInSlot(i);
//            weaponSlotUIs[i].SetCardData(card);
//        }
//    }
    
//    // 更新玩家金币文本
//    private void UpdatePlayerCoinsText()
//    {
//        if (playerCoinsText == null) return;
        
//        GameManager gameManager = GameManager.Instance;
//        if (gameManager == null) return;
        
//        playerCoinsText.text = "金币: " + gameManager.PlayerCoins;
//    }
    
//    // 更新刷新花费文本
//    private void UpdateRefreshCostText()
//    {
//        if (refreshCostText == null) return;
        
//        StoreManager storeManager = StoreManager.Instance;
//        if (storeManager == null) return;
        
//        refreshCostText.text = "刷新: " + storeManager.GetRefreshCost() + " 金币";
//    }
    
//    // 显示商店UI
//    public void ShowStoreUI()
//    {
//        if (storePanel != null)
//        {
//            storePanel.SetActive(true);
//        }
        
//        // 更新UI显示
//        UpdateStoreUI();
//    }
    
//    // 隐藏商店UI
//    public void HideStoreUI()
//    {
//        if (storePanel != null)
//        {
//            storePanel.SetActive(false);
//        }
        
//        // 清除选中状态
//        ClearSelection();
//    }
    
//    // 商店物品点击事件
//    private void OnStoreItemClicked(int itemIndex)
//    {
//        StoreManager storeManager = StoreManager.Instance;
//        if (storeManager == null) return;
        
//        // 获取商品
//        CardWeaponBase card = storeManager.GetCurrentStoreItems()[itemIndex];
        
//        // 如果商品为空，不处理
//        if (card == null) return;
        
//        // 如果已经选择了一个槽位，直接购买并放入
//        if (selectedSlotIndex >= 0)
//        {
//            // 尝试购买
//            if (storeManager.PurchaseItem(itemIndex))
//            {
//                // 购买成功，放入选中的槽位
//                WeaponSystem.Instance.SetCardInSlot(selectedSlotIndex, card);
                
//                // 更新UI
//                UpdateStoreUI();
                
//                // 清除选中状态
//                ClearSelection();
//            }
//        }
//        else
//        {
//            // 如果之前已经选中了卡牌，先清除选中状态
//            if (selectedCard != null)
//            {
//                ClearSelection();
//            }
            
//            // 设置选中的卡牌
//            selectedCard = card;
            
//            // 更新UI显示选中状态
//            storeItemUIs[itemIndex].SetSelected(true);
//        }
//    }
    
//    // 武器槽点击事件
//    private void OnWeaponSlotClicked(int slotIndex)
//    {
//        // 获取武器系统
//        WeaponSystem weaponSystem = WeaponSystem.Instance;
//        if (weaponSystem == null) return;
        
//        // 如果已经选择了一个卡牌，直接购买并放入
//        if (selectedCard != null)
//        {
//            // 获取商店管理器
//            StoreManager storeManager = StoreManager.Instance;
//            if (storeManager == null) return;
            
//            // 查找商店中对应的物品索引
//            CardWeaponBase[] storeItems = storeManager.GetCurrentStoreItems();
//            int itemIndex = -1;
            
//            for (int i = 0; i < storeItems.Length; i++)
//            {
//                if (storeItems[i] == selectedCard)
//                {
//                    itemIndex = i;
//                    break;
//                }
//            }
            
//            if (itemIndex >= 0)
//            {
//                // 尝试购买
//                if (storeManager.PurchaseItem(itemIndex))
//                {
//                    // 购买成功，放入选中的槽位
//                    weaponSystem.SetCardInSlot(slotIndex, selectedCard);
                    
//                    // 更新UI
//                    UpdateStoreUI();
                    
//                    // 清除选中状态
//                    ClearSelection();
//                }
//            }
//        }
//        else
//        {
//            // 如果之前已经选中了槽位，先清除选中状态
//            if (selectedSlotIndex >= 0)
//            {
//                ClearSelection();
//            }
            
//            // 设置选中的槽位
//            selectedSlotIndex = slotIndex;
            
//            // 更新UI显示选中状态
//            weaponSlotUIs[slotIndex].SetSelected(true);
//        }
//    }
    
//    // 关闭按钮点击事件
//    private void OnCloseButtonClicked()
//    {
//        // 关闭商店
//        StoreManager.Instance.CloseStore();
//    }
    
//    // 刷新按钮点击事件
//    private void OnRefreshButtonClicked()
//    {
//        // 尝试刷新商店
//        if (StoreManager.Instance.TryRefreshStore())
//        {
//            // 刷新成功，更新UI
//            UpdateStoreUI();
            
//            // 清除选中状态
//            ClearSelection();
//        }
//    }
    
//    // 清除选中状态
//    private void ClearSelection()
//    {
//        // 清除选中的卡牌
//        selectedCard = null;
        
//        // 清除选中的槽位
//        selectedSlotIndex = -1;
        
//        // 更新UI显示
//        foreach (StoreItemUI itemUI in storeItemUIs)
//        {
//            itemUI.SetSelected(false);
//        }
        
//        foreach (WeaponSlotUI slotUI in weaponSlotUIs)
//        {
//            slotUI.SetSelected(false);
//        }
//    }
//}