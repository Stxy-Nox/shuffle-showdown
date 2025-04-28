using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ShuffleShowdown.Editor
{
    public static class BuildSettingsEditor
    {
        [MenuItem("ShuffleShowdown/Setup Build Settings")]
        public static void SetupBuildSettings()
        {
            // 添加场景到构建设置
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/BulletHellGame.unity", true)
            };
            
            Debug.Log("场景已添加到构建设置");
        }

        [MenuItem("ShuffleShowdown/Open Main Menu Scene")]
        public static void OpenMainMenuScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
            }
        }

        [MenuItem("ShuffleShowdown/Open Game Scene")]
        public static void OpenGameScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/BulletHellGame.unity");
            }
        }
    }
}