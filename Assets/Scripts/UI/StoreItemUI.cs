using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreItemUI : MonoBehaviour
{
    [Header("UI组件")]
    public Image cardImage;                  // 卡牌图片
    public TextMeshProUGUI cardNameText;     // 卡牌名称文本
    public TextMeshProUGUI cardPriceText;    // 卡牌价格文本
    public TextMeshProUGUI cardDescText;     // 卡牌描述文本
    public GameObject selectedIndicator;     // 选中指示器
    
    [Header("样式")]
    public Color normalColor = Color.white;  // 正常颜色
    public Color selectedColor = Color.yellow; // 选中颜色
    
    // 商品索引
    private int itemIndex = -1;
    
    // 卡牌数据
    private CardWeaponBase cardData;
    
    // 点击回调
    public System.Action OnItemClicked;
    
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
    
    // 设置商品索引
    public void SetItemIndex(int index)
    {
        itemIndex = index;
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
            
            if (cardNameText != null)
            {
                cardNameText.text = card.cardName;
            }
            
            if (cardPriceText != null)
            {
                cardPriceText.text = card.price + " 金币";
            }
            
            if (cardDescText != null)
            {
                cardDescText.text = card.description;
            }
            
            // 启用整个UI项
            gameObject.SetActive(true);
        }
        else
        {
            // 卡牌为空，禁用UI项
            if (cardImage != null)
            {
                cardImage.enabled = false;
            }
            
            if (cardNameText != null)
            {
                cardNameText.text = "";
            }
            
            if (cardPriceText != null)
            {
                cardPriceText.text = "";
            }
            
            if (cardDescText != null)
            {
                cardDescText.text = "";
            }
            
            // 禁用整个UI项
            gameObject.SetActive(false);
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
        OnItemClicked?.Invoke();
    }
}