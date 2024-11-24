
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Cysharp.Threading.Tasks;

public class FadeController : SingletonMonoBehaviour<FadeController>
{
    public float FadeInTime = 1f;
    public float FadeOutTime = 1f;
    public FadeImage FadeImage;


    [SerializeField]
    CanvasGroup group = null;

    [SerializeField]
    Fade fade = null;

    [Space(10)]
    public bool YKeyDebugFade;

    [Header("状態")]
    public bool IsFade;

    public event Action<string> LoadNameSceneEvent;
    private Color _defaultFadeColor;


    private void Awake()
    {
        _defaultFadeColor = FadeImage.color;
    }

    private void Start()
    {
        if (FadeController.Instance == this)
        {
            FadeOut();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Update()
    {
        if (YKeyDebugFade)
            if (Input.GetKeyDown(KeyCode.Y))
            {
                FadeIn(0.4f, 0.4f, Color.cyan);
            }
    }

    public void FadeIn(float fadeInTime = -1, float fadeOutTime = -1, Color color = default)
    {
        if (IsFade) return;
        var inTime = fadeInTime == -1 ? FadeInTime : fadeInTime;
        var outTime = fadeOutTime == -1 ? FadeOutTime : fadeOutTime;
        if (FadeImage != null)
        {
            var co = color == default ? _defaultFadeColor : color;
            FadeImage.color = co;
        }


        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(inTime, async () =>
        {
            await UniTask.Delay(1);
            // 黒くなったタイミングで呼び出される処理
            fade.FadeOut(outTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void LoadSceneFadeIn(string loadSceneName)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            LoadNameSceneEvent?.Invoke(loadSceneName);
            SceneManager.LoadScene(loadSceneName);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void AdditiveSceneFadeIn(string loadSceneName)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //SceneManager.Additive(loadSceneName);
            await UniTask.Delay(1);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void ActionPlayFadeIn(Action action)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            action();
            await UniTask.Delay(1);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void ReloadSceneFadeIn()
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            await UniTask.Delay(1);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    /// <summary>
    /// シーン開始時
    /// </summary>
    public void FadeOut()
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(0f, async () =>
        {
            await UniTask.Delay(1);
            // 黒くなったタイミングで呼び出される処理
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
}
