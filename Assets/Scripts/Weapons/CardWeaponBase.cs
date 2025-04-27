using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardWeapon", menuName = "Weapons/Card Weapon", order = 1)]
public class CardWeaponBase : ScriptableObject
{
    [Header("基本信息")]
    public string cardName;           // 卡牌名称
    public Sprite cardIcon;           // 卡牌图标
    [TextArea]
    public string description;        // 卡牌描述
    public int price;                 // 卡牌价格

    [Header("效果参数")]
    public CardEffect[] cardEffects;  // 卡牌效果数组

    [System.Serializable]
    public class CardEffect
    {
        public EffectType effectType;     // 效果类型
        public float effectValue;         // 效果数值
        public bool isPercentage;         // 是否为百分比数值
    }

    // 效果类型枚举
    public enum EffectType
    {
        Damage,             // 伤害
        FireRate,           // 射速
        BulletSize,         // 子弹大小
        BulletSpeed,        // 子弹速度
        MaxHealth,          // 最大生命值
        MovementSpeed,      // 移动速度
        CriticalChance,     // 暴击几率
        CriticalMultiplier, // 暴击倍率
        Range,              // 攻击范围
        Penetration,        // 穿透数
        // 可以添加更多效果类型
    }

    // 应用卡牌效果到武器参数
    public void ApplyCardEffect(WeaponParameters parameters)
    {
        foreach (CardEffect effect in cardEffects)
        {
            ApplySingleEffect(parameters, effect);
        }
    }

    // 应用单个效果
    private void ApplySingleEffect(WeaponParameters parameters, CardEffect effect)
    {
        float value = effect.effectValue;
        
        // 如果是百分比，转换为乘数
        if (effect.isPercentage)
        {
            value = value / 100f;
        }
        
        // 根据效果类型修改武器参数
        switch (effect.effectType)
        {
            case EffectType.Damage:
                if (effect.isPercentage)
                    parameters.damage *= (1 + value);
                else
                    parameters.damage += value;
                break;
                
            case EffectType.FireRate:
                if (effect.isPercentage)
                    parameters.fireRate *= (1 + value);
                else
                    parameters.fireRate += value;
                break;
                
            case EffectType.BulletSize:
                if (effect.isPercentage)
                    parameters.bulletSize *= (1 + value);
                else
                    parameters.bulletSize += value;
                break;
                
            case EffectType.BulletSpeed:
                if (effect.isPercentage)
                    parameters.bulletSpeed *= (1 + value);
                else
                    parameters.bulletSpeed += value;
                break;
                
            case EffectType.MaxHealth:
                if (effect.isPercentage)
                    parameters.maxHealthModifier *= (1 + value);
                else
                    parameters.maxHealthModifier += value;
                break;
                
            case EffectType.MovementSpeed:
                if (effect.isPercentage)
                    parameters.movementSpeedModifier *= (1 + value);
                else
                    parameters.movementSpeedModifier += value;
                break;
                
            case EffectType.CriticalChance:
                if (effect.isPercentage)
                    parameters.criticalChance *= (1 + value);
                else
                    parameters.criticalChance += value;
                break;
                
            case EffectType.CriticalMultiplier:
                if (effect.isPercentage)
                    parameters.criticalMultiplier *= (1 + value);
                else
                    parameters.criticalMultiplier += value;
                break;
                
            case EffectType.Range:
                if (effect.isPercentage)
                    parameters.range *= (1 + value);
                else
                    parameters.range += value;
                break;
                
            case EffectType.Penetration:
                if (effect.isPercentage)
                    parameters.penetration = Mathf.RoundToInt(parameters.penetration * (1 + value));
                else
                    parameters.penetration += Mathf.RoundToInt(value);
                break;
        }
    }
    
    // 获取格式化的效果描述
    public string GetFormattedEffectDescription()
    {
        string result = "";
        
        foreach (CardEffect effect in cardEffects)
        {
            string effectName = GetEffectName(effect.effectType);
            string sign = effect.effectValue >= 0 ? "+" : "";
            string percentSign = effect.isPercentage ? "%" : "";
            
            result += $"{effectName}: {sign}{effect.effectValue}{percentSign}\n";
        }
        
        return result;
    }
    
    // 根据效果类型获取名称
    private string GetEffectName(EffectType type)
    {
        switch (type)
        {
            case EffectType.Damage: return "伤害";
            case EffectType.FireRate: return "射速";
            case EffectType.BulletSize: return "子弹大小";
            case EffectType.BulletSpeed: return "子弹速度";
            case EffectType.MaxHealth: return "最大生命值";
            case EffectType.MovementSpeed: return "移动速度";
            case EffectType.CriticalChance: return "暴击几率";
            case EffectType.CriticalMultiplier: return "暴击倍率";
            case EffectType.Range: return "攻击范围";
            case EffectType.Penetration: return "穿透数";
            default: return type.ToString();
        }
    }
}