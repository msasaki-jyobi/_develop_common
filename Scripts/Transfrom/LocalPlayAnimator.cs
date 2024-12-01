using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace develop_common
{
    [Serializable]
    public class AnimationStep
    {
        [Tooltip("ローカル座標での座標")] public Vector3 LocalPosition;
        [Tooltip("ローカル座標での回転角度")] public Vector3 LocalRotation;
        [Space(3)]
        [Tooltip("アニメーションにかかる時間（秒）")]public float Duration;
        [Tooltip("このステップが完了した後、次のステップに進むまでの待機時間（秒）")] public float TransitionDuration; // ステップ間の遷移時間
        [Space(3)]
        [Tooltip("再生するアニメーションステートの名前")] public string StateName;
        [Tooltip("ステート再生時の再生タイプ（例: ワンショット再生、ループ再生など）。")] public EStatePlayType StatePlayType;
        [Tooltip("ステートを再生する前にリセット（初期状態に戻す）するかどうか。")] public bool StateReset;
        [Space(3)]
        public ClipData SE;
        public GameObject EffectPrefab;
        [Space(3)]
        public int DamageValue;
        public string DamageAdditiveStateName = "";
        public string VoiceID;
    }

    public class LocalPlayAnimator : MonoBehaviour
    {
        [Tooltip("アニメーションの対象となるオブジェクト。\r\n")] public Transform Target; // アニメーション対象のTransform
        [Tooltip("アニメーションの状態を管理するカスタムコントローラー（Animatorと連携するオブジェクト）")] public AnimatorStateController AnimatorStateController;
        [Tooltip("実行するアニメーションステップのリスト。。")] public List<AnimationStep> AnimationSteps = new List<AnimationStep>(); // アニメーションステップのリスト
        [Tooltip("アニメーション開始前、および終了後に戻る初期位置。")] public Vector3 InitialPosition = Vector3.zero; // 元の位置
        [Tooltip(" アニメーション開始前、および終了後に戻る初期回転（オイラー角）。")] public Vector3 InitialRotation = Vector3.zero; // 元の回転
        [Tooltip("アニメーション終了後、ターゲットが初期位置に戻るまでにかかる時間（秒）。")] public float ReturnDuration = 0.5f; // 元の位置に戻る時間
        [Tooltip("初期位置に戻る際の追加の遷移時間（秒）。")] public float ReturnTransitionDuration = 0.2f; // 元の位置に戻る際の遷移時間

        [Tooltip("アニメーションをループ再生するかどうかを制御します。")] public bool IsLooping = false; // ループのフラグ
        private bool isAnimating = false; // 現在アニメーションが実行中かを管理

        private Sequence _animationSequence; // アニメーションシーケンス

        [Tooltip("デバッグ用のループ再生トグル。SetLoopFlag をテストするために使用。")] public bool DebugLoopPlay;
        [Tooltip("DebugLoopPlay と連動するデバッグ用フラグ。")] public bool DebugLoopFlg;

        private void Start()
        {
            DOTween.Init(); // DoTweenの初期化
            // IsLoopがONになるまでアニメーション開始はしない
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                OnPlay();

            if (DebugLoopPlay)
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
