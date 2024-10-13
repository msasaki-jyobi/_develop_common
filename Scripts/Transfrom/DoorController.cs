using System;
using UnityEngine;
using DG.Tweening; // DOTweenを使用
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading; // UniTaskを使用

namespace develop_common
{
    public class DoorController : MonoBehaviour
    {
        public bool IsDefaultOpen;
        // フラグA
        private bool isOpen = false;

        // 移動先と回転先
        public Vector3 closePosition; // E ローカル座標 (閉じた時の座標)
        public Vector3 closeRotation; // F ローカル回転値 (閉じた時の回転)

        public Vector3 openPosition; // B ローカル座標 (開いた時の座標)
        public Vector3 openRotation; // D ローカル回転値 (開いた時の回転)


        // アニメーションにかける時間C
        public float duration = 1f;

        // アニメーション中かどうか
        private Tween moveTween;
        private Tween rotateTween;

        private CancellationTokenSource cancellationTokenSource;

        void Start()
        {
            //// Play関数を毎フレーム監視 (UniRx使用)
            //Observable.EveryUpdate()
            //    .Where(_ => Input.GetKeyDown(KeyCode.Space)) // スペースキーでPlay関数を呼ぶ
            //    .Subscribe(_ => Play())
            //    .AddTo(this);

            Play(true, IsDefaultOpen);
        }

        public async void Play(bool flgOverride = false, bool overrideIsOpen = false)
        {
            // 既に進行中のアニメーションがある場合はキャンセル
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // Aフラグを切り替える
            isOpen = !isOpen;

            if (flgOverride)
                isOpen = overrideIsOpen;

            // アニメーション中断
            moveTween?.Kill();
            rotateTween?.Kill();

            if (isOpen)
            {
                // AがONになったとき、Bのローカル座標へ移動し、Dのローカル回転値へ回転
                moveTween = transform.DOLocalMove(openPosition, duration).SetEase(Ease.Linear);
                rotateTween = transform.DOLocalRotate(openRotation, duration).SetEase(Ease.Linear);
            }
            else
            {
                // AがOFFになったとき、Eのローカル座標へ移動し、Fのローカル回転値へ回転
                moveTween = transform.DOLocalMove(closePosition, duration).SetEase(Ease.Linear);
                rotateTween = transform.DOLocalRotate(closeRotation, duration).SetEase(Ease.Linear);
            }

            try
            {
                // アニメーション完了を待つ
                await UniTask.Delay((int)(duration * 1000), cancellationToken: cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合は何もしない
            }
        }

        private void OnDestroy()
        {
            // オブジェクトが破棄される際にキャンセル
            cancellationTokenSource?.Cancel();
        }
    }

}
