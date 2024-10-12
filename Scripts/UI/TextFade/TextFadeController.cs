using UnityEngine;
using TMPro; // TextMeshPro用
using UniRx;
using UniTask = Cysharp.Threading.Tasks.UniTask; // UniTask用
using DG.Tweening;
using System.Threading;
using System; // DOTween用

public class TextFadeController : MonoBehaviour
{
    public TextMeshProUGUI text; // TextMeshProの参照
    public float fadeDuration = 1f; // A秒かけてフェードする時間
    public float waitDuration = 2f; // B秒経過後に再フェードアウト
    private Tween fadeTween; // DOTweenのTweenオブジェクト（キャンセル用）
    private CancellationTokenSource cancellationTokenSource; // UniTask用のキャンセルトークン

    void Start()
    {
        // Qキーが押されたときの処理をUniRxで監視
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Q))
            .Subscribe(_ => OnQKeyPressed())
            .AddTo(this); // オブジェクトが破棄されるときに自動的に購読解除
    }

    private async void OnQKeyPressed()
    {
        // 前回のアニメーションをキャンセルする
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();

        // アルファ値を1にするTweenの開始（途中で新しいQキー押下があると、現在のTweenをキャンセル）
        fadeTween?.Kill();
        fadeTween = text.DOFade(1f, fadeDuration).SetEase(Ease.Linear);

        // アニメーション完了後にB秒待機
        try
        {
            await UniTask.Delay((int)(waitDuration * 1000), cancellationToken: cancellationTokenSource.Token);

            // 待機後にアルファ値を0に戻すアニメーション
            fadeTween?.Kill();
            fadeTween = text.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
        }
        catch (OperationCanceledException)
        {
            // キャンセルが発生した場合の処理（何もしないで終了）
        }
    }

    private void OnDestroy()
    {
        // オブジェクトが破棄される際にリソースを解放
        cancellationTokenSource?.Cancel();
    }
}
