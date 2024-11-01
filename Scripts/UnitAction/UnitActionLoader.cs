using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using System;
using develop_tps;
using Cysharp.Threading.Tasks;

namespace develop_common
{
    public class UnitActionLoader : MonoBehaviour
    {
        [SerializeField] private UnitComponents _unitComponents;
        public EUnitStatus UnitStatus;

        // Property
        public GameObject ActiveActionObject { get; private set; }
        public ActionBase ActiveActionBase { get; private set; }
        public bool IsNextAction { get; private set; }

        // 攻撃判定
        public bool IsAttack;

        // private Frame Parameter
        private List<FrameInfo> _loadFrameInfos;

        // Action Loading Check
        private bool _isExecuting;

        // Action Delay
        private float _actionDelayTimer;
        private float _actionDelayTime = 0.1f;

        // components
        private AnimatorStateController _stateController;
        private BattleDealer _attackDealer;

        // Event
        public event Action<ActionBase> PlayActionEvent;
        public event Action<ActionBase> FinishActionEvent;
        public event Action<Vector3> FrameFouceEvent;
        public event Action FrameResetVelocityEvent;
        public event Action<string, int> StartAdditiveParameterEvent;
        public event Action<string, int> FinishAdditiveParameterEvent;

        public EUnitType UnitType;
        [Header("攻撃対象IK用")]
        public string TargetObjectName = "Player";
        public UnitComponents TargetComponents;

        private void Start()
        {
            // IK攻撃用ターゲットを取得
            var target = GameObject.Find(TargetObjectName);
            if (target != null)
                TargetComponents = target.GetComponent<UnitComponents>();

            _stateController = _unitComponents.AnimatorStateController;
            _attackDealer = _unitComponents.AttackDealer;
            if (TryGetComponent<IHealth>(out var health))
                UnitType = health.UnitType;

            // Event Handle
            _stateController.FinishMotionEvent += FinishMotionEventHandle;

            // Frame 
            _stateController.FrameTimer
                .Subscribe(async (x) =>
                {
                    // FrameInfo動作用
                    if (_isExecuting)
                    {
                        if (_loadFrameInfos?.Count != 0)
                            foreach (var frameInfo in _loadFrameInfos)
                                if (_stateController.Frame.Value * _stateController.GetCurrentClipSpeed() >= frameInfo.PlayFrame)
                                    if (!frameInfo.IsComplete)
                                    {
                                        frameInfo.IsComplete = true;

                                        if (frameInfo.NormalData != null)
                                        {
                                            if (frameInfo.NormalData.IsResetVelocity) // 速度リセット
                                                FrameResetVelocityEvent?.Invoke();

                                            if (frameInfo.NormalData.IsForce)　// 力を加える
                                                FrameFouceEvent?.Invoke(frameInfo.NormalData.ForcePower);

                                            if (frameInfo.NormalData.IsKey)　// 追加入力受付をON
                                                IsNextAction = true;
                                        }

                                        if (frameInfo.PrefabData != null) // Prefab生成
                                            CreatePrefab(frameInfo.PrefabData, gameObject);

                                        if (frameInfo.IKData != null) // IKの設定
                                        {
                                            // このタイミングはモーション中なのでここでONにする必要がある
                                            // ここで攻撃対象のIK取得が必要
                                            if (_unitComponents.iKController != null)
                                                _unitComponents.iKController.SetTargetEnableIK(
                                                     frameInfo.IKData.IKKeyName,
                                                     frameInfo.IKData.IKLifeTime,
                                                     TargetComponents.UnitInstance.SearchObject(frameInfo.IKData.IKTargetKeyName).transform
                                                    );
                                        }

                                        if (frameInfo.ActiveAttackData != null) // Task:攻撃判定をONにする
                                        {
                                            // Pull
                                            //List<PullData> pulls = new List<PullData>();
                                            //if (frameInfo.PullData != null)
                                            //    pulls = frameInfo.PullData.PullDatas;
                                            //// 固定化情報を変更する
                                            //if (frameInfo.IsPull)
                                            //{
                                            //    if (pulls.Count > frameInfo.PullNum + 1)
                                            //    {
                                            //        // 指定部位を基準に敵をコライダーに固定させるようにダメージ情報を変更する
                                            //        ActiveActionBase.ActionActiveAttackBody.IsPull = true;
                                            //        ActiveActionBase.ActionActiveAttackBody.PullData = pulls[frameInfo.PullNum];
                                            //    }
                                            //}

                                            // ActionChange
                                            //List<GameObject> changeActions = new List<GameObject>();
                                            //if (frameInfo.ActiveAttackData != null)
                                            //    changeActions = frameInfo.AttackChangeData.ChangeDamageActions;

                                            //// ダメージ情報を変更する
                                            //if (frameInfo.IsChangeDamage)
                                            //{
                                            //    if (changeActions.Count > frameInfo.ChangeDamageActionNum + 1)
                                            //    {
                                            //        // 技を変更する
                                            //        ActiveActionBase.ActionActiveAttackBody.DamageAction = changeActions[frameInfo.ChangeDamageActionNum];
                                            //    }
                                            //}

                                            // AttackBodyName
                                            //LogManager.Instance.AddLog(gameObject, $"IsPull:{frameInfo.IsPull}, IsChangeDamage:{frameInfo.IsChangeDamage}", 1);
                                            foreach (var attackBodyName in frameInfo.ActiveAttackData.AttackBodyNames)
                                            {
                                                _attackDealer.SetAttack
                                                (attackBodyName,
                                                frameInfo.ActiveAttackData.AttackLifeTime,
                                                frameInfo.Attack_DamageData,
                                                frameInfo.PullData != null,
                                                frameInfo.PullData != null ? frameInfo.PullData.PullDatas : null);
                                            }
                                        }
                                    }
                    }
                });
        }

