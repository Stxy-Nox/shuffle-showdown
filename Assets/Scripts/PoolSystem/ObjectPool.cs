//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObjectPool : MonoBehaviour
//{
//    // 单例模式
//    public static ObjectPool Instance { get; private set; }

//    // 对象池字典
//    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    
//    // 对象池预设及初始大小
//    [System.Serializable]
//    public class Pool
//    {
//        public string tag;
//        public GameObject prefab;
//        public int initialSize;
//    }
    
//    public List<Pool> pools;

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
//        // 初始化对象池
//        InitializePools();
//    }

//    // 初始化所有对象池
//    private void InitializePools()
//    {
//        foreach (Pool pool in pools)
//        {
//            // 创建该预制体的对象池
//            Queue<GameObject> objectPool = new Queue<GameObject>();
            
//            // 预先实例化对象
//            for (int i = 0; i < pool.initialSize; i++)
//            {
//                GameObject obj = Instantiate(pool.prefab);
//                obj.SetActive(false);
//                obj.transform.SetParent(transform);
//                objectPool.Enqueue(obj);
//            }
            
//            // 添加到对象池字典
//            poolDictionary.Add(pool.tag, objectPool);
//        }
//    }

//    // 从对象池中获取对象
//    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
//    {
//        // 检查对象池是否存在
//        if (!poolDictionary.ContainsKey(tag))
//        {
//            Debug.LogWarning("对象池标签 " + tag + " 不存在!");
//            return null;
//        }
        
//        // 获取对象池队列
//        Queue<GameObject> objectPool = poolDictionary[tag];
        
//        // 如果队列为空，创建新对象
//        if (objectPool.Count == 0)
//        {
//            // 找到对应的预制体
//            Pool targetPool = pools.Find(p => p.tag == tag);
//            if (targetPool != null)
//            {
//                GameObject newObj = Instantiate(targetPool.prefab);
//                newObj.transform.SetParent(transform);
//                return ConfigurePooledObject(newObj, position, rotation);
//            }
//            else
//            {
//                Debug.LogError("无法找到预制体: " + tag);
//                return null;
//            }
//        }
        
//        // 从队列中取出对象
//        GameObject objectToSpawn = objectPool.Dequeue();
        
//        // 配置对象
//        return ConfigurePooledObject(objectToSpawn, position, rotation);
//    }

//    // 配置池化对象
//    private GameObject ConfigurePooledObject(GameObject obj, Vector3 position, Quaternion rotation)
//    {
//        // 重新激活对象
//        obj.SetActive(true);
//        obj.transform.position = position;
//        obj.transform.rotation = rotation;
        
//        // 获取可池化接口
//        IPoolable poolableObject = obj.GetComponent<IPoolable>();
//        if (poolableObject != null)
//        {
//            poolableObject.OnObjectSpawn();
//        }
        
//        return obj;
//    }

//    // 将对象返回池中
//    public void ReturnToPool(string tag, GameObject objectToReturn)
//    {
//        if (!poolDictionary.ContainsKey(tag))
//        {
//            Debug.LogWarning("对象池标签 " + tag + " 不存在!");
//            return;
//        }
        
//        // 重置对象
//        objectToReturn.SetActive(false);
        
//        // 返回到对象池
//        poolDictionary[tag].Enqueue(objectToReturn);
//    }
//}