using UnityEngine;
using TMPro; // TextMeshPro用
using UniRx;
using UniTask = Cysharp.Threading.Tasks.UniTask; // UniTask用
using DG.Tweening;
using System.Threading;
using System; // DOTween用


namespace develop_common
{

    public class TextFadeController : SingletonMonoBehaviour<TextFadeController>
    {
        public TextMeshProUGUI MilestroneTextUGUI;
        public TextMeshProUGUI MessageTextUGUI;
        public float fadeDuration = 1f; // A秒かけてフェードする時間
        public float waitDuration = 2f; // B秒経過後に再フェードアウト
        private Tween fadeTween; // DOTweenのTweenオブジェクト（キャンセル用）
        private CancellationTokenSource cancellationTokenSource; // UniTask用のキャンセルトークン

        void Start()
        {
            if (MilestroneTextUGUI != null)
            {
                // Qキーが押されたときの処理をUniRxで監視
                Observable.EveryUpdate()
                    .Where(_ => Input.GetKeyDown(KeyCode.Q))
                    .Subscribe(_ => OnQKeyPressed(MilestroneTextUGUI))
                    .AddTo(this); // オブジェクトが破棄されるときに自動的に購読解除
            }
        }

        private async void OnQKeyPressed(TextMeshProUGUI targetTextUGUI)
        {
            var defaultColor = Color.white;
            defaultColor.a = 0;
            targetTextUGUI.color = defaultColor;


            // 前回のアニメーションをキャンセルする
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // アルファ値を1にするTweenの開始（途中で新しいQキー押下があると、現在のTweenをキャンセル）
            fadeTween?.Kill();
            fadeTween = targetTextUGUI.DOFade(1f, fadeDuration).SetEase(Ease.Linear);

            // アニメーション完了後にB秒待機
            try
            {
                await UniTask.Delay((int)(waitDuration * 1000), cancellationToken: cancellationTokenSource.Token);

                // 待機後にアルファ値を0に戻すアニメーション
                fadeTween?.Kill();
                fadeTween = targetTextUGUI.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
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

        // 限定的処理
        public void UpdateMileStone(string text)
        {
            MilestroneTextUGUI.text = $"目標：{text}";
            OnQKeyPressed(MilestroneTextUGUI);
        }
        public void UpdateMessageText(string text)
        {
            MessageTextUGUI.gameObject.SetActive(true);
            MessageTextUGUI.text = $"{text}";
            OnQKeyPressed(MessageTextUGUI);
        }
    }
}
