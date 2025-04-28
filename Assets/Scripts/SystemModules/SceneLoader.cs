using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
namespace ShuffleShowdown
{
    /// <summary>
    /// 管理场景加载和管理的工具类
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        // 单例模式
        public static SceneLoader Instance { get; private set; }
        
        [Header("场景名称")]
        public string mainMenuSceneName = "MainMenu";
        public string gameSceneName = "BulletHellGame";
        
        [Header("加载设置")]
        public float minimumLoadTime = 1.5f; // 最小加载时间，用于显示加载画面
        
        private void Awake()
        {
            // 确保单例实例唯一性
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        
        /// <summary>
        /// 加载主菜单场景
        /// </summary>
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        
        /// <summary>
        /// 加载游戏场景
        /// </summary>
        public void LoadGameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }
        
        /// <summary>
        /// 退出游戏
        /// </summary>
        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        
        /// <summary>
        /// 将场景添加到构建设置中（仅在编辑器中有效）
        /// </summary>
        public static void SetupBuildSettings()
{
    #if UNITY_EDITOR
    // 获取场景（虽然这两行其实没有使用，可以删掉）
    Scene mainMenuScene = EditorSceneManager.GetSceneByPath("Assets/Scenes/MainMenu.unity");
    Scene gameScene = EditorSceneManager.GetSceneByPath("Assets/Scenes/BulletHellGame.unity");

    // 添加场景到构建设置
    EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
    {
        new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
        new EditorBuildSettingsScene("Assets/Scenes/BulletHellGame.unity", true)
    };

    Debug.Log("场景已添加到构建设置");
    #endif
}
    }
}