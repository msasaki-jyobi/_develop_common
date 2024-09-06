using System;
using System.Collections;
using UnityEngine;

namespace GameSet.Common
{
    public enum EDamageID
    { 
        Normal = 0,
        Blow = 1,
    }



    [Serializable]
    public class DamageValue
    {
        // 攻撃者をを識別するID
        //public int AttakeID; オブジェクトから取得させる
        public GameObject AttackerUnit;
        public GameObject DamageAction; 
        public GameObject DeadAction;
        // ダメージ量
        public int Amount = 1;
        public int WeightDiff = 1; // 重さの倍率
        public int HitLimit = 1;
        public int UnitLimit = 5;
        // ユニットへのヒット上限
        // 再ヒットまでの時間間隔
        public float HitSpanTime = 0.1f;
        // 再ヒットまでの時間間隔　のリセット時間
        public float HitLimitResetTime = 6f;
        
        
        
        
        
        // ヒットエフェクト
        public GameObject HitEffect;
        // ヒット効果音
        public ClipData HitSE;
        // destroyエフェクト
        public GameObject DestroyEffect;
        // destroy効果音
        public ClipData DestroySE;
        // 触れたら削除する
        public bool UnitHitDestroy;
        public bool ObjectHitDestroy;

        public void OverrideDamageValue(DamageValue damageValue)
        {
            this.DamageAction = damageValue.DamageAction;
            this.DeadAction = damageValue.DeadAction;
            this.Amount = damageValue.Amount;
            this.WeightDiff = damageValue.WeightDiff;
            this.HitLimit = damageValue.HitLimit;
            this.UnitLimit = damageValue.UnitLimit;
            this.HitSpanTime = damageValue.HitSpanTime;
            this.HitLimitResetTime = damageValue.HitLimitResetTime;
            this.HitEffect = damageValue.HitEffect;
            this.HitSE = damageValue.HitSE;
            this.DestroyEffect = damageValue.DestroyEffect;
            this.DestroySE = damageValue.DestroySE;
            this.UnitHitDestroy = damageValue.UnitHitDestroy;
            this.ObjectHitDestroy = damageValue.ObjectHitDestroy;
        }

    }
}