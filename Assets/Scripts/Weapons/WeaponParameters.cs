using UnityEngine;

// 武器参数类，用于存储武器配置
[System.Serializable]
public class WeaponParameters
{
    // 武器基础参数
    public float damage = 10f;                // 基础伤害
    public float fireRate = 1f;               // 射击频率 (每秒射击次数)
    public float bulletSize = 1f;             // 子弹大小
    public float bulletSpeed = 10f;           // 子弹速度
    public float range = 10f;                 // 射程范围
    public int penetration = 0;               // 穿透数量 (0表示不穿透)
    
    // 玩家属性修饰符
    public float maxHealthModifier = 0f;        // 最大生命值修饰符
    public float movementSpeedModifier = 0f;    // 移动速度修饰符
    
    // 暴击相关
    public float criticalChance = 0.05f;        // 暴击几率 (0.05 = 5%)
    public float criticalMultiplier = 1.5f;     // 暴击伤害倍率 (1.5 = 150% 伤害)
    
    // 默认构造函数
    public WeaponParameters() { }
    
    // 深拷贝
    public WeaponParameters Clone()
    {
        return new WeaponParameters
        {
            damage = this.damage,
            fireRate = this.fireRate,
            bulletSize = this.bulletSize,
            bulletSpeed = this.bulletSpeed,
            range = this.range,
            penetration = this.penetration,
            maxHealthModifier = this.maxHealthModifier,
            movementSpeedModifier = this.movementSpeedModifier,
            criticalChance = this.criticalChance,
            criticalMultiplier = this.criticalMultiplier
        };
    }
    
    // 重置为默认值
    public void Reset()
    {
        damage = 10f;
        fireRate = 1f;
        bulletSize = 1f;
        bulletSpeed = 10f;
        range = 10f;
        penetration = 0;
        maxHealthModifier = 0f;
        movementSpeedModifier = 0f;
        criticalChance = 0.05f;
        criticalMultiplier = 1.5f;
    }
}