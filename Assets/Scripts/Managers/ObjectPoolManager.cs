//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectPoolManager : MonoBehaviour
//{
//    // 单例模式
//    public static ObjectPoolManager Instance { get; private set; }
    
//    // 对象池字典，每个预制体对应一个对象池
//    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
//    // 对象池父物体
//    private Transform poolParent;
    
//    // 预制体字典，用于存储预制体引用
//    private Dictionary<string, GameObject> prefabDictionary;
    
//    private void Awake()
//    {
//        // 确保只有一个ObjectPoolManager实例
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
        
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
        
//        // 初始化字典和父物体
//        poolDictionary = new Dictionary<string, Queue<GameObject>>();
//        prefabDictionary = new Dictionary<string, GameObject>();
        
//        // 创建对象池父物体
//        poolParent = new GameObject("ObjectPool").transform;
//        poolParent.SetParent(transform);
//    }
    
//    // 注册预制体
//    public void RegisterPrefab(string poolName, GameObject prefab)
//    {
//        if (!prefabDictionary.ContainsKey(poolName))
//        {
//            prefabDictionary.Add(poolName, prefab);
//        }
//    }
    
//    // 预热对象池(提前创建指定数量的对象)
//    public void PreWarmPool(string poolName, int count)
//    {
//        if (!prefabDictionary.ContainsKey(poolName))
//        {
//            Debug.LogError($"预制体 {poolName} 未注册!");
//            return;
//        }
        
//        if (!poolDictionary.ContainsKey(poolName))
//        {
//            poolDictionary.Add(poolName, new Queue<GameObject>());
            
//            // 创建池子对应的父物体
//            GameObject poolContainer = new GameObject(poolName + "Pool");
//            poolContainer.transform.SetParent(poolParent);
            
//            // 创建指定数量的对象并放入池中
//            for (int i = 0; i < count; i++)
//            {
//                GameObject obj = Instantiate(prefabDictionary[poolName]);
//                obj.name = prefabDictionary[poolName].name + "_" + i;
//                obj.transform.SetParent(poolContainer.transform);
//                obj.SetActive(false);
//                poolDictionary[poolName].Enqueue(obj);
//            }
//        }
//    }
    
//    // 从对象池获取对象
//    public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation)
//    {
//        // 检查预制体是否注册
//        if (!prefabDictionary.ContainsKey(poolName))
//        {
//            Debug.LogError($"预制体 {poolName} 未注册!");
//            return null;
//        }
        
//        // 检查对象池是否存在，如果不存在则创建
//        if (!poolDictionary.ContainsKey(poolName))
//        {
//            poolDictionary.Add(poolName, new Queue<GameObject>());
            
//            // 创建池子对应的父物体
//            GameObject poolContainer = new GameObject(poolName + "Pool");
//            poolContainer.transform.SetParent(poolParent);
//        }
        
//        GameObject obj;
        
//        // 如果池中有对象，从池中取出
//        if (poolDictionary[poolName].Count > 0)
//        {
//            obj = poolDictionary[poolName].Dequeue();
//        }
//        // 否则创建新对象
//        else
//        {
//            obj = Instantiate(prefabDictionary[poolName]);
//            obj.name = prefabDictionary[poolName].name + "_" + Random.Range(1000, 9999);
//            obj.transform.SetParent(GameObject.Find(poolName + "Pool")?.transform ?? poolParent);
//        }
        
//        // 设置对象位置和旋转
//        obj.transform.position = position;
//        obj.transform.rotation = rotation;
        
//        // 激活对象
//        obj.SetActive(true);
        
//        // 如果对象有IPoolable接口，调用OnSpawn方法
//        IPoolable poolable = obj.GetComponent<IPoolable>();
//        poolable?.OnSpawn();
        
//        return obj;
//    }
    
//    // 回收对象到池中
//    public void ReturnObjectToPool(GameObject obj)
//    {
//        // 获取对象名称前缀(不包含_和后面的数字)
//        string objName = obj.name;
//        int underscoreIndex = objName.LastIndexOf('_');
//        if (underscoreIndex > 0)
//        {
//            objName = objName.Substring(0, underscoreIndex);
//        }
        
//        // 如果对象有IPoolable接口，调用OnDespawn方法
//        IPoolable poolable = obj.GetComponent<IPoolable>();
//        poolable?.OnDespawn();
        
//        // 停用对象
//        obj.SetActive(false);
        
//        // 将对象放回对应的池中
//        if (poolDictionary.ContainsKey(objName))
//        {
//            poolDictionary[objName].Enqueue(obj);
//        }
//        else
//        {
//            Debug.LogWarning($"对象 {obj.name} 没有对应的对象池，无法回收");
//            Destroy(obj);
//        }
//    }
    
//    // 清空所有对象池
//    public void ClearAllPools()
//    {
//        foreach (var pool in poolDictionary.Values)
//        {
//            while (pool.Count > 0)
//            {
//                GameObject obj = pool.Dequeue();
//                Destroy(obj);
//            }
//        }
        
//        poolDictionary.Clear();
//    }
    
//    // 清空指定对象池
//    public void ClearPool(string poolName)
//    {
//        if (poolDictionary.ContainsKey(poolName))
//        {
//            while (poolDictionary[poolName].Count > 0)
//            {
//                GameObject obj = poolDictionary[poolName].Dequeue();
//                Destroy(obj);
//            }
            
//            poolDictionary.Remove(poolName);
//        }
//    }
//}