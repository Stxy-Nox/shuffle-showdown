using UnityEngine;
using UnityEditor;

namespace ShuffleShowdown.Editor
{
    /// <summary>
    /// 编辑器工具，用于更新敌人预制体的颜色设置
    /// </summary>
    public class EnemyPrefabUpdater : EditorWindow
    {
        private Color defaultEnemyColor = Color.white;
        
        [MenuItem("ShuffleShowdown/Update Enemy Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<EnemyPrefabUpdater>("敌人预制体更新器");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("敌人预制体颜色更新", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            defaultEnemyColor = EditorGUILayout.ColorField("敌人默认颜色", defaultEnemyColor);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("更新所有敌人预制体"))
            {
                UpdateAllEnemyPrefabs();
            }
            
            if (GUILayout.Button("添加EnemyHealth组件到所有敌人预制体"))
            {
                AddHealthComponentToAllEnemyPrefabs();
            }
        }
        
        private void UpdateAllEnemyPrefabs()
        {
            // 查找所有敌人预制体
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
            
            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                // 加载预制体
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;
                
                // 检查是否是敌人预制体
                if (prefab.name.Contains("Enemy") || prefab.GetComponent<Enemy>() != null)
                {
                    // 创建预制体实例
                    GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    if (instance == null) continue;
                    
                    // 更新材质颜色
                    Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
                    bool modified = false;
                    
                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer.sharedMaterial != null && renderer.sharedMaterial.HasProperty("_Color"))
                        {
                            // 创建材质的副本以避免修改共享材质
                            Material newMaterial = new Material(renderer.sharedMaterial);
                            newMaterial.color = defaultEnemyColor;
                            renderer.sharedMaterial = newMaterial;
                            modified = true;
                        }
                    }
                    
                    if (modified)
                    {
                        // 应用更改回预制体
                        PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
                        count++;
                    }
                    
                    // 销毁实例
                    DestroyImmediate(instance);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("操作完成", $"已更新 {count} 个敌人预制体的颜色。", "确定");
        }
        
        private void AddHealthComponentToAllEnemyPrefabs()
        {
            // 查找所有敌人预制体
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs" });
            
            int count = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                // 加载预制体
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null) continue;
                
                // 检查是否是敌人预制体
                if (prefab.name.Contains("Enemy") || prefab.GetComponent<Enemy>() != null)
                {
                    // 创建预制体实例
                    GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                    if (instance == null) continue;
                    
                    // 检查是否已有EnemyHealth组件
                    EnemyHealth healthComponent = instance.GetComponent<EnemyHealth>();
                    if (healthComponent == null)
                    {
                        // 添加EnemyHealth组件
                        healthComponent = instance.AddComponent<EnemyHealth>();
                        
                        // 获取Enemy组件，并同步健康值
                        Enemy enemy = instance.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            healthComponent.maxHealth = enemy.health;
                            healthComponent.defaultColor = defaultEnemyColor;
                        }
                        
                        // 应用更改回预制体
                        PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
                        count++;
                    }
                    
                    // 销毁实例
                    DestroyImmediate(instance);
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("操作完成", $"已为 {count} 个敌人预制体添加了EnemyHealth组件。", "确定");
        }
    }
}