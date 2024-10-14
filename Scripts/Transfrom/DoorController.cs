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
        // ドアのタイプ（回転、スライド）
        public bool IsPushDoorType;
        // オープン時の座標移動量
        public Vector3 MovePower = new Vector3(0, 0, -1);
        public Vector3 RotatePower = new Vector3(0, 90, 0);
        // アニメーションにかける時間
        public float duration = 1f;

        // デフォルト座標と回転
        private Vector3 _defaultPosition;
        private Vector3 _defaultRotation;
        // フラグ
        private bool isOpen = false;
        // アニメーション中かどうか
        private Tween moveTween;
        private Tween rotateTween;

        private CancellationTokenSource cancellationTokenSource;

        void Start()
        {
            // デフォルトの位置と回転を保存
            _defaultPosition = transform.localPosition;
            _defaultRotation = transform.localEulerAngles;

            // Play関数を毎フレーム監視 (UniRx使用)
            //Observable.EveryUpdate()
            //    .Where(_ => Input.GetKeyDown(KeyCode.Space)) // スペースキーでPlay関数を呼ぶ
            //    .Subscribe(_ => Play())
            //    .AddTo(this);

            gameObject.name = "OpenDoorX";

            if(TryGetComponent<BoxCollider>(out var box))
            {
                box.isTrigger = true;
                box.center = new Vector3(0, 1, 0.5f);
                box.size = new Vector3(0.5f, 2, 1);
            }
        }

        public async void Play(bool flgOverride = false, bool overrideIsOpen = false)
        {
            // 既に進行中のアニメーションがある場合はキャンセル
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            // フラグAを切り替える
            isOpen = flgOverride ? overrideIsOpen : !isOpen;

            // アニメーション中断
            moveTween?.Kill();
            rotateTween?.Kill();

            if (IsPushDoorType) // 回転式ドアの場合
            {
                if (isOpen)
                {
                    // 回転式の場合、ドアを回転させる
                    rotateTween = transform.DOLocalRotate(_defaultRotation + RotatePower, duration).SetEase(Ease.Linear);
                }
                else
                {
                    // ドアを元の回転位置に戻す
                    rotateTween = transform.DOLocalRotate(_defaultRotation, duration).SetEase(Ease.Linear);
                }
            }
            else // スライド式ドアの場合
            {
                if (isOpen)
                {
                    // スライド式の場合、ドアを移動させる
                    moveTween = transform.DOLocalMove(_defaultPosition + MovePower, duration).SetEase(Ease.Linear);
                }
                else
                {
                    // ドアを元の位置に戻す
                    moveTween = transform.DOLocalMove(_defaultPosition, duration).SetEase(Ease.Linear);
                }
            }

            try
            {
                // アニメーション完了を待つ
                await UniTask.Delay((int)(duration * 1000), cancellationToken: cancellationTokenSource
                    .Token);
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
