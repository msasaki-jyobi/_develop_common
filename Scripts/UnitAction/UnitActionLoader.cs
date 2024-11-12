using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using System;
using develop_tps;
using Cysharp.Threading.Tasks;
using static UnityEditor.PlayerSettings;
using System.Security.Cryptography.X509Certificates;
using RPGCharacterAnims.Actions;

namespace develop_common
{
    public class UnitActionLoader : MonoBehaviour
    {
        [SerializeField] private UnitComponents _unitComponents;
        public ReactiveProperty<EUnitStatus> UnitStatus = new ReactiveProperty<EUnitStatus>();
        [Tooltip("Damage:GetHit1(LoopOFF), 通常:Locomotion")]
        public bool NotHumanoid; // 人じゃない場合「Dogとかの場合」

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

        [Header("計算用")]
        public float DownTimer = 0;
        public float DownNoneActionTime = 0.5f;

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
                        if (_loadFrameInfos?.Count > 0)
                            foreach (var frameInfo in _loadFrameInfos)
                                if (_stateController.Frame.Value * _stateController.GetCurrentClipSpeed() >= frameInfo.PlayFrame)
                                    if (!frameInfo.IsComplete)
                                    {
                                        frameInfo.IsComplete = true;



                                        if (frameInfo.FramePair.Count > 0) // Pair
                                        {
                                            foreach (var framePair in frameInfo.FramePair)
                                                PairManager.Instance.PlayPair(_unitComponents, framePair.Key, framePair.Value);
                                        }

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
                                            if (_unitComponents.AttaqckIKController != null)
                                                if (frameInfo.IKData != null)
                                                    _unitComponents.AttaqckIKController.SetTargetEnableIK(
                                                         frameInfo.IKData.IKKeyName,
                                                         frameInfo.IKData.IKLifeTime,
                                                         TargetComponents.UnitInstance.SearchObject(frameInfo.IKData.IKTargetKeyName).transform
                                                        );
                                        }
                                        if (frameInfo.SyncData != null) // Syncの設定
                                        {
                                            // 近くの敵の部位を取得
                                            var target = GameObject.Find("Player");
                                            if (target != null)
                                            {
                                                if (target.TryGetComponent<develop_common.UnitComponents>(out var component))
                                                {
                                                    var body = component.UnitInstance.SearchObject(frameInfo.SyncData.DamageKeyName);
                                                    //transform.position = pos + frameInfo.SyncData.WorldOffset;
                                                    component.PartAttachment.ActivateAbility(
                                                         body.transform, // ターゲットBody用
                                                         _unitComponents.PartAttachment.PlayerRoot,
                                                         _unitComponents.AttackDealer.GetAttack(frameInfo.SyncData.AttackKeyName).transform, // 武器用
                                                         frameInfo.SyncData.WorldOffset
                                                         );
                                                    await UniTask.Delay(1);
                                                    component.PartAttachment.ActivateAbility(
                                                        body.transform, // ターゲットBody用
                                                        _unitComponents.PartAttachment.PlayerRoot,
                                                        _unitComponents.AttackDealer.GetAttack(frameInfo.SyncData.AttackKeyName).transform, // 武器用
                                                        frameInfo.SyncData.WorldOffset
                                                        );

                                                }
                                            }
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

                                        if (frameInfo.OverwriteAction != null)
                                        {
                                            LoadAction(frameInfo.OverwriteAction, ignoreRequirement: true);
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
                // Memo. バグ修正にはなるが、これにより、Vicのモーション終了時に強制で立ち上がるので、改めてコメント化
                ChangeStatus(EUnitStatus.Ready, 99);
            }

            DownTimer += Time.deltaTime;
        }

