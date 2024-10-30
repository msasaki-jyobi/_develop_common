using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace develop_common
{
    public class DamageDealer : MonoBehaviour
    {
        public EUnitType AttackUnitType;
        public DamageValue DamageValue;

        public Material PlayerMaterial;
        public Material EnemyMaterial;

        // 各オブジェクトのタグ名
        private const string _unitTagName = "Unit";

        // 触れたオブジェクトのダメージ管理
        private List<DamageUnit> _damageUnits = new List<DamageUnit>();
        private float _hitTimer = 0;

        public Action<DamageValue, GameObject, GameObject> OnDamageEvent;


        private void Start()
        {
            if(TryGetComponent<Renderer>(out var mat))
            {
                var targetMaterial = AttackUnitType == EUnitType.Player ? PlayerMaterial : EnemyMaterial;
                mat.material = targetMaterial;
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            HitCheck(collision.gameObject);
        }
        private void OnTriggerStay(Collider other)
        {
            HitCheck(other.gameObject);
        }
        private void Update()
        {
            ResetDamageUnits();
            HitTimeSubtraction();
        }
        private void OnDestroy()
        {
            UtilityFunction.PlayEffect(gameObject, DamageValue.DestroyEffect);
            AudioManager.Instance?.PlayOneShotClipData(DamageValue.DestroySE);
        }
        /// <summary>
        /// オブジェクトに触れた時に発動する処理
        /// </summary>
        /// <param name="hit"></param>
        public void HitCheck(GameObject hit)
        {
            // HitCheckを行う
            if (OnReturnCheck(hit)) return;
            // ダメージ回数などキャラクター情報を確認
            DamageUnit damageUnit = CheckDamageInfos(hit);
            // 攻撃判定を実行可能か判定 ヒット上限 ヒットタイマーも0
            bool hitPlayFlg = true;
            hitPlayFlg = hitPlayFlg && damageUnit.HitCount < DamageValue.HitLimit;
            hitPlayFlg = hitPlayFlg && damageUnit.HitTimer <= 0;
            hitPlayFlg = hitPlayFlg && damageUnit.UnitObject != DamageValue.AttackerUnit;

            // ダメージ実行
            if (hitPlayFlg)

                if (damageUnit.UnitObject.TryGetComponent<IHealth>(out var health))
                {
                    LogManager.Instance.AddLog(gameObject, "ダメージ実行");
                    // 同じキャラクタ同士ならReturn
                    if (health.UnitType == AttackUnitType)
                    {
                        LogManager.Instance.AddLog(gameObject, "同じキャラクタ同士ならReturn");
                        return;
                    }

                    // ヒット可能
                    damageUnit.HitCount++; // 回数加算
                    damageUnit.HitTimer = DamageValue.HitSpanTime; // ダメージ間隔を上書き

                    HitEffect(DamageValue.HitEffect);

                    // ダメージを与える
                    health.TakeDamage(DamageValue);

                    // 追加ダメージハンドラ
                    OnDamageEvent?.Invoke(DamageValue, damageUnit.UnitObject, gameObject);

                    // オブジェクトの破棄
                    if (DamageValue.UnitHitDestroy && damageUnit.UnitObject.CompareTag(_unitTagName))
                        Destroy(gameObject);
                }
        }

        /// <summary>
        /// ResetTimeに達したらダメージユニットをクリアする
        /// </summary>
        private void ResetDamageUnits()
        {
            _hitTimer += Time.deltaTime;
            if (_hitTimer > DamageValue.HitLimitResetTime)
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
            AudioManager.Instance.PlayOneShotClipData(DamageValue.HitSE);
        }

        protected virtual bool OnReturnCheck(GameObject hit)
        {
            // オブジェクトに触れたら消える場合
            if (DamageValue.ObjectHitDestroy)
            {
                // タグがユニットじゃないならReturn
                if (!hit.gameObject.CompareTag(_unitTagName))
                {
                    // Effect
                    Destroy(gameObject);
                    HitEffect(DamageValue.DestroyEffect);
                    return true;
                }
            }
            // 上限超えてたらReturn
            if (_damageUnits.Count >= DamageValue.UnitLimit)
            {
                return true;
            }
            return false;
        }
    }

}
