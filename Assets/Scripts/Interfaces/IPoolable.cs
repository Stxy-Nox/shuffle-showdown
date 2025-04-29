using UnityEngine;

namespace ShuffleShowdown
{
    // 对象池中的对象需要实现此接口
    public interface IPoolable
    {
        // 当对象从池中取出时调用
        void OnObjectSpawn();
        
        // 当对象回收到池中时调用
        void OnObjectDespawn();
        
        // 添加 OnSpawn 和 OnDespawn 方法，兼容 ObjectPoolManager
        void OnSpawn();
        void OnDespawn();
    }
}