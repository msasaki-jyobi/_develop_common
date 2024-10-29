using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using System;
using develop_tps;

namespace develop_common
{
    public class UnitActionLoader : MonoBehaviour
    {
        [SerializeField] private AnimatorStateController _stateController;
        public EUnitStatus UnitStatus;

        // Property
        public GameObject ActiveActionObject { get; private set; }
        public ActionBase ActiveActionBase { get; private set; }
        public bool IsNextAction { get; private set; }

        // private Frame Parameter
        private List<FrameInfo> _loadFrameInfos;

        // Action Loading Check
        private bool _isExecuting;

        // Action Delay
        private float _actionDelayTimer;
        private float _actionDelayTime = 0.1f;

        // Event
        public event Action<ActionBase> PlayActionEvent;
        public event Action<ActionBase> FinishActionEvent;
        public event Action<Vector3> FrameFouceEvent;
        public event Action FrameResetVelocityEvent;
        public event Action<string, int> StartAdditiveParameterEvent;
        public event Action<string, int> FinishAdditiveParameterEvent;

        [SerializeField] private bool _isDebugLog;

        private void Start()
        {
            // Event Handle
            _stateController.FinishMotionEvent += FinishMotionEventHandle;

            // Frame 
            _stateController.FrameTimer
                .Subscribe((x) =>
                {
                    // FrameInfo動作用
                    if (_isExecuting)
                    {
                        if (_loadFrameInfos?.Count != 0)
                            foreach (var frameInfo in _loadFrameInfos)
                                if (_stateController.Frame.Value >= frameInfo.PlayFrame)
                                    if (!frameInfo.IsComplete)
                                    {
                                        frameInfo.IsComplete = true;

                                        if (frameInfo.IsResetVelocity)
                                            FrameResetVelocityEvent?.Invoke();
                                        if (frameInfo.IsForce)
                                            FrameFouceEvent?.Invoke(frameInfo.FoucePower);
                                        if (frameInfo.IsPrefab)
                                            if (ActiveActionBase.ActionPrefabInfo != null)
                                                ActiveActionBase.ActionPrefabInfo.CreatePrefab(frameInfo.PrefabNum, gameObject);
                                        if (frameInfo.IsNextAction)
                                            IsNextAction = true;
                                    }
                    }
                });
        }

        private void Update()
        {
            if (_actionDelayTimer >= 0)
                _actionDelayTimer -= Time.deltaTime;

            // 着地して攻撃すると、動けなくなるバグの臨時修正
            if(_stateController.Frame.Value == 0 &&
                _stateController.FrameRate == 0 &&
                _stateController.TotalFrames == 0
                //_stateController.FrameTimer.Value >= 0.5f
                )
            {
                UnitStatus = EUnitStatus.Ready;
            }
        }

        private void FinishMotionEventHandle(string stateName, bool isLoop)
        {
            // Delay Time Return
            //if (_actionDelayTimer > 0) return;

            if (_isDebugLog)
                Debug.Log($"GameObject:{gameObject.name} State: {stateName} 終了XXX");

            if (ActiveActionBase != null)
                if (ActiveActionBase.ActionStart.MotionName != stateName) return;

            // Frame Reset
            _loadFrameInfos?.Clear();


            // ActionPlay Reset
            _isExecuting = false;

            // Reset
            var oldActiveActionObject = ActiveActionObject;
            var oldActiveActionBase = ActiveActionBase;
            ActiveActionObject = null;
            ActiveActionBase = null;

            // Action Finish
            if (oldActiveActionBase != null)
                if (oldActiveActionBase.ActionFinish != null)
                {
                    // Status Change
                    ChangeStatus(oldActiveActionBase.ActionFinish.SetFinishStatus, 1);
                    // Next ActionData
                    if (oldActiveActionBase.ActionFinish.NextActionData != null)
                    {
                        Debug.Log($"NextAction:::{oldActiveActionBase.ActionFinish.NextActionData}");
                        LoadAction(oldActiveActionBase.ActionFinish.NextActionData);
                        return;
                    }
                }

            // Finish Event
            FinishActionEvent?.Invoke(oldActiveActionBase);

            // Finish Additive Parameter
            if (oldActiveActionBase != null)
                if (oldActiveActionBase.ActionFinishAdditiveParameter != null)
                {
                    foreach (var finishParameter in oldActiveActionBase.ActionFinishAdditiveParameter.FinishAdditiveParameters)
                        FinishAdditiveParameterEvent?.Invoke(finishParameter.AdditiveParameterName, finishParameter.AdditiveParameterValue);
                }


        }

        public void LoadAction(GameObject actionObject, EInputReader key = EInputReader.None)
        {
            if (actionObject.TryGetComponent<ActionBase>(out var actionBase))
            {
                // Delay Time Return
                if (_actionDelayTimer > 0) return;

                if (actionBase.ActionRequirement != null)
                    // アクションの条件チェック
                    if (!actionBase.ActionRequirement.CheckExecute(this, key))
                        return;

                Debug.Log($"実行!!. {gameObject.name} {actionObject.name}");

                _stateController.Frame.Value = 0;
                _stateController.FrameTimer.Value = 0;

                // アクションの実行
                _isExecuting = true;
                ActiveActionObject = actionObject;
                ActiveActionBase = actionBase;
                IsNextAction = false;
                _actionDelayTimer = _actionDelayTime; // 連打発動防止


                // イベント発行
                PlayActionEvent?.Invoke(actionBase);

                // Start Additive Parameter
                if (actionBase.ActionStartAdditiveParameter != null)
                {
                    foreach (var startParameter in actionBase.ActionStartAdditiveParameter.StartAdditiveParameters)
                        StartAdditiveParameterEvent?.Invoke(startParameter.AdditiveParameterName, startParameter.AdditiveParameterValue);
                }

                // Start
                if (actionBase.ActionStart != null)
                {
                    var stateName = actionBase.ActionStart.MotionName;
                    var playType = actionBase.ActionStart.StatePlayType;
                    var reset = actionBase.ActionStart.IsStateReset;
                    var root = actionBase.ActionStart.IsApplyRootMotion;

                    var layer = actionBase.ActionStart.AnimatorLayer;

                    if (layer == 0)
                        _stateController.StatePlay(stateName, playType, reset, root);
                    else
                    {
                        _stateController.AnimatorLayerWeightPlay(layer, stateName,
                            actionBase.ActionStart.WeightValue, actionBase.ActionStart.WeightTime);
                    }

                    ChangeStatus(actionBase.ActionStart.SetStartStatus, 0);

                }
                // Frame
                if (actionBase.ActionFrame != null)
                    _loadFrameInfos = actionBase.ActionFrame.FrameInfo.Select(item => new FrameInfo(item)).ToList();

            }
        }

        public void ChangeStatus(EUnitStatus status, int code = 0)
        {
            if (status != EUnitStatus.None)
                UnitStatus = status;

        }
    }
}