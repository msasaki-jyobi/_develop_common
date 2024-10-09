using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace develop_common
{
    [Serializable]
    public class AnimationStep
    {
        public Vector3 LocalPosition;
        public Vector3 LocalRotation;
        public float Duration;
        public float TransitionDuration; // ステップ間の遷移時間
        public string StateName;
        public EStatePlayType StatePlayType;
        public bool StateReset;
    }

    public class LocalPlayAnimator : MonoBehaviour
    {
        public Transform Target; // アニメーション対象のTransform
        public AnimatorStateController AnimatorStateController;
        public List<AnimationStep> AnimationSteps = new List<AnimationStep>(); // アニメーションステップのリスト
        public Vector3 InitialPosition = Vector3.zero; // 元の位置
        public Vector3 InitialRotation = Vector3.zero; // 元の回転
        public float ReturnDuration = 0.5f; // 元の位置に戻る時間
        public float ReturnTransitionDuration = 0.2f; // 元の位置に戻る際の遷移時間

        public bool IsLooping = false; // ループのフラグ
        private bool isAnimating = false; // 現在アニメーションが実行中かを管理

        private Sequence _animationSequence; // アニメーションシーケンス

        public bool DebugLoopPlay;
        public bool DebugLoopFlg;

        private void Start()
        {
            DOTween.Init(); // DoTweenの初期化
            // IsLoopがONになるまでアニメーション開始はしない
        }

        private void Update()
        {
            if(DebugLoopPlay)
            {
                SetLoopFlag(DebugLoopFlg);
                DebugLoopPlay = false;
            }
        }
        // アニメーションを再生するメソッド
        public void OnPlay()
        {
            // 既にアニメーションが再生中でなければ開始する
            if (!isAnimating)
            {
                StartAnimating();
            }
        }

        private void StartAnimating()
        {
            // 現在のシーケンスを停止して破棄
            _animationSequence?.Kill();

            isAnimating = true;

            _animationSequence = DOTween.Sequence();

            // アニメーションステップを順に追加
            foreach (var step in AnimationSteps)
            {
                // カスタムモーション（Animatorの状態再生）をシーケンスに追加
                if (!string.IsNullOrEmpty(step.StateName))
                {
                    _animationSequence.AppendCallback(() =>
                    {
                        AnimatorStateController.StatePlay(step.StateName, step.StatePlayType, step.StateReset);
                    });
                }

                _animationSequence.Append(Target.DOLocalMove(step.LocalPosition, step.Duration).SetEase(Ease.OutCubic));
                _animationSequence.Join(Target.DOLocalRotate(step.LocalRotation, step.Duration).SetEase(Ease.OutCubic));

                // ステップ間の遷移時間を挿入
                _animationSequence.AppendInterval(step.TransitionDuration);
            }

            // ループフラグを確認し、最後に止める処理
            _animationSequence.OnComplete(() =>
            {
                if (IsLooping)
                {
                    StartAnimating(); // ループフラグがONなら最初から再生
                }
                else
                {
                    isAnimating = false; // アニメーション終了
                    // リストの最後の状態で終了（初期位置には戻らない）
                    var lastStep = AnimationSteps[AnimationSteps.Count - 1];
                    Target.localPosition = lastStep.LocalPosition;
                    Target.localRotation = Quaternion.Euler(lastStep.LocalRotation);
                }
            });

            _animationSequence.Play();
        }

        public void ChangeWaruSpeed(float speed)
        {
            ReturnDuration /= speed;
            ReturnTransitionDuration /= speed;
            foreach (var step in AnimationSteps)
            {
                step.Duration /= speed;
                step.TransitionDuration /= speed;
            }

            // 速度を変えた後もアニメーションが実行中でなければ再生
            if (!isAnimating)
            {
                OnPlay();
            }
        }

        // フラグをオンにしてシーケンスを再実行
        public void SetLoopFlag(bool loop)
        {
            IsLooping = loop;

            if (loop && !isAnimating)
            {
                // フラグがオンでかつアニメーションが再生されていない場合、再度再生
                StartAnimating();
            }
        }
    }
}