        private void Update()
        {
            if (_actionDelayTimer >= 0)
                _actionDelayTimer -= Time.deltaTime;

            // 着地して攻撃すると、動けなくなるバグの臨時修正
            if (_stateController.Frame.Value == 0 &&
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

            //Debug.Log($"GameObject:{gameObject.name} State: {stateName} 終了XXX");

            if (ActiveActionBase != null)
                if (ActiveActionBase.ActionStart.PlayClip.name != stateName) return;

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

                    if (oldActiveActionBase.ActionFinish.IsDown)
                        _unitComponents.PartAttachment.IsDown = true;
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

        public void LoadAction(GameObject actionObject, EInputReader key = EInputReader.None, bool ignoreDelayTime = false)
        {
            if (actionObject.TryGetComponent<ActionBase>(out var actionBase))
            {
                // Delay Time Return
                if (!ignoreDelayTime && _actionDelayTimer > 0) return;

                if (actionBase.ActionRequirement != null)
                    // アクションの条件チェック
                    if (!actionBase.ActionRequirement.CheckExecute(this, key))
                        return;

                //Debug.Log($"実行!!. {gameObject.name} {actionObject.name}");

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
                    var stateName = actionBase.ActionStart.PlayClip.name;
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

                    if (actionBase.ActionStart.IsParentEntity)
                        _unitComponents.PartAttachment.SetEntityParent();

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

        public async void CreatePrefab(FramePrefabData framePrefabData, GameObject unit)
        {
            //if (PrefabDatas.Count == 0) return;
            //if (PrefabDatas.Count <= listIndex) return;

            var data = framePrefabData.PrefabData;

            // 生成するAttackPointの特定
            GameObject pointObject = unit.gameObject;
            if (data.ParentKeyName != "")
                if (unit.TryGetComponent<UnitComponents>(out var unitComponents))
                {
                    var parent = unitComponents.UnitInstance.SearchObject(data.ParentKeyName);
                    if (parent != null)
                        pointObject = parent;
                }
            //// UnitBodyのアタックポイントを一つずつチェックして、一致するものをPointObjectに設定
            //if (_unitStatus.UnitBodys.Count != 0) // count0ならオブジェクトを設定
            //{
            //    foreach (var unitBody in _unitStatus.UnitBodys)
            //        if (data.BodyPoint == unitBody.BodyPoint)
            //            pointObject = unitBody.BodyObject;
            //}

            // オブジェクトの向く方向を指定
            Vector3 lookPos =
            UtilityFunction.LocalLookPos(unit.gameObject.transform, data.LocalPosition);

            // Position
            Vector3 pos = pointObject.transform.position + lookPos;

            GameObject prefab = null;
            if (data.Prefab != null)
                // Instantiate
                prefab = Instantiate(data.Prefab, pos, Quaternion.identity);

            // Rotation
            GameObject rotOrigin = gameObject;
            if (data.LookType == ELookType.Camera) // カメラならカメラの向きに依存
                rotOrigin = Camera.main.gameObject;
            Vector3 rot = rotOrigin.transform.localEulerAngles + data.LocalEulerAngle;




            // 効果音再生
            AudioManager.Instance.PlayOneShot(data.CreateSe, EAudioType.Se);

            if (prefab != null)
            {
                prefab.transform.rotation = Quaternion.Euler(rot);
                // Scale
                if (data.SetScale != Vector3.zero)
                    prefab.transform.localScale = data.SetScale;
                // Parent
                if (data.ParentType == EParentType.SetParent) // Parent
                    prefab.transform.parent = pointObject.transform;

                Destroy(prefab, data.DestroyTime);

            }

            // ダメージの設定
            //if (prefab.TryGetComponent<DamageDealer>(out var dealer))
            //{
            //    if (unit.TryGetComponent<IHealth>(out var health))
            //        dealer.AttackUnitType = health.UnitType;

            //    if (TryGetComponent<ActionDamageValue>(out var actionDamageValue))
            //    {
            //        dealer.DamageValue.OverrideDamageValue(actionDamageValue.DamageValue);
            //        dealer.DamageValue.AttackerUnit = unit;
            //    }
            //}

        }
    }
}