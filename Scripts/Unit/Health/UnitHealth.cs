using _develop_common;
using Cysharp.Threading.Tasks;
using develop_body;
using develop_tps;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace develop_common
{
    public enum EUnitType
    {
        Enemy,
        Player,
        Other
    }
    // PlayerHealth クラス
    public class UnitHealth : MonoBehaviour, IHealth
    {
        [Header("Player Only")]
        [SerializeField] private develop_tps.InputReader _inputReader;
        [Header("Player and Enemy")]
        [SerializeField] private UnitComponents _unitComponents;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private AnimatorStateController _animatorStateController;
        [Header("Player and Enemy")]
        public List<string> DeadMotions = new List<string>() { "Dead", };

        public GameObject GetUpAction;
        public bool NoneChangeIsKinetic;

        [SerializeField]
        private EUnitType _unitType = EUnitType.Player;
        public EUnitType UnitType => _unitType;

        public ReactiveProperty<EUnitStatus> UnitStatus => new ReactiveProperty<EUnitStatus>();

        [field: SerializeField] public int CurrentHealth { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;

        [field: SerializeField] public bool IsInvisible { get; private set; }
        public float InVisibleTimer = 0;

        public bool IsDeadSlow;
        public float SlowTimer = 1;
        [SerializeField] private List<ShakeController> Shakes = new List<ShakeController>();

        public event Action<int> DamageActionEvent;
        public ReactiveProperty<bool> InitDead = new ReactiveProperty<bool>();

        public event Action DeadEvent;

        private void Start()
        {
            if (_unitComponents != null)
                if (_unitComponents.UnitActionLoader != null)
                    _unitComponents.UnitActionLoader.FrameFouceEvent += OnFrameFouceHandle;

            if (_inputReader != null)
                _inputReader.PrimaryActionCrossEvent += OnCrossHandle;
        }

        private void Update()
        {
            //if (SlowTimer > 0)
            //{
            //    Time.timeScale = 0.2f;
            //    SlowTimer -= Time.unscaledDeltaTime;
            //}
            //else if (Time.timeScale != 1)
            //{
            //    Time.timeScale = 1f;
            //}

            if (InVisibleTimer > 0) InVisibleTimer -= Time.deltaTime;
            IsInvisible = InVisibleTimer > 0;
        }

        private async void OnCrossHandle(bool arg1, EInputReader reader)
        {
            if (CurrentHealth <= 0) return;
            // 起き上がる　これをGetUpのアクションにそもそもすればいいんじゃね？条件チェックでIsDownがTrueでDownValueが0ならこれみたいな
            if (_unitComponents.PartAttachment.IsPull || (_unitComponents.PartAttachment.IsDown && _unitComponents.UnitActionLoader.UnitStatus.Value == EUnitStatus.Down))
            {
                // ダウン時間から0.5未満ならReturn
                if (_unitComponents.UnitActionLoader.DownTimer <= _unitComponents.UnitActionLoader.DownNoneActionTime) return;

                _unitComponents.PartAttachment.SetEntityParent();
                await UniTask.Delay(1);
                _unitComponents.PartAttachment.SetEntityParent(); // なぜかここでも呼ばないと親オブジェクト解除されない
                if (!NoneChangeIsKinetic)
                    _rigidBody.isKinematic = false;
                // 角度を修正
                Vector3 rot = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, rot.y, 0);
                // 起き上がりモーションを実行
                _unitComponents.UnitActionLoader.LoadAction(GetUpAction);
                InVisibleTimer = 3f;
            }
        }

        public async void TakeDamage(GameObject damageAction, bool isPull, int totalDamage, bool InitRandomCamera = false, List<string> bodyNames = default)
        {
            //if (_unitComponents.UnitActionLoader.DownTimer >= 5f) // 無敵時間中ならReturn 
            //    return;

            DamageActionEvent?.Invoke(totalDamage);

            CurrentHealth -= totalDamage;

            foreach (var shake in Shakes)
                shake.ShakeActionMove();

            if (CurrentHealth < 0)
            {
                // 死亡
                if (CurrentHealth <= 0)
                {
                    InitDead.Value = true;
                    if (_unitComponents.UnitVoice != null)
                        if (!InitDead.Value)
                        {
                            _unitComponents.UnitVoice.PlayVoice("Dead", true);
                            DeadEvent?.Invoke();
                        }
                }

                if (IsDeadSlow)
                    SlowTimer = 3f;

                // 立ち上がっていたらDeadモーション
                if (_unitComponents.AnimatorStateController.MainStateName.Value == "Locomotion")
                {
                    _rigidBody.velocity = Vector3.zero;
                    string deadMotion = DeadMotions[UnityEngine.Random.Range(0, DeadMotions.Count)];
                    _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                    _unitComponents.AnimatorStateController.StatePlay(deadMotion, EStatePlayType.SinglePlay, true);

                    // Infinityのダメージ リストからランダム化でもいいかも
                }
                // 敵が攻撃中Locomotionじゃないので死なないため
                if (_unitComponents.UnitActionLoader.NotHumanoid)
                {
                    if (_unitComponents.AnimatorStateController.MainStateName.Value != "Dead") // Generic 死亡モーションリピート防止
                    {
                        _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                        _unitComponents.AnimatorStateController.StatePlay("Dead", EStatePlayType.SinglePlay, true);
                    }
                }
            }




            // Task: ダメージを受け取ってモーション再生を行う・シェイプ再生など
            if (damageAction.TryGetComponent<ActionBase>(out var actionBase))
            {
                //if (actionBase.ActionDamageData.DamageType == EDamageType.Additive)
                //    _animatorStateController.AnimatorLayerPlay(1, actionBase.ActionStart.PlayClip, 0f);


                var additiveMotion = "";
                int ran = 0;
                if (actionBase.ActionStart != null)
                    additiveMotion = actionBase.ActionStart.PlayClip; // いったんStartモーションを設定
                if (actionBase.ActionDamageData.AdditiveDamageData != null)
                    ran = UnityEngine.Random.Range(0, actionBase.ActionDamageData.AdditiveDamageData.AdditiveDatas.Count);
                // Additiveデータがあれば上書き
                if (actionBase.ActionDamageData != null)
                    if (actionBase.ActionDamageData.AdditiveDamageData != null)
                        additiveMotion = actionBase.ActionDamageData.AdditiveDamageData.AdditiveDatas[ran];

                if (actionBase.ActionDamageData.IsAddAddtive)
                {
                    _animatorStateController.AnimatorLayerPlay(1, additiveMotion, 0f);
                }
                if (actionBase.ActionDamageData.DamageVoiceKey != "")
                {
                    if (_unitComponents.UnitVoice != null)
                        _unitComponents.UnitVoice.PlayVoice(actionBase.ActionDamageData.DamageVoiceKey);
                }

                if (!isPull) // 固定化モーション以外を再生の場合
                {
                    // Additiveを考慮する必要があるが、とりあえず引き離す必要がある吹き飛ばす
                    if (!NoneChangeIsKinetic)
                        _rigidBody.isKinematic = false;
                    _unitComponents.PartAttachment.SetEntityParent();
                    //Vector3 rot = transform.rotation.eulerAngles;
                    //transform.rotation = Quaternion.Euler(0, rot.y, 0);

                    // グラップモーションの場合　座標と回転値を考慮する必要がある or グラップモーションをPullとして扱う
                    if (actionBase.ActionDamageData.IsAddAddtiveOnly) return;

                    if (CurrentHealth > 0)
                    {
                        // ノーマルモーション・グラップモーションを再生
                        _unitComponents.UnitActionLoader.LoadAction(damageAction);
                    }
                    else // 死亡した場合
                    {
                        _rigidBody.velocity = Vector3.zero;

                        // 別モーション再生中ならAdditiveのみ
                        _animatorStateController.AnimatorLayerPlay(1, $"Additive Damage0{UnityEngine.Random.Range(0, 3)}", 0f);

                        // 立ち上がっていたらDeadモーション
                        if (_unitComponents.AnimatorStateController.MainStateName.Value == "Locomotion")
                        {
                            if (actionBase.ActionDamageData.DeadAction != null) // 死亡データがある場合
                                _unitComponents.UnitActionLoader.LoadAction(actionBase.ActionDamageData.DeadAction);
                            else // DeadMotionがない場合
                            {
                                string deadMotion = DeadMotions[UnityEngine.Random.Range(0, DeadMotions.Count)];
                                _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                                _unitComponents.AnimatorStateController.StatePlay(deadMotion, EStatePlayType.SinglePlay, true);
                            }
                        }
                    }

                }
                else // 固定化モーションを再生の場合
                {
                    if (!_unitComponents.PartAttachment.IsPull) // まだ固定されていない
                    {
                        if (!NoneChangeIsKinetic)
                            _rigidBody.isKinematic = true;
                        _unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Executing;
                        _unitComponents.AnimatorStateController.StatePlay(actionBase.ActionStart.PlayClip, EStatePlayType.SinglePlay, true);
                        // HitCollider：ランダムカメラなら
                        if (InitRandomCamera)
                        {
                            PairManager.Instance.PlayRandomCamera(_unitComponents);
                        }
                    }
                    else // すでに固定化済み
                    {
                        _animatorStateController.AnimatorLayerPlay(1, additiveMotion, 0);
                    }


                }
            }

            var unitShape = _unitComponents.UnitShape;
            if (unitShape != null)
            {
                ShapeWordData word = new ShapeWordData();
                word.WordData = "Da";
                word.NotWardData = new List<string>() { "通常" };
                if (CurrentHealth <= 0)
                    word.NotWardData.Add("開");
                unitShape.SetShapeWard(word);
            }
        }

        public void Heal(float amount)
        {
            CurrentHealth += (int)amount;
            // 最大値を超えないようにする
        }

        public void ChangeStatus(EUnitStatus status)
        {

        }
        private void OnFrameFouceHandle(Vector3 power)
        {
            if (_rigidBody != null)
                Knockback(power);
        }
        /// <summary>
        /// Velocity knockback
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="force"></param>
        public void Knockback(Vector3 direction)
        {
            // プレイヤーが向いている方向に合わせて力を加える
            Vector3 localForce = transform.forward * direction.z + transform.right * direction.x + transform.up * direction.y;
            // Rigidbody に力を加える (Impulse モードで瞬間的に力を加える)
            _rigidBody.AddForce(localForce, ForceMode.Impulse);
        }
    }
}
