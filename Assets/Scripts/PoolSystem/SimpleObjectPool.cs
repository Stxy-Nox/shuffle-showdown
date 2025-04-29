using System.Collections.Generic;
using UnityEngine;

namespace ShuffleShowdown
{
    public class SimpleObjectPool : MonoBehaviour
    {
        // 单例模式
        public static SimpleObjectPool Instance { get; private set; }

        // 对象池字典
        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        // 对象池根节点
        private Transform poolRoot;

        private void Awake()
        {
            // 单例初始化
            if (Instance == null)
            {
                Instance = this;
                // 不要在场景加载时销毁
                DontDestroyOnLoad(gameObject);
                
                // 创建对象池根节点
                poolRoot = new GameObject("ObjectPoolRoot").transform;
                poolRoot.SetParent(transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // 注册预制件
        public void RegisterPrefab(string poolName, GameObject prefab)
        {
            if (!prefabDictionary.ContainsKey(poolName))
            {
                prefabDictionary.Add(poolName, prefab);
            }
        }

        // 预热对象池 - 提前创建对象
        public void PreWarm(string poolName, int count)
        {
            if (!prefabDictionary.ContainsKey(poolName))
            {
                Debug.LogError($"对象池预热失败：未注册预制件 {poolName}");
                return;
            }

            // 确保对象池存在
            if (!poolDictionary.ContainsKey(poolName))
            {
                poolDictionary.Add(poolName, new Queue<GameObject>());
                
                // 创建池专用的父节点
                GameObject poolContainerObj = new GameObject(poolName + "Pool");
                poolContainerObj.transform.SetParent(poolRoot);
            }

            // 创建指定数量的对象
            GameObject prefab = prefabDictionary[poolName];
            Transform poolContainer = poolRoot.Find(poolName + "Pool");

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = prefab.name + "_" + i;
                obj.transform.SetParent(poolContainer);
                obj.SetActive(false);
                poolDictionary[poolName].Enqueue(obj);
            }
        }

        // 从对象池获取对象
        public GameObject GetFromPool(string poolName, Vector3 position, Quaternion rotation)
        {
            // 如果对象池不存在或预制件未注册，返回null
            if (!prefabDictionary.ContainsKey(poolName))
            {
                Debug.LogError($"获取对象失败：未注册预制件 {poolName}");
                return null;
            }

            // 确保对象池存在
            if (!poolDictionary.ContainsKey(poolName))
            {
                poolDictionary.Add(poolName, new Queue<GameObject>());
                
                // 创建池专用的父节点
                GameObject poolContainerObj = new GameObject(poolName + "Pool");
                poolContainerObj.transform.SetParent(poolRoot);
            }

            GameObject obj;

            // 检查池中是否有可用对象
            if (poolDictionary[poolName].Count > 0)
            {
                // 从池中获取对象
                obj = poolDictionary[poolName].Dequeue();
            }
            else
            {
                // 池为空，创建新对象
                GameObject prefab = prefabDictionary[poolName];
                obj = Instantiate(prefab);
                obj.name = prefab.name + "_" + Random.Range(1000, 9999);
                
                // 找到或创建这个池的容器
                Transform poolTransform = poolRoot.Find(poolName + "Pool");
                if (poolTransform == null)
                {
                    GameObject containerObj = new GameObject(poolName + "Pool");
                    containerObj.transform.SetParent(poolRoot);
                    poolTransform = containerObj.transform;
                }
                
                // 设置父对象为池容器
                obj.transform.SetParent(poolTransform);
            }

            // 设置位置和旋转
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            // 通知对象已从池中取出
            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnObjectSpawn();
                poolable.OnSpawn();
            }

            return obj;
        }

        // 将对象返回池中
        public void ReturnToPool(GameObject obj)
        {
            // 获取对象名称的前缀（没有数字后缀）
            string objName = obj.name;
            int underscoreIndex = objName.LastIndexOf('_');
            if (underscoreIndex > 0)
            {
                objName = objName.Substring(0, underscoreIndex);
            }

            // 通知对象已返回池中
            IPoolable poolable = obj.GetComponent<IPoolable>();
            if (poolable != null)
            {
                poolable.OnObjectDespawn();
                poolable.OnDespawn();
            }

            // 禁用对象
            obj.SetActive(false);

            // 添加回池中
            if (poolDictionary.ContainsKey(objName))
            {
                poolDictionary[objName].Enqueue(obj);
            }
            else
            {
                Debug.LogWarning($"返回对象到池失败：找不到池 {objName}");
                Destroy(obj);
            }
        }
    }
}