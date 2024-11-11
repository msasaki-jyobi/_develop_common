using Cysharp.Threading.Tasks;
using develop_body;
using develop_common;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
//using DamageValue = develop_common.DamageValue;

namespace _develop_common
{
    public class HitCollider : MonoBehaviour
    {
        // 攻撃者を識別するID
        //public int AttakeID; オブジェクトから取得させる
        //public bool IsCheckAttackMode; // 攻撃者が攻撃判定状態なら　角　足 斧 攻撃モーションを実行させる際に、角はModeA　足はModeBとか？　ModeAの状態（吹き飛び）　ModeBの内容（くっつく）　ModeCの状態（Additive)　ModeDの状態（掴み開始）　
        //public UnitActionLoader AttackUnitLoader;
        //public UnitActionLoader DamageUnitLoader;

        // 各オブジェクトのタグ名
        private const string _bodyTagName = "Body";

        [Header("ダメージの重さ")]
        [Tooltip("ダメージ計算に利用される")]
        public int DamageWeight;
        [Header("ダメージに関する詳細")]
        [Tooltip("エフェクトやヒット上限の詳細情報データ")]
        public DamageData DamageData;
        [Space(10)]
        [Header("自動設定：攻撃者タイプ")]
        [Tooltip("ダメージ側が一致しないタイプなら、ダメージ発生する")]
        public develop_common.EUnitType AttackerUnitType;
        [Header("自動設定：攻撃判定")]
        public ReactiveProperty<bool> IsAttack = new ReactiveProperty<bool>();
        [Header("自動設定：攻撃判定時間")]
        [Tooltip("攻撃判定がONになっている時間")]
        public float AttackLifeTime;
        [Header("自動設定：ダメージアクション")]
        [Tooltip("ヒットしたキャラクターに再生させるダメージアクション")]
        public GameObject DamageAction;
        [Header("自動設定：固定化フラグ")]
        public bool IsPull;
        [Tooltip("固定化させる情報")]
        public PullData PullData;
        [Header("自動設定：ActionLoader")]
        [Tooltip("Replay:即切り替えモーションがあるかどうか、の時だけ利用する。")]
        public UnitActionLoader AttakerActionLoader;


        // 触れたオブジェクトのダメージ管理
        private List<DamageUnit> _damageUnits = new List<DamageUnit>();
        private float _hitTimer = 0;

        //public Action<DamageValue, GameObject, GameObject> OnDamageEvent;


        // DamageCatalog (全体で共有） EDamageMode
        // ModeA: Additive 他のユニットの影響　[]Normal []Catch []Down
        // ModeB: DamageA
        // ModeC: 打ち上げA
        // ModeD: 打ち上げB
        // ModeE: 打ち上げC

        // DamageModeInfo
        // EDamageMode
        // DamagValue

        // 変数
        // ダメージが発生するHitColliderリスト string:EDamageMode
        private void Start()
        {
            IsAttack
                .Subscribe((x) =>
                {
                    _damageUnits.Clear();
                });
        }
        private void Update()
        {
            IsAttack.Value = AttackLifeTime > 0;
            if (AttackLifeTime > 0)
                AttackLifeTime -= Time.deltaTime;

            ResetDamageUnits();
            HitTimeSubtraction();
        }

