using UnityEngine;
using UnityEditor;

public class SceneSetup : MonoBehaviour
{
    [MenuItem("Window/SetupBuildSettings")]
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
}