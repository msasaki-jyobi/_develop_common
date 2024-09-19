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
        public GameObject ActiveActionObject { get; private set; }
        public ActionBase ActiveActionBase { get; private set; }
        private List<FrameInfo> _loadFrameInfos;

        // Action Loading Check
        private bool _isExecuting;
        public bool IsNextAction { get; private set; }

        public event Action<ActionBase> PlayActionEvent;

        private void Start()
        {
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

                                        if (frameInfo.IsForce)
                                            if (TryGetComponent<Rigidbody>(out var rigid)) rigid.AddForce(transform.up * frameInfo.FoucePower.y, ForceMode.Impulse);
                                        if (frameInfo.IsPrefab)
                                            if (ActiveActionBase.ActionPrefabInfo != null)
                                                ActiveActionBase.ActionPrefabInfo.CreatePrefab(frameInfo.PrefabNum, gameObject);
                                        if (frameInfo.IsNextAction)
                                            IsNextAction = true;

                                    }
                    }
                });
            _stateController.FinishMotionEvent += FinishMotionEventHandle;
        }

        private void FinishMotionEventHandle(string stateName)
        {
            Debug.Log($"State: {stateName} 終了");
            _loadFrameInfos?.Clear();
            _isExecuting = false;

            if (ActiveActionBase.ActionFinish != null)
            {
                ChangeStatus(ActiveActionBase.ActionFinish.SetFinishStatus);
                if (ActiveActionBase.ActionFinish.NextActionData != null)
                    LoadAction(ActiveActionBase.ActionFinish.NextActionData);
            }
            ActiveActionObject = null;
            ActiveActionBase = null;
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
                        Debug.Log($"実行できません. {gameObject.name} {actionObject.name}");
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
                //// Finish
                //if (actionBase.ActionFinish != null)
                //    ChangeStatus(actionBase.ActionFinish.SetFinishStatus);
            }
        }

        public void ChangeStatus(EUnitStatus status)
        {
            if (status != EUnitStatus.None)
                UnitStatus = status;
        }
    }
}