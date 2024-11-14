
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class FadeController : SingletonMonoBehaviour<FadeController>
{
    public float FadeInTime = 1f;
    public float FadeOutTime = 1f;

    [SerializeField]
    CanvasGroup group = null;

    [SerializeField]
    Fade fade = null;

    [Space(10)]
    public bool YKeyDebugFade;

    [Header("状態")]
    public bool IsFade;

    private void Start()
    {

    }

    private void Update()
    {
        if (YKeyDebugFade)
            if (Input.GetKeyDown(KeyCode.Y))
                Fadeout();
    }

    public void Fadeout()
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, () =>
        {
            // 黒くなったタイミングで呼び出される処理
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void LoadSceneFadeout(string loadSceneName)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, () =>
        {
            // 黒くなったタイミングで呼び出される処理
            SceneManager.LoadScene(loadSceneName);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void AdditiveSceneFadeout(string loadSceneName)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //SceneManager.Additive(loadSceneName);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
    public void ActionPlayFadeout(Action action)
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            action();
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }

    public void ReloadSceneFadeOut()
    {
        if (IsFade) return;
        IsFade = true;
        group.blocksRaycasts = false; // UIの操作を停止
        fade.FadeIn(FadeInTime, () =>
        {
            // 黒くなったタイミングで呼び出される処理
            //PlayableDirector.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            fade.FadeOut(FadeOutTime, () =>
            {
                // フェードが終了したタイミングで呼び出される処理
                group.blocksRaycasts = true; // UIの操作を再開
                IsFade = false;
            });
        });
    }
}
