using UnityEngine;
using System;
using Cysharp.Threading.Tasks;  // UniTaskを使用するための名前空間

public class Fade : MonoBehaviour
{
    IFade fade;

    void Start()
    {
        Init();
        fade.Range = cutoutRange;
    }

    float cutoutRange;

    void Init()
    {
        fade = GetComponent<IFade>();
    }

    void OnValidate()
    {
        Init();
        fade.Range = cutoutRange;
    }

    async UniTask FadeoutTask(float time, Action action)
    {
        float endTime = Time.realtimeSinceStartup + time * (cutoutRange);

        while (Time.realtimeSinceStartup <= endTime)
        {
            cutoutRange = (endTime - Time.realtimeSinceStartup) / time;
            fade.Range = cutoutRange;
            await UniTask.Yield(PlayerLoopTiming.Update); // フレームの終わりを待つ
        }

        cutoutRange = 0;
        fade.Range = cutoutRange;

        action?.Invoke();
    }

    async UniTask FadeinTask(float time, Action action)
    {
        float endTime = Time.realtimeSinceStartup + time * (1 - cutoutRange);

        while (Time.realtimeSinceStartup <= endTime)
        {
            cutoutRange = 1 - ((endTime - Time.realtimeSinceStartup) / time);
            fade.Range = cutoutRange;
            await UniTask.Yield(PlayerLoopTiming.Update); // フレームの終わりを待つ
        }

        cutoutRange = 1;
        if (fade != null)
            fade.Range = cutoutRange;

        action?.Invoke();
    }

    public void FadeOut(float time, Action action = null)
    {
        FadeoutTask(time, action).Forget(); // 非同期処理を開始し、結果を待たない
    }

    public void FadeIn(float time, Action action = null)
    {
        FadeinTask(time, action).Forget(); // 非同期処理を開始し、結果を待たない
    }
}
