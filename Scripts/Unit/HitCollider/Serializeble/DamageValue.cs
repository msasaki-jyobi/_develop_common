using System;
using System.Collections;
using UnityEngine;

namespace develop_common
{


    [Serializable]
    public class DamageValue
    {
        // 攻撃者をを識別するID
        //public int AttakeID; オブジェクトから取得させる
        public bool IsCheckAttackMode; // 攻撃者が攻撃判定状態なら　角　足 斧 攻撃モーションを実行させる際に、角はModeA　足はModeBとか？　ModeAの状態（吹き飛び）　ModeBの内容（くっつく）　ModeCの状態（Additive)　ModeDの状態（掴み開始）　
        public UnitActionLoader AttackUnitLoader;
        public UnitActionLoader DamageUnitLoader;



        public GameObject AttackerUnit;
        public GameObject DamageAction; 
        public GameObject DeadAction;
        // ダメージ量
        public EDamageType DamageType;
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