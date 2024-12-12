
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Cysharp.Threading.Tasks;
using TMPro;

public class FadeController : SingletonMonoBehaviour<FadeController>
{
    public float FadeInTime = 1f;
    public float FadeOutTime = 1f;
    public FadeImage FadeImage;
    public TextMeshProUGUI LoadingTextUGUI;


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
    }

    private async void Start()
    {

        await UniTask.Delay(10);
        _defaultFadeColor = FadeImage.color;

        if (FadeController.Instance == this)
        {
            Application.targetFrameRate = 60;
            FadeOut();
            if(gameObject != null)
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

    public void FadeIn(float fadeInTime = -1, float fadeOutTime = -1, Color color = default, bool loadingText = false)
    {
        // フェードカラー指定
        FadeImage.UpdateMaterialColor(color != default ? color : Color.white);

        if (IsFade) return;
        var inTime = fadeInTime == -1 ? FadeInTime : fadeInTime;
        var outTime = fadeOutTime == -1 ? FadeOutTime : fadeOutTime;
        if (FadeImage != null)
        {
            var co = color == default ? _defaultFadeColor : color;
            FadeImage.color = co;
        }
        if (loadingText)
            LoadingTextUGUI.text = "よみこみ中...";
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(inTime, async () =>
        {
            await UniTask.Delay(1);
            // 黒くなったタイミングで呼び出される処理
            fade.FadeOut(outTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void LoadSceneFadeIn(string loadSceneName, bool isLoadingText = false)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        if (isLoadingText)
            LoadingTextUGUI.text = "よみこみ中...";
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            LoadNameSceneEvent?.Invoke(loadSceneName);
            SceneManager.LoadScene(loadSceneName);
            fade.FadeOut(FadeOutTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void AdditiveSceneFadeIn(string loadSceneName, bool loadingText = false)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        if (loadingText)
            LoadingTextUGUI.text = "よみこみ中...";
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //SceneManager.Additive(loadSceneName);
            await UniTask.Delay(1);
            fade.FadeOut(FadeOutTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void ActionPlayFadeIn(Action action, float fadeInTime = -1, float fadeOutTime = -1, Color color = default, bool loadingText = false)
    {
        // フェードカラー指定
        FadeImage.UpdateMaterialColor(color != default ? color : Color.white);

        var inTime = fadeInTime == -1 ? FadeInTime : fadeInTime;
        var outTime = fadeOutTime == -1 ? FadeOutTime : fadeOutTime;
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        if (loadingText)
            LoadingTextUGUI.text = "よみこみ中...";
        fade.FadeIn(inTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            action();
            await UniTask.Delay(1);
            fade.FadeOut(outTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void ReloadSceneFadeIn( bool loadingText = false)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        if (loadingText)
            LoadingTextUGUI.text = "よみこみ中...";
        fade.FadeIn(FadeInTime, async () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            await UniTask.Delay(1);
            fade.FadeOut(FadeOutTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    /// <summary>
    /// シーン開始時
    /// </summary>
    public void FadeOut(bool loadingText = false)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        if(loadingText)
        LoadingTextUGUI.text = "よみこみ中...";
        fade.FadeIn(0f, async () =>
        {
            await UniTask.Delay(1);
            // 黒くなったタイミングで呼び出される処理
            fade.FadeOut(FadeOutTime, () =>
            {
                LoadingTextUGUI.text = "";
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
}
