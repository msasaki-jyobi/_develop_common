using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameSet.Common
{
    public enum EStatePlayType
    {
        SinglePlay,
        Loop,
    }

    public class AnimatorStateController : MonoBehaviour
    {
        public Animator Animator;
        [SerializeField] private float _crossAnimationTimer = 0.2f;


        public ReactiveProperty<string> MainStateName { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> Frame { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<float> FrameTimer { get; private set; } = new ReactiveProperty<float>();

        //private string _lastStateName;
        private float _frameRate = 60f;
        private float _totalFrames = 60f;
        private bool _isMotionLoop;

        public event Action<string> FinishMotionEvent;

        private void Start()
        {
            FrameTimer
                .Subscribe((x) => Frame.Value = (int)(FrameTimer.Value * _frameRate));

            Frame
                .Subscribe((x) =>
                {
                    // モーション終了時
                    if (Frame.Value >= _totalFrames)
                    {
                        Debug.Log("モーション終了");
                        FrameTimer.Value = 0;
                        if (_isMotionLoop) // モーションを繰り返す
                        {
                            var stateName = MainStateName.Value;
                            MainStateName.Value = "";
                            MainStateName.Value = stateName;
                        }
                        else
                        {
                            // モーションの終了を発行
                            FinishMotionEvent.Invoke(MainStateName.Value);
                        }
                    }
                });

            MainStateName
                .Subscribe((x) => {
                    OnChangeState();
                });
        }

      

        private void Update()
        {
            // アニメーション用カウント
            FrameTimer.Value += Time.deltaTime;
        }

        /// <summary>
        /// モーションを切り替える
        /// </summary>
        /// <param name="stateName">切り替えるモーション</param>
        /// <param name="rate">フレームレート</param>
        /// <param name="motionLoop">モーションのループ（LoopTimerがTrueのモーションはこの値は無視されLoopされます）</param>
        /// <param name="resetMotion">同じモーションでも再生しなおす</param>
        public void ChangeMotion(string stateName, float rate, EStatePlayType statePlayType, bool resetMotion, bool apply = false)
        {
            _frameRate = rate;
            _isMotionLoop = statePlayType == EStatePlayType.Loop ? true : false;
            if (resetMotion) MainStateName.Value = "";
            MainStateName.Value = stateName;
            Animator.applyRootMotion = apply;
        }

        /// <summary>
        /// ステートを切り替える
        /// </summary>
        private async void OnChangeState()
        {
            // クロスフェードを開始する
            Animator.CrossFade(MainStateName.Value, _crossAnimationTimer);

            // 次のステートに遷移するのを待つ
            await UniTask.Yield(PlayerLoopTiming.Update);

            AnimatorStateInfo nextStateInfo = Animator.GetNextAnimatorStateInfo(0);

            // 遷移先ステートがまだ0であれば、次のフレームまで待つ
            while (nextStateInfo.length == 0)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                nextStateInfo = Animator.GetNextAnimatorStateInfo(0);
            }

            float animationLength = nextStateInfo.length;
            FrameTimer.Value = 0;
            _totalFrames = Mathf.RoundToInt(animationLength * _frameRate);

            // 次のモーションの終了フレームレートを出力
            // Debug.Log("Total frames in the next state: " + _totalFrames);
        }

    }
}
