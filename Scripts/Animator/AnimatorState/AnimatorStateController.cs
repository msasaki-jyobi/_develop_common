using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;

namespace develop_common
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
        [SerializeField] private string _defaultAnimatorState; // botu
        [SerializeField] private List<string> _defaultRandomAnimatorState = new List<string>();

        public ReactiveProperty<string> MainStateName { get; private set; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> Frame { get; private set; } = new ReactiveProperty<int>();
        public ReactiveProperty<float> FrameTimer { get; private set; } = new ReactiveProperty<float>();

        // All State (Editor Tools Get)
        public List<string> AllStates = new List<string>();

        //private string _lastStateName;
        private float _frameRate = 60f;
        private float _totalFrames = 60f;
        private bool _isMotionLoop;

        public event Action<string, bool> FinishMotionEvent;

        private void Start()
        {
            if (_defaultRandomAnimatorState.Count > 0)
            {
                int ran = UnityEngine.Random.Range(0, _defaultRandomAnimatorState.Count);
                StatePlay(_defaultRandomAnimatorState[ran], EStatePlayType.SinglePlay, true);
            }
            else if(_defaultAnimatorState != "")
            {
                StatePlay(_defaultAnimatorState, EStatePlayType.SinglePlay, true);
            }

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
                        //else
                        //{
                        //    // モーションの終了を発行
                        //    FinishMotionEvent.Invoke(MainStateName.Value);
                        //}

                        // モーションの終了を発行
                        FinishMotionEvent?.Invoke(MainStateName.Value, _isMotionLoop);
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
        public void StatePlay(string stateName, EStatePlayType statePlayType, bool resetMotion, bool apply = false)
        {
            _isMotionLoop = statePlayType == EStatePlayType.Loop ? true : false;
            if (resetMotion) MainStateName.Value = "";
            MainStateName.Value = stateName;
            Animator.applyRootMotion = apply;
            _frameRate = GetPlayStateFrameLate();
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

        /// <summary>
        /// 指定レイヤーのモーションを再生します(Layerを利用した際に活用）
        /// </summary>
        /// <param name="layerNo">レイヤー番号</param>
        /// <param name="_animator">Animator</param>
        /// <param name="animName">ステート名</param>
        /// <param name="fadeLength">切り替え時間</param>
        public async void AnimatorLayerPlay(int layerNo, string animName, float fadeLength)
        {
            //if (_addTimer >= 0.7f) return;
            //_addTimer = 1;

            // 指定レイヤーの情報を取得
            AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(layerNo);
            // アニメーションを実行
            //animator.Play(info.shortNameHash, layerNo, 0);
            // 滑らかに切り替えるまでの時間を計測
            float duration = fadeLength / Animator.GetCurrentAnimatorStateInfo(layerNo).length;
            // 現在のモーションから滑らかにモーション切り替え
            Animator.CrossFade("", duration, layerNo);
            await UniTask.Delay(1);
            Animator.CrossFade(animName, duration, layerNo);
        }



        /// <summary>
        /// 現在再生中のモーションフレームレートを返します
        /// </summary>
        /// <param name="animator"></param>
        /// <returns></returns>
        public float GetPlayStateFrameLate()
        {
            float rate = 0;

            // 現在のAnimatorStateに関連付けられているAnimationClipを取得
            AnimatorClipInfo[] clipInfos = Animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfos.Length > 0)
            {
                // 取得したAnimationClipからフレームレートを表示
                AnimationClip clip = clipInfos[0].clip;
                Debug.Log("Animation Clip: " + clip.name + " | Frame Rate: " + clip.frameRate);
                rate = clip.frameRate;
            }

            return rate;
        }
    }
}
