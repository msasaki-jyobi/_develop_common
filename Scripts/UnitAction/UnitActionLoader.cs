using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using System;

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

        // Event
        public event Action<ActionBase> PlayActionEvent;
        public event Action<ActionBase> FinishActionEvent;
        public event Action<Vector3> FrameFouceEvent;
        public event Action FrameResetVelocityEvent;

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

        private void FinishMotionEventHandle(string stateName)
        {
            Debug.Log($"State: {stateName} 終了");
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
                    ChangeStatus(oldActiveActionBase.ActionFinish.SetFinishStatus);
                    // Next ActionData
                    if (oldActiveActionBase.ActionFinish.NextActionData != null)
                    {
                        LoadAction(oldActiveActionBase.ActionFinish.NextActionData);
                        return;
                    }
                }

            // Finish Event
            FinishActionEvent?.Invoke(oldActiveActionBase);

        }

        public void LoadAction(GameObject actionObject)
        {
            if (actionObject.TryGetComponent<ActionBase>(out var actionBase))
            {
                if (actionBase.ActionRequirement != null)
                    // アクションの条件チェック
                    if (!actionBase.ActionRequirement.CheckExecute(this))
                    {
                        _isExecuting = false;
                        return;
                    }

                Debug.Log($"実行!!. {gameObject.name} {actionObject.name}");

                // アクションの実行
                _isExecuting = true;
                ActiveActionObject = actionObject;
                ActiveActionBase = actionBase;
                IsNextAction = false;

                // イベント発行
                PlayActionEvent?.Invoke(actionBase);

                // Start
                if (actionBase.ActionStart != null)
                {
                    var stateName = actionBase.ActionStart.MotionName;
                    var late = actionBase.ActionStart.MotionLate;
                    var playType = actionBase.ActionStart.StatePlayType;
                    var reset = actionBase.ActionStart.IsStateReset;
                    var root = actionBase.ActionStart.IsApplyRootMotion;

                    _stateController.ChangeMotion(stateName, late, playType, reset, root);
                    ChangeStatus(actionBase.ActionStart.SetStartStatus);
                }
                // Frame
                if (actionBase.ActionFrame != null)
                    _loadFrameInfos = actionBase.ActionFrame.FrameInfo.Select(item => new FrameInfo(item)).ToList();

            }
        }

        public void ChangeStatus(EUnitStatus status)
        {
            if (status != EUnitStatus.None)
                UnitStatus = status;
        }
    }
}