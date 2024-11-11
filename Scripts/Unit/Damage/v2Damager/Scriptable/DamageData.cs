using develop_common;
using System.Collections;
using UnityEngine;

namespace develop_common
{

    [CreateAssetMenu(fileName = "DamageData", menuName = "develop_common / DamageData")]
    public class DamageData : ScriptableObject
    {

        [Header("ユニットごとのヒット上限")]
        public int HitLimit = 1;
        [Header("再ヒットまでの間隔")]
        public float HitSpanTime = 0.1f;
        [Header("HitLimitを完全リセットするまでの時間")]
        public float HitLimitResetTime = 6f;
        [Space(10)]
        [Header("ヒットするユニット数の上限")]
        public int UnitLimit = 5;

        [Space(10)]
        [Header("エフェクト・演出関連")]
        public GameObject HitEffect;
        public ClipData HitSE;
        public GameObject DestroyEffect;
        public ClipData DestroySE;

        [Space(10)]
        [Header("Unitや壁 一回きりで消える設定")]
        public bool UnitHitOFF;
        public bool ObjectHitOff;

        public void OverrideDamageValue(DamageData damageValue)
        {
            this.HitLimit = damageValue.HitLimit;
            this.UnitLimit = damageValue.UnitLimit;
            this.HitSpanTime = damageValue.HitSpanTime;
            this.HitLimitResetTime = damageValue.HitLimitResetTime;
            this.HitEffect = damageValue.HitEffect;
            this.HitSE = damageValue.HitSE;
            this.DestroyEffect = damageValue.DestroyEffect;
            this.DestroySE = damageValue.DestroySE;
            this.UnitHitOFF = damageValue.UnitHitOFF;
            this.ObjectHitOff = damageValue.ObjectHitOff;
        }
    }
}
