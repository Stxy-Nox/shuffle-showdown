using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSlotUI : MonoBehaviour
{
    [Header("UI组件")]
    public Image cardImage;                  // 卡牌图片
    public TextMeshProUGUI slotIndexText;    // 槽位索引文本
    public GameObject selectedIndicator;     // 选中指示器
    public GameObject emptySlotIndicator;    // 空槽位指示器
    
    [Header("样式")]
    public Color normalColor = Color.white;  // 正常颜色
    public Color selectedColor = Color.yellow; // 选中颜色
    
    // 槽位索引
    private int slotIndex = -1;
    
    // 卡牌数据
    private CardWeaponBase cardData;
    
    // 点击回调
    public System.Action OnSlotClicked;
    
    private void Start()
    {
        // 添加点击事件
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(HandleClick);
        }
        
        // 初始化选中状态
        SetSelected(false);
    }
    
    // 设置槽位索引
    public void SetSlotIndex(int index)
    {
        slotIndex = index;
        
        // 更新索引文本
        if (slotIndexText != null)
        {
            slotIndexText.text = "槽位 " + (index + 1);
        }
    }
    
    // 设置卡牌数据
    public void SetCardData(CardWeaponBase card)
    {
        cardData = card;
        
        if (card != null)
        {
            // 设置UI显示
            if (cardImage != null)
            {
                cardImage.sprite = card.cardIcon;
                cardImage.enabled = true;
            }
            
            // 隐藏空槽位指示器
            if (emptySlotIndicator != null)
            {
                emptySlotIndicator.SetActive(false);
            }
        }
        else
        {
            // 卡牌为空，更新UI
            if (cardImage != null)
            {
                cardImage.enabled = false;
            }
            
            // 显示空槽位指示器
            if (emptySlotIndicator != null)
            {
                emptySlotIndicator.SetActive(true);
            }
        }
    }
    
    // 设置选中状态
    public void SetSelected(bool selected)
    {
        if (selectedIndicator != null)
        {
            selectedIndicator.SetActive(selected);
        }
        
        // 更新颜色
        Image background = GetComponent<Image>();
        if (background != null)
        {
            background.color = selected ? selectedColor : normalColor;
        }
    }
    
    // 点击处理
    private void HandleClick()
    {
        // 调用回调
        OnSlotClicked?.Invoke();
    }
}