        private void OnCollisionStay(Collision collision)
        {
            OnHit(collision.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            OnHit(other.gameObject);
        }

        public async void OnHit(GameObject hit)
        {
            //LogManager.Instance.AddLog(hit.gameObject, $"${gameObject.name} Damage0:{IsAttack.Value}, {AttackLifeTime}");

            if (!IsAttack.Value) return;
            bool check = true; // HitCheckを行う

            if (hit.TryGetComponent<develop_body.BodyCollider>(out var bodyCollider))
            {
                if (bodyCollider.RootObject.TryGetComponent<IHealth>(out var health))
                {

                    DamageUnit damageUnit = CheckDamageInfos(hit, bodyCollider, health); // ダメージ回数などキャラクター情報を確認
                    if (damageUnit == null) return;

                    // オブジェクトに触れたら消える場合
                    if (DamageData.ObjectHitOff)
                    {
                        // タグがダメージを受けるタグじゃないならReturn
                        check = check && !hit.gameObject.CompareTag(_bodyTagName);
                        if (!check)
                        {
                            // Effect
                            Destroy(gameObject);
                            HitEffect(DamageData.DestroyEffect);
                        }
                    }


                    //check = check && health.UnitType != AttackerUnitType; // 同じキャラクター同士じゃない
                    check = check && _damageUnits.Count <= DamageData.UnitLimit; // ユニット数ヒットリミット ADD
                    check = check && damageUnit.HitCount <= DamageData.HitLimit; // 上限超えてない
                    check = check && damageUnit.HitTimer <= 0; // ヒットタイマーがリセットされていない
                    check = check && !health.IsInvisible;

                    Debug.Log($"Damage**{bodyCollider.RootObject.name}**, {health.UnitType != AttackerUnitType}.{damageUnit.HitCount <= DamageData.UnitLimit},{damageUnit.HitTimer >= 0},{!health.IsInvisible},");
                    if (!check) return;

                    // ヒット可能
                    damageUnit.HitCount++; // 回数加算 なぜか数ヒットしてる１なのに
                    damageUnit.HitTimer = DamageData.HitSpanTime; // ダメージ間隔を上書き
                    // エフェクト再生
                    HitEffect(DamageData.HitEffect);
                    //DamagePlay(health, damageUnit.UnitObject); // 触れたオブジェクトを渡す
                    DamagePlay(health, bodyCollider.RootObject); // 触れたBodyの親オブジェクトを渡す
                }
            }

            // 追加ダメージハンドラ
            //OnDamageEvent?.Invoke(DamageData, damageUnit.UnitObject, gameObject);
            // オブジェクトの破棄
            //if (DamageData.UnitHitDestroy && damageUnit.UnitObject.CompareTag(_unitTagName))
            //    Destroy(gameObject);
        }

        /// <summary>
        /// ダメージ処理を実行させる
        /// </summary>
        /// <param name="health"></param>
        /// <param name="damageUnit"></param>
        public async void DamagePlay(IHealth health, GameObject damageUnit)
        {
            if (DamageAction != null)
                if (DamageAction.TryGetComponent<ActionBase>(out var actionBase))
                {
                    // Task: HitCollider 技を受け取って、TakeDamageに渡す、固定化を行う
                    //Debug.Log($"a::::{AttakerActionLoader.ActiveActionBase.ActionRePlay.RePlayAction}");
                    //Debug.Log($"b::::{AttakerActionLoader.ActiveActionBase.name}");
                    // のけぞる渡されとる！
                    //LogManager.Instance.AddLog(gameObject, $"{actionBase.ActionRePlay != null} x", 3);


                    // 即切り替えアクションがあるか？
                    if (AttakerActionLoader != null)
                        if (AttakerActionLoader.ActiveActionBase.ActionRePlay != null)
                        {
                            //LogManager.Instance.ConsoleLog(gameObject, $"{AttakerActionLoader.ActiveActionBase.ActionRePlay.RePlayAction.name} x", 3);
                            if (AttakerActionLoader.ActiveActionBase.ActionRePlay.RePlayAction.TryGetComponent<ActionGrap>(out var grep))
                            {
                                Debug.Log($"a::::{AttakerActionLoader.ActiveActionBase.ActionRePlay.RePlayAction}");
                                Debug.Log($"b::::{AttakerActionLoader.ActiveActionBase.name}");
                                // 攻撃者投げ
                                AttakerActionLoader.LoadAction(AttakerActionLoader.ActiveActionBase.ActionRePlay.RePlayAction, ignoreDelayTime: true);

                                var stateName = grep.GrapClip;
                                var pos = grep.OffsetPos;
                                var rotOffset = grep.OffsetRot;

                                // ダメージ側もダメージモーション実行
                                if (damageUnit.TryGetComponent<develop_common.UnitComponents>(out var unitComponents))
                                {
                                    // AttakerActionLoader.ActiveActionBase.ActionRePlay この時点でかわってしまう Attに

                                    unitComponents.UnitActionLoader.UnitStatus.Value = EUnitStatus.Down;
                                    unitComponents.AnimatorStateController.StatePlay(stateName, EStatePlayType.SinglePlay, true);

                                    damageUnit.transform.position = transform.position + UtilityFunction.LocalLookPos(transform, pos);
                                    Vector3 rot = transform.rotation.eulerAngles;
                                    damageUnit.transform.rotation = Quaternion.Euler(rot + rotOffset);
                                    if (damageUnit.TryGetComponent<Rigidbody>(out var rigid)) rigid.velocity = Vector3.zero;

                                    // Task:これをモーション終了時に付与すれば良い
                                    unitComponents.PartAttachment.IsDown = true;
                                }
                            }

                            return;
                        }




                    // 投げ技ではないダメージ判定
                    int totalDamage = DamageWeight * actionBase.ActionDamageData.MotionDamage;

                    // ダメージを与えモーションも実行してもらう
                    health.TakeDamage(DamageAction, IsPull, totalDamage);

                    if (damageUnit.TryGetComponent<EnemyAI>(out var enemyAI))
                    {
                        enemyAI.TakeDamage(totalDamage);
                    }

                    if (IsPull) // 固定化ONの場合
                    {
                        if (damageUnit.TryGetComponent<develop_common.UnitComponents>(out var unitComponents))
                        {
                            // Pull
                            var ran = UnityEngine.Random.Range(0, PullData.PullRots.Count);
                            var pos = PullData.PullPos;
                            Vector3 rot = PullData.PullRots[ran];
                            rot.x = PullData.RandomRotX ? UnityEngine.Random.Range(0, 360) : rot.x;
                            rot.y = PullData.RandomRotY ? UnityEngine.Random.Range(0, 360) : rot.y;
                            rot.z = PullData.RandomRotZ ? UnityEngine.Random.Range(0, 360) : rot.z;
                            unitComponents.PartAttachment.AttachTarget
                                (transform, PullData.BodyKeyName,
                                positionOffset: pos, rotationOffset: rot);
                            Debug.Log($"DEDE::{PullData.PullRots[ran]}");
                        }
                    }

                }
        }

        /// <summary>
        /// ResetTimeに達したらダメージユニットをクリアする
        /// </summary>
        private void ResetDamageUnits()
        {
            _hitTimer += Time.deltaTime;
            if (_hitTimer > DamageData.HitLimitResetTime)
            {
                _hitTimer = 0;
                _damageUnits.Clear();
            }
        }
        /// <summary>
        /// 各ユニットのヒットタイムを減算する
        /// </summary>
        private void HitTimeSubtraction()
        {
            if (_damageUnits.Count != 0)
            {
                for (int i = 0; i < _damageUnits.Count; i++)
                {
                    // ヒット時間中なら、再ヒット可能まで減算
                    if (_damageUnits[i].HitTimer >= 0)
                    {
                        // 時間を減算
                        _damageUnits[i].HitTimer -= Time.deltaTime;
                    }
                }
            }
        }
        /// <summary>
        /// ダメージを与えたキャラクターが追加済みかチェックし、未追加の場合は追加も行う
        /// </summary>
        protected virtual DamageUnit CheckDamageInfos(GameObject target, develop_body.BodyCollider body, IHealth health)
        {
            if (body != null)
            {
                // 同じタイプならReturn
                if (AttackerUnitType == health.UnitType) return null;

                target = body.RootObject;

                for (int i = 0; i < _damageUnits.Count; i++)
                {
                    // 存在した場合は、該当キャラクターを返す
                    if (target == _damageUnits[i].UnitObject)
                        return _damageUnits[i];
                }

                // 存在しない場合追加
                DamageUnit damageUnit = new DamageUnit();
                damageUnit.UnitObject = target;
                _damageUnits.Add(damageUnit);
                return damageUnit;
            }

            return null;
        }

        private void HitEffect(GameObject effect)
        {
            // HitEffect
            UtilityFunction.PlayEffect(gameObject, effect);
            // HitSE再生
            AudioManager.Instance.PlayOneShotClipData(DamageData.HitSE);
        }

        //protected virtual bool OnReturnCheck(GameObject hit)
        //{
        //    bool check = true;
        //    // ダメージ回数などキャラクター情報を確認
        //    DamageUnit damageUnit = CheckDamageInfos(hit);

        //    // オブジェクトに触れたら消える場合
        //    if (DamageData.ObjectHitOff)
        //    {
        //        // タグがユニットじゃないならReturn
        //        check = check && !hit.gameObject.CompareTag(_unitTagName);
        //        if (!check)
        //        {
        //            // Effect
        //            Destroy(gameObject);
        //            HitEffect(DamageData.DestroyEffect);
        //        }
        //    }

        //    if (damageUnit.UnitObject.TryGetComponent<IHealth>(out var health))
        //        check = check && health.UnitType == AttackerUnitType; // 同じキャラクター同士
        //    check = check && damageUnit.HitCount >= DamageData.UnitLimit; // 上限超えてたらReturn
        //    check = check && damageUnit.HitTimer <= 0; // ヒットタイマーがリセットされていない


        //    return check;
        //}
    }

}