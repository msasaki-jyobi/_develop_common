using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;

namespace develop_common
{
    public class UnitActionLoader : MonoBehaviour
    {
        [SerializeField] private AnimatorStateController _stateController;
        public EUnitStatus UnitStatus;

        public GameObject C_TestAction;
        public GameObject X_TestAction;

        public GameObject ActiveAction { get; private set; }
        private List<FrameInfo> _loadFrameInfos;
        private bool _isExecuting;
        public bool IsNextAction { get; private set; }

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
                                            if(TryGetComponent<Rigidbody>(out var rigid)) rigid.AddForce(transform.up * frameInfo.FoucePower.y, ForceMode.Impulse);
                                        if (frameInfo.IsPrefab)
                                            if (ActiveAction.TryGetComponent<ActionPrefabInfo>(out var actionPrefabInfo)) actionPrefabInfo.CreatePrefab(frameInfo.PrefabNum, gameObject);
                                        if (frameInfo.IsNextAction)
                                            IsNextAction = true;

                                    }
                    }
                });


            _stateController.FinishMotionEvent += FinishMotionEventHandle;
        }

        private void FinishMotionEventHandle(string stateName)
        {
            _loadFrameInfos?.Clear();
            ActiveAction = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                LoadAction(C_TestAction);
            if (Input.GetKeyDown(KeyCode.X))
                LoadAction(X_TestAction);
        }



        private void LoadAction(GameObject ActionObject)
        {
            // アクションの条件チェック
            if (ActionObject.TryGetComponent<ActionRequirement>(out var requirement))
            {
                if (!requirement.CanExecute(gameObject))
                {
                    _isExecuting = false;
                    return;
                }
            }

            // アクションの実行
            _isExecuting = true;
            ActiveAction = ActionObject;
            IsNextAction = false;

            if (ActionObject.TryGetComponent<ActionBase>(out var actionBase))
            {
                var stateName = actionBase.MotionName;
                var late = actionBase.MotionLate;
                var playType = actionBase.StatePlayType;
                var reset = actionBase.StateReset;
                var root = actionBase.ApplyRootMotion;

                _stateController.ChangeMotion(stateName, late, playType, reset, root);
            }
            if (ActionObject.TryGetComponent<ActionFrame>(out var actionFrame))
            {
                //_loadFrameInfos = new List<FrameInfo>(actionFrame.FrameInfo);
                _loadFrameInfos = actionFrame.FrameInfo.Select(item => new FrameInfo(item)).ToList();
            }
        }
    }
}