using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

namespace ShuffleShowdown
{
    public class GameOverManager : MonoBehaviour
    {
        [Header("UI引用")]
        public Text titleText;               // 游戏标题文本
        public Button mainMenuButton;        // 返回主菜单按钮
        public Button exitButton;            // 退出按钮

        [Header("设置")]
        public string gameSceneName = "BulletHellGame";  // 游戏场景名称
        public string mainMenuSceneName = "MainMenu";    // 主菜单场景名称
        public float buttonAnimationDuration = 0.1f;     // 按钮动画持续时间
        public float buttonScale = 1.1f;                 // 按钮悬停时的缩放

        private void Start()
        {
            // 激活游戏结束菜单
            gameObject.SetActive(true);
            
            // 设置游戏标题
            if (titleText != null)
            {
                titleText.text = "Game over";
            }

            // 为主菜单按钮添加事件监听
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);

                // 添加悬停动画
                AddButtonAnimation(mainMenuButton);
            }

            // 为退出按钮添加事件监听
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ExitGame);

                // 添加悬停动画
                AddButtonAnimation(exitButton);
            }
            
            // 设置为游戏结束状态
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.GameOver);
            }
        }

        // 返回主菜单
        public void ReturnToMainMenu()
        {
            // 加载主菜单场景
            SceneManager.LoadScene(mainMenuSceneName);
        }

        // 开始新游戏
        public void StartNewGame()
        {
            // 加载游戏场景
            SceneManager.LoadScene(gameSceneName);
        }

        // 退出游戏
        public void ExitGame()
        {
#if UNITY_EDITOR
            // 在Unity编辑器中停止播放模式
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在构建的应用程序中退出
            Application.Quit();
#endif
        }

        // 为按钮添加悬停动画
        private void AddButtonAnimation(Button button)
        {
            // 获取按钮的原始缩放
            Vector3 originalScale = button.transform.localScale;
            Vector3 hoverScale = originalScale * buttonScale;

            // 创建事件触发器
            EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<EventTrigger>();
            }

            // 清除任何现有触发器
            trigger.triggers.Clear();

            // 添加进入事件
            EventTrigger.Entry enterEntry = new EventTrigger.Entry();
            enterEntry.eventID = EventTriggerType.PointerEnter;
            enterEntry.callback.AddListener((data) => {
                StartCoroutine(ScaleButton(button.transform, hoverScale, buttonAnimationDuration));
            });
            trigger.triggers.Add(enterEntry);

            // 添加退出事件
            EventTrigger.Entry exitEntry = new EventTrigger.Entry();
            exitEntry.eventID = EventTriggerType.PointerExit;
            exitEntry.callback.AddListener((data) => {
                StartCoroutine(ScaleButton(button.transform, originalScale, buttonAnimationDuration));
            });
            trigger.triggers.Add(exitEntry);
        }

        // 缩放按钮的协程
        private IEnumerator ScaleButton(Transform buttonTransform, Vector3 targetScale, float duration)
        {
            Vector3 startScale = buttonTransform.localScale;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                buttonTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
                yield return null;
            }

            buttonTransform.localScale = targetScale;
        }
    }
}