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
        private const string _unitTagName = "Unit";

        [Header("攻撃者")]
        public GameObject AttackerUnit;
        [Header("ダメージの重さ")]
        public int DamageWeight;
        [Header("ダメージに関する詳細")]
        public DamageData DamageData;
        [Space(10)]
        [Header("自動設定：攻撃者タイプ")]
        public develop_common.EUnitType AttackerUnitType;
        [Header("自動設定：攻撃判定")]
        public ReactiveProperty<bool> IsAttack = new ReactiveProperty<bool>();
        [Header("自動設定：攻撃判定時間")]
        public float AttackLifeTime;
        [Header("自動設定：ダメージアクション")]
        public GameObject DamageAction;
        [Header("自動設定：固定化フラグ")]
        public bool IsPull;
        [Header("自動設定：Pull情報")]
        public PullData PullData;
        [Header("自動設定：ActionLoader")]
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

        public void OnHit(GameObject hit)
        {
            LogManager.Instance.AddLog(hit.gameObject, $"${gameObject.name} Damage0:{IsAttack.Value}, {AttackLifeTime}");
            if (!IsAttack.Value) return;

            // HitCheckを行う
            bool check = true;
            // ダメージ回数などキャラクター情報を確認
            DamageUnit damageUnit = CheckDamageInfos(hit);
            LogManager.Instance.AddLog(damageUnit.UnitObject, "Damage1");

            // オブジェクトに触れたら消える場合
            if (DamageData.ObjectHitOff)
            {
                // タグがユニットじゃないならReturn
                check = check && !hit.gameObject.CompareTag(_unitTagName);
                if (!check)
                {
                    // Effect
                    Destroy(gameObject);
                    HitEffect(DamageData.DestroyEffect);
                }
            }

            if (damageUnit.UnitObject.TryGetComponent<IHealth>(out var health))
            {
                check = check && health.UnitType != AttackerUnitType; // 同じキャラクター同士じゃない
                check = check && damageUnit.HitCount <= DamageData.UnitLimit; // 上限超えてない
                check = check && damageUnit.HitTimer >= 0; // ヒットタイマーがリセットされていない
                if (!check) return;

                // エフェクト再生
                HitEffect(DamageData.HitEffect);

                if (DamageAction.TryGetComponent<ActionBase>(out var actionBase))
                {
                    // 即切り替えアクションがあるか？
                    var replay = actionBase.ActionRePlay;
                    GameObject replayAction = null;
                    if (replay != null)
                        replayAction = replay.RePlayAction;
                    if(replayAction != null && AttakerActionLoader != null)
                    {
                        AttakerActionLoader.LoadAction(replayAction);
                    }
                    else
                    {
                        // ヒット可能
                        damageUnit.HitCount++; // 回数加算
                        damageUnit.HitTimer = DamageData.HitSpanTime; // ダメージ間隔を上書き

                        int totalDamage = DamageWeight * actionBase.ActionDamageData.MotionDamage;
                        // ダメージを与える
                        health.TakeDamage(this, totalDamage);

                        if (IsPull) // 固定化ONの場合
                        {
                            if (damageUnit.UnitObject.TryGetComponent<develop_common.UnitComponents>(out var unitComponents))
                            {
                                var ran = UnityEngine.Random.Range(0, PullData.PullRots.Count);
                                var pos = PullData.PullPos;
                                unitComponents.PartAttachment.AttachTarget
                                    (transform, PullData.BodyKeyName,
                                    positionOffset: pos, rotationOffset: PullData.PullRots[ran]);
                            }
                        }
                    }


                }
            }
           



            // 追加ダメージハンドラ
            //OnDamageEvent?.Invoke(DamageData, damageUnit.UnitObject, gameObject);
            // オブジェクトの破棄
            //if (DamageData.UnitHitDestroy && damageUnit.UnitObject.CompareTag(_unitTagName))
            //    Destroy(gameObject);
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
        protected virtual DamageUnit CheckDamageInfos(GameObject target)
        {
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