        private void FinishMotionEventHandle(string stateName, bool isLoop)
        {
            // Delay Time Return
            //if (_actionDelayTimer > 0) return;

            //Debug.Log($"GameObject:{gameObject.name} State: {stateName} 終了XXX");

            // Dogがダメージを受け終わった場合
            if (NotHumanoid && stateName == "GetHit1") // Dogとかの場合 ダメージが終了した場合
            {
                ChangeStatus(EUnitStatus.Ready, 1, null);
                _stateController.StatePlay("Locomotion", EStatePlayType.SinglePlay, resetMotion: true);
            }

            if (ActiveActionBase != null)
            {
                if (ActiveActionBase.ActionStart != null)
                {
                    if (ActiveActionBase.ActionStart.PlayClip == null) return;// 投げ技は↓でエラーなっちゃうから追加してみた
                    if (ActiveActionBase.ActionStart.PlayClip != stateName) return;
                }
            }

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
                    // Change State
                    ChangeStatus(oldActiveActionBase.ActionFinish.SetFinishStatus, 1, oldActiveActionObject);

                    // Next ActionData
                    if (oldActiveActionBase.ActionFinish.NextActionData != null)
                    {
                        LoadAction(oldActiveActionBase.ActionFinish.NextActionData);
                        return;
                    }

                    if (oldActiveActionBase.ActionFinish.IsDown)
                    {
                        _unitComponents.PartAttachment.IsDown = true;
                        _unitComponents.PartAttachment.SetEntityParent();
                    }

                    // 終了時にダウン状態になる場合
                    if (oldActiveActionBase.ActionFinish.SetFinishStatus == EUnitStatus.Down)
                        DownTimer = 0;
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

        public void LoadAction(GameObject actionObject, EInputReader key = EInputReader.None, bool ignoreDelayTime = false, bool ignoreRequirement = false)
        {
            if (actionObject == null) return;
            if (actionObject.TryGetComponent<ActionBase>(out var actionBase))
            {

                // Delay Time Return
                if (!ignoreDelayTime && _actionDelayTimer > 0) return;

                if (!ignoreRequirement)
                    if (actionBase.ActionRequirement != null)
                        // アクションの条件チェック
                        if (!actionBase.ActionRequirement.CheckExecute(this, key))
                            return;

                // ダウンDelay ダウン開始から0.5秒はretur, ダウン開始から5秒間は無敵
                if (DownTimer < DownNoneActionTime)
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
                    var stateName = actionBase.ActionStart.PlayClip != null ? actionBase.ActionStart.PlayClip : "";
                    var playType = actionBase.ActionStart.StatePlayType;
                    var reset = actionBase.ActionStart.IsStateReset;
                    var root = actionBase.ActionStart.IsApplyRootMotion;

                    var layer = actionBase.ActionStart.AnimatorLayer;

                    if (layer == 0)
                    {
                        // Dogのダメージを強制的に"GetHit1"にする
                        if (NotHumanoid && actionBase.ActionDamageData != null) // Dogとかの場合
                        {
                            stateName = "GetHit1";
                        }
                        _stateController.StatePlay(stateName, playType, reset, root);
                    }
                    else
                    {
                        _stateController.AnimatorLayerWeightPlay(layer, stateName,
                            actionBase.ActionStart.WeightValue, actionBase.ActionStart.WeightTime);
                    }

                    if (actionBase.ActionStart.IsParentEntity)
                        _unitComponents.PartAttachment.SetEntityParent();

                    if (actionBase.ActionStart.IsCameraReset)
                        develop_easymovie.CameraManager.Instance.SetDefaultCamera(false);

                    ChangeStatus(actionBase.ActionStart.SetStartStatus, 0, actionBase.gameObject);


                }
                // Frame
                if (actionBase.ActionFrame != null)
                    _loadFrameInfos = actionBase.ActionFrame.FrameInfo.Select(item => new FrameInfo(item)).ToList();

            }
        }

        public void ChangeStatus(EUnitStatus status, int code = 0, GameObject Action = null)
        {
            Debug.Log($"{gameObject.name} ChangeStatus:{status}, code:{code}, アクション：{Action}");
            if (status != EUnitStatus.None)
                UnitStatus.Value = status;